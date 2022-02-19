using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingProject.ViewModels.Test;

namespace TrainingProject.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }


        // GET: Test
        [HttpGet]
        public ActionResult Test()
        {
            TestViewModel TestVM = new TestViewModel();
            return View(TestVM);
        }

        // GET: Test
        [HttpPost]
        public ActionResult Test(TestViewModel TestVM)
        {
            return View();
        }
    }
}