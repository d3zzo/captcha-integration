using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using captcha_integration.Models;

namespace captcha_integration.Controllers
{
    public class FormController : Controller
    {
        // GET: Form

        public ActionResult Index()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Index(UserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.formValidated = false;
                return View(viewModel);

            }
            var form = Request.Form;
            //new HomeController().Try();
            //if (viewModel.formValidated == false)
            //{

            viewModel.formValidated = true;
            return View(viewModel);
            //}
            //    Server.Transfer("/Home/Try",
            //    return Redirect("asd/fdas");
            //}
        }
            public ActionResult Confirmed()
        {
            return View();
        }

    }
}