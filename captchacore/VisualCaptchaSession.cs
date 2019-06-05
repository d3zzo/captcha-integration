using System.Collections.Generic;

namespace captcha_integration.Core
{
	public class VisualCaptchaSession
	{
		public List<Option> Images { get; set; }
		public Option ValidImageOption { get; set; }
		public Option ValidAudioOption { get; set; }
		public FrontendData FrontendData { get; set; }
	}
}