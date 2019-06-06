using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using captcha_integration.Models;
using VisualCaptcha;

namespace captcha_integration.Controllers
{
    public class HomeController : Controller
    {

        private const string SessionKey = "visualcaptcha";
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Start(int numberOfImages)
        {
            var captcha = new Captcha(numberOfImages);
            Session[SessionKey] = captcha;

            var frontEndData = captcha.GetFrontEndData();

            // Client side library requires lowercase property names
            return Json(new
            {
                values = frontEndData.Values,
                imageName = frontEndData.ImageName,
                imageFieldName = frontEndData.ImageFieldName,
                audioFieldName = frontEndData.AudioFieldName
            }, JsonRequestBehavior.AllowGet);
        }

        public FileResult Image(int imageIndex, int retina = 0)
        {
            var captcha = (Captcha)Session[SessionKey];
            var content = captcha.GetImage(imageIndex, retina == 1);

            return File(content, "image/png");
        }

        public FileResult Audio(string type = "mp3")
        {
            var captcha = (Captcha)Session[SessionKey];
            var content = captcha.GetAudio(type);

            var contentType = type == "mp3" ? "audio/mpeg" : "audio/ogg";
            return File(content, contentType);
        }

        [HttpPost]
        public JsonResult Try(string captcha)
        {
            var captcha2 = (Captcha)Session[SessionKey];
            return Json(new { Valid = false });
        }

        //[HttpPost]
        //public JsonResult Try(UserViewModel viewModel)
        //{
            
        //    if (!ModelState.IsValid)
        //    {
        //        //Response.StatusCode = (int)HttpStatusCode.BadGateway;
        //        return Json(new { Valid = ModelState.IsValid });
                
        //        //return Json(new
        //        //{
        //        //    Valid = ModelState.IsValid,
        //        //    Errors = GetErrorsFromModelState(),
        //        //    //StudentsPartial = studentPartialViewHtml
        //        //});
        //    }
        //    else //if (viewModel.formValidated == true)
        //    {
        //        var result = _visualCaptcha.ValidateAnswer(Request.Form);
        //        var queryParams = new NameValueCollection();
        //        if (result == CaptchaState.ValidImage || result == CaptchaState.ValidAudio)
        //        {
        //            return Json(new
        //            {
        //                redirectUrl = Url.Action("Confirmed", "Form"),
        //                isRedirect = true
        //            });
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Captcha", "Provided captcha is invalid.");
        //            return Json(new { Valid = false, Errors = true, ErrorMsg = "Invalid captcha" });
        //        }
        //    }
        //}
    }
    
}