using TrainingProject.Security;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Models.BLL
{
    public abstract class BL_Base : IDisposable
    {
        #region Properties

        /// <summary>
        /// Unit of work
        /// </summary>
       public UnitofWork uow { get; set; }

        /// <summary>
        /// prop for result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// property for message
        /// </summary>
        public string Message { get; set; }

        public CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }


        #endregion

        #region Dispose

        /// <summary>
        /// Disposes the object
        /// </summary>
        public virtual void Dispose()
        {
            uow?.Dispose();
        }
        #endregion
    }
}