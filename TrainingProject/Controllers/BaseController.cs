using TrainingProject.Security;
using TrainingProject.ViewModels.Dashboard;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// BaseController use for common propertys
    /// </summary>
    [CustomErrorHandler]
    [SessionFilter]
    [CustomAuthorize]
    public class BaseController : Controller
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
       

        #region Mehods

        #region Dispose

        /// <summary>
        /// Dispose the object
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(uow != null)
                {
                    uow.Dispose();
                    uow = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion



        #endregion
    }
}