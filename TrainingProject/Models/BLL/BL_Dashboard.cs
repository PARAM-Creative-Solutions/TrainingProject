using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProject.ViewModels.Dashboard;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProject.Security;
using System.IO;

namespace TrainingProject.Models.BLL
{
    public class BL_Dashboard : IDisposable
    {

        #region Properties

        /// <summary>
        /// Unit of work
        /// </summary>
        UnitofWork uow { get; set; }

        /// <summary>
        /// prop for result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// property for message
        /// </summary>
        public string Message { get; set; }

        CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }
        /// <summary>
        /// CurrentUser
        /// </summary>
        protected virtual User CurrentUserDetails
        {
            get { return uow.UserRepository.Get(c => c.UserId == CurrentUser.Id); }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="puow">Unit of work</param>
        public BL_Dashboard(UnitofWork puow)
        {
            uow = puow;

        }
        #endregion


        #region User Dashboard

        #endregion

        #region Dispose
        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            uow?.Dispose();
        }
        #endregion
    }
}