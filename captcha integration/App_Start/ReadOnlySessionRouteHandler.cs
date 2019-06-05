using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace captcha_integration
{
    public class ReadOnlySessionRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.HttpContext.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
            return new MvcHandler(requestContext);
        }
    }
}
