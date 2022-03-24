using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;

namespace TrainingProject.Controllers
{
    public class CollectionListController : BaseController
    {
        public CollectionListController()
        {
            uow = new TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        // GET: CollectionList
        public ActionResult Index()
        {

            List<User> Users = new List<User>();
            Users = uow.UserRepository.GetAll().ToList();

            return View(Users);
        }
    }
}