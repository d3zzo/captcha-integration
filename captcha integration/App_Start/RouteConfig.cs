using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace captcha_integration
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.Add(
                 "VisualCaptchaImages",
                 new Route("Home/Image/{index}",
                     new RouteValueDictionary(
                         new
                         {
                             controller = "Home",
                             action = "Image"
                         }),
                     new ReadOnlySessionRouteHandler()
                     )
                 );
            routes.MapRoute(
                "captcha-start",
                "{controller}/Start/{numberOfImages}",
                new { Controller = "Home", Action = "Start", numberOfImages = UrlParameter.Optional });

            routes.MapRoute(
                "captcha-audio",
                "{controller}/Audio",
                new { Controller = "Home", Action = "Audio" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Form", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
