
namespace captcha_integration.Core
{
	public enum CaptchaState
	{
		NoCaptcha,

		ValidImage,
		FailedImage,
		ValidAudio,
		FailedAudio,

		GeneralFail,
	}
}
