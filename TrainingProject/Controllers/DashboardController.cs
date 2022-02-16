using TrainingProject.Security;
using TrainingProject.ViewModels.Dashboard;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingProjectDataLayer.Constants;
using System.Collections;
using TrainingProject.Models.BLL;
using System.IO;

namespace TrainingProject.Controllers
{
    /// <summary>
    ///  Dashboard Controller
    /// </summary>
    public class DashboardController : BaseController
    {
        #region Constructor

        /// <summary>
        /// Initilizes instance of unit of work
        /// </summary>
        public DashboardController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }

        #endregion

        /// <summary>
        /// Represent all user dashboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
          
            DashboardViewModel dashboard = new DashboardViewModel();
            BL_Dashboard bl = new BL_Dashboard(uow);
           // List<InspectionCallRequest> All = uow.InspectionCallRequestRepository.GetAll(x => x.PlantId == CurrentUser.PlantId && x.IsSubmit).ToList();
            dashboard.CompletedCallCount= string.Concat("00", "0");
            dashboard.ConfirmedCallCount = string.Concat("00", "0");
            
            dashboard.NewCallCount = string.Concat("00", "0");
            dashboard.WaivedCallCount = string.Concat("00", "0");
            dashboard.RejectedCallCount = string.Concat("00", "0");
            return View(dashboard);
        }

    }
}
