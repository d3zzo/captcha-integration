using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using captcha_integration.Core;
using captcha_integration.Models;

namespace captcha_integration.Controllers
{
    public class HomeController : Controller
    {
        private readonly VisualCaptcha _visualCaptcha;


        public HomeController() : this(new HttpContextSession())
        {

        }
        public HomeController(ISessionProvider sessionProvider)
        {
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
            _visualCaptcha = new VisualCaptcha(sessionProvider, mediaPath);
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Start(int noImages = VisualCaptcha.DefaultNumberOfImages)
        {
            _visualCaptcha.Generate(noImages);
            return Json(_visualCaptcha.Session.FrontendData, JsonRequestBehavior.AllowGet);
        }

        public FileStreamResult Image(int index) //, bool isRetina = false) //isRetina should be on querystring.
        {
            return new FileStreamResult(_visualCaptcha.StreamImage(index, false), _visualCaptcha.GetImageMimeType(index, false));
            //return new FileStreamResult(_visualCaptcha.StreamImage(index, isRetina), _visualCaptcha.GetImageMimeType(index, isRetina));
        }

        public FileStreamResult Audio(string type = "mp3")
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return new FileStreamResult(_visualCaptcha.StreamAudio(type), _visualCaptcha.GetAudioMimeType(type));
        }

        [HttpPost]
        public JsonResult Try(UserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadGateway;
                return Json(new { Valid = ModelState.IsValid });
                
                //return Json(new
                //{
                //    Valid = ModelState.IsValid,
                //    Errors = GetErrorsFromModelState(),
                //    //StudentsPartial = studentPartialViewHtml
                //});
            }
            else //if (viewModel.formValidated == true)
            {
                var result = _visualCaptcha.ValidateAnswer(Request.Form);
                var queryParams = new NameValueCollection();
                if (result == CaptchaState.ValidImage || result == CaptchaState.ValidAudio)
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("Confirmed", "Form"),
                        isRedirect = true
                    });
                }
                else
                {
                    ModelState.AddModelError("Captcha", "Provided captcha is invalid.");
                    return Json(new { Valid = false, Errors = true, ErrorMsg = "Invalid captcha" });
                }
            }
        }

        public Dictionary<string, object> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in ModelState.Keys)
            {
                // Only send the errors to the client.
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors;
                }
            }

            return errors;
        }
    }


    public class HttpContextSession : ISessionProvider
    {
        public VisualCaptchaSession GetSession(string key)
        {
            return (VisualCaptchaSession)HttpContext.Current.Session[key];
        }

        public void SetSession(string key, VisualCaptchaSession value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}