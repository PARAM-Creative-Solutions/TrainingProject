using TrainingProject.Security;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// ErrorController
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Unit Of Work Object
        /// </summary>
        public UnitofWork uow { get; set; }

        /// <summary>
        /// CurrentUser
        /// </summary>
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorController()
        {
            uow = new TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        #endregion

        #region Methods

        #region Index

        /// <summary>
        ///  Renders to the view with error message
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region AccessDenied

        /// <summary>
        /// View for non authorised user
        /// </summary>
        /// <returns>viwew</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region ErrorInProcessing

        /// <summary>
        /// Renders the view if any error is occured while processing the request of user
        /// </summary>
        /// <returns>view</returns>
        public ActionResult ErrorInProcessing()
        {
            return View();
        }

        #endregion

        #region Exceeded

        /// <summary>
        /// Renders the view when the no of online  users gets exceeded from the given limit
        /// </summary>
        /// <returns>view</returns>
        [HttpGet]
        public ActionResult Exceeded()
        {
            return View();
        }

        #endregion

        #region InActive

        /// <summary>
        /// Renders the view when the Administrator made the user inactive
        /// </summary>
        /// <returns>view</returns>
        [HttpGet]
        public ActionResult InActive()
        {
            return View();
        }
        #endregion

        #region LoggedOff
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoggedOff()
        {
            Security.CustomPrincipal user = HttpContext.User as Security.CustomPrincipal;
            TrainingProjectDataLayer.DataLayer.Entities.DAL.UserLog logged = uow.UserLogsRepository.Get(x => x.UserId == user.Id);
            ViewBag.LoggedUserSystem = logged == null ? string.Empty : logged.IPAddress;
            return View();
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Disposes the object of Unit of work
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #endregion
    }
}