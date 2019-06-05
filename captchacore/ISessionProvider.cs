namespace captcha_integration.Core
{
	public interface ISessionProvider
	{
		void SetSession(string key, VisualCaptchaSession value);
		VisualCaptchaSession GetSession(string key);
	}
}
