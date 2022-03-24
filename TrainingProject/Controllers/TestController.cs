using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingProject.ViewModels.Test;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;

namespace TrainingProject.Controllers
{
    public class TestController : BaseController
    {

        public TestController()
        {
            uow = new TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork(new TrainingProjectEntities());
        }


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
            User user=uow.UserRepository.GetAll().FirstOrDefault();

            TestVM.FirstName = user.FirstName;
            TestVM.LastName = user.LastName;
            TestVM.Id = user.UserId;
            return View(TestVM);
        }

        // GET: Test
        [HttpPost]
        public ActionResult Test(TestViewModel TestVM)
        {
            if (ModelState.IsValid)
            {
                User user = uow.UserRepository.Get(TestVM.Id);
                user.FirstName = TestVM.FirstName;
                user.LastName = TestVM.LastName;

                uow.SaveChanges();
                return Json(new { Result = true, Message = "Users Updated" });
            }
            else
            {
                ModelState.AddModelError("Error", "Invalid model state");
                return Json(new { Result = false, Message = "Error" });
            }
            
        }
    }
}