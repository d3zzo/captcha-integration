using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace captcha_integration.Core
{
    public class VisualCaptcha
    {
        public const int DefaultNumberOfImages = 5;

        // Dependency required to store session state.
        private readonly ISessionProvider _sessionProvider;

        /// Session namespace, used for filtering session data across multiple captchas
        private readonly string _namespace;

        private string __dirname;

        /// <summary>
        /// Object that will have a reference for the session object
        /// It will have .visualCaptcha.images, .visualCaptcha.audios, .visualCaptcha.validImageOption, and .visualCaptcha.validAudioOption
        /// </summary>
        public VisualCaptchaSession Session
        {
            get { return _sessionProvider.GetSession(_namespace); }
            set { _sessionProvider.SetSession(_namespace, value); }
        }

        /// <summary>
        /// All the image options.
        /// These can be easily overwritten or extended or replaced
        /// By default, they're populated using the ./images.json file
        /// </summary>
        public List<Option> ImageOptions { get; set; }

        /// <summary>
        /// All the audio options.
        /// These can be easily overwritten or extended or replaced
        /// By default, they're populated using the ./audios.json file
        /// </summary>
        public List<Option> AudioOptions { get; set; }

        public VisualCaptcha(ISessionProvider sessionProvider, string mediaPath, string @namespace = "visualcaptcha", List<Option> images = null, List<Option> audios = null)
        {
            _sessionProvider = sessionProvider;
            __dirname = mediaPath;
            this.ImageOptions = images ?? LoadImageOptions();
            this.AudioOptions = audios ?? LoadAudioOptions();
            _namespace = @namespace;
        }

        // Generate a new valid option
        // @param numberOfOptions is optional. Defaults to 5
        public void Generate(int numberOfOptions = DefaultNumberOfImages)
        {
            var visualCaptchaSession = new VisualCaptchaSession();

            // Shuffle all imageOptions
            ImageOptions = ImageOptions.Shuffle().ToList();

            if (numberOfOptions < 2)
            {
                numberOfOptions = 2;
            }

            // Get a random sample of X images
            // AND Set a random value for each of the images, to be used in the frontend
            visualCaptchaSession.Images = ImageOptions.Shuffle()
                .Take(numberOfOptions)
                .Select(x => new Option { name = x.name, path = x.path, value = Extensions.GenerateRandomKey(20) })
                .ToList();

            // Select a random image option, for the current valid image option (but not the last one)
            visualCaptchaSession.ValidImageOption =
                visualCaptchaSession.Images.Except(new[] { visualCaptchaSession.ValidImageOption }).Shuffle().First();

            // Select a random audio option, for the current valid audio option (but not the last one)
            visualCaptchaSession.ValidAudioOption =
                AudioOptions.Except(new[] { visualCaptchaSession.ValidAudioOption }).Shuffle().First();

            // Set random hashes for audio and image field names, and add it in the frontend data object
            visualCaptchaSession.FrontendData = new FrontendData()
            {
                values = visualCaptchaSession.Images.Select(x => x.value).ToList(),
                imageName = visualCaptchaSession.ValidImageOption.name,
                imageFieldName = Extensions.GenerateRandomKey(20),
                audioFieldName = Extensions.GenerateRandomKey(20),
            };
            Session = visualCaptchaSession;
        }

        private List<Option> LoadAudioOptions()
        {
            var json = File.ReadAllText(Path.Combine(__dirname, "audios.json"));
            return JsonConvert.DeserializeObject<List<Option>>(json);
        }

        private List<Option> LoadImageOptions()
        {
            var json = File.ReadAllText(Path.Combine(__dirname, "images.json"));
            return JsonConvert.DeserializeObject<List<Option>>(json);
        }

        public bool ValidateImage(string sentOption)
        {
            return (sentOption == Session.ValidImageOption.value);
        }

        public bool ValidateAudio(string sentOption)
        {
            return (sentOption == Session.ValidAudioOption.value);
        }

        public FileStream StreamAudio(string fileType)
        {
            if (Session.ValidAudioOption.path == null)
            {
                return null;
            }

            var audioFilePath = Path.Combine(__dirname, "audios", Session.ValidAudioOption.path);

            if (fileType == "ogg")
            {
                audioFilePath = audioFilePath.Replace(".mp3", ".ogg");
            }

            return GetFileStream(audioFilePath);
        }

        public FileStream StreamImage(int index, bool isRetina)
        {
            var imageFilePath = Path.Combine(__dirname, "images", Session.Images[index].path);
            if (isRetina)
            {
                imageFilePath = imageFilePath.Replace(".png", "@2x.png");
            }
            return GetFileStream(imageFilePath);
        }

        private static FileStream GetFileStream(string audioFilePath)
        {
            var file = new FileInfo(audioFilePath);

            if (file.Exists == false)
            {
                return null;
            }

            return new FileStream(file.FullName, FileMode.Open);
        }

        public string GetImageMimeType(int index, bool isRetina)
        {
            return "image/png";
            throw new System.NotImplementedException();
        }

        public string GetAudioMimeType(string type)
        {
            if (type.ToLower() == "ogg")
            {
                return "audio/ogg";
            }
            return "audio/mpeg";
        }

        public CaptchaState ValidateAnswer(NameValueCollection form)
        {
            var frontendData = Session.FrontendData;
            if (frontendData == null) return CaptchaState.NoCaptcha;

            var imageAnswer = form[frontendData.imageFieldName];
            var audioAnswer = form[frontendData.audioFieldName];

            if (imageAnswer != null)
                return ValidateImage(imageAnswer)
                    ? CaptchaState.ValidImage
                    : CaptchaState.FailedImage;

            if (audioAnswer != null)
                // We set lowercase to allow case-insensitivity, but it's actually optional
                return ValidateAudio(audioAnswer.ToLower())
                    ? CaptchaState.ValidAudio
                    : CaptchaState.FailedAudio;

            return CaptchaState.GeneralFail;
        }
    }
}