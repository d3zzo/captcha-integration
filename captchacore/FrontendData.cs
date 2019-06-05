using System.Collections.Generic;

namespace captcha_integration.Core
{
	public class FrontendData
	{
		public List<string> values { get; set; }
		public string imageName { get; set; }
		public string imageFieldName { get; set; }
		public string audioFieldName { get; set; }

		public FrontendData()
		{
			values = new List<string>();
		}
	}
}
