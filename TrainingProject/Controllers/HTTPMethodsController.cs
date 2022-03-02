using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Controllers
{
    public class HTTPMethodsController : Controller
    {
        // GET: HTTPMethods
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Test(string FirstName)
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexPost()
        {
            return View();

        }

    }
}