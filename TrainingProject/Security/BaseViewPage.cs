using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Security
{
    
    #region Classes

    /// <summary>
    /// Abstract class for BaseViwePage,inheriting WebViwePage
    /// </summary>
    public abstract class BaseViewPage : WebViewPage
    {
       
        /// <summary>
        /// gets the user as customprincipal
        /// </summary>
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }

    /// <summary>
    /// Abstract class for BaseViewPage having virtual method of type CustomPrincipal
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Gets the user as CustomPrincipal
        /// </summary>
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
       
    }


    #endregion
}