using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingProject.ViewModels.Dropdown;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;

namespace TrainingProject.Controllers
{
    public class DropdownController : BaseController
    {
        public DropdownController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }


        // GET: Dropdown
        public ActionResult Index()
        {
            DropdownViewModel model = new DropdownViewModel();
            ViewBag.UsersConstructor1 = new SelectList(uow.UserRepository.GetAll().Select(s=>s.UserName));

            ViewBag.UsersConstructor2 = new SelectList(uow.UserRepository.GetAll().Select(w=>w.UserName), "PPDAdmin");

            List<string> disabledValue = new List<string> { "PPDAdmin", "sysadmin" };
            ViewBag.UsersConstructor3 = new SelectList(uow.UserRepository.GetAll().Select(w=>w.UserName), "EPDAdmin", disabledValue);


            //ViewBag.UsersConstructor1 = new SelectList(uow.UserRepository.GetAll());
            //ViewBag.UsersConstructor1 = new SelectList(uow.UserRepository.GetAll());
            //ViewBag.UsersConstructor1 = new SelectList(uow.UserRepository.GetAll());
            //ViewBag.UsersConstructor1 = new SelectList(uow.UserRepository.GetAll());
            return View(model);
        }
    }
}