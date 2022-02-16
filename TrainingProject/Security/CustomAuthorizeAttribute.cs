using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TrainingProject.Security
{
    /// <summary>
    /// Class for custom autharize inheriting Autharize Attribute
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        #region Properties

        /// <summary>
        /// Property for UserCongigKey field
        /// </summary>
        public string UsersConfigKey { get; set; }

        /// <summary>
        /// Property for RolesConfigKey field
        /// </summary>
        public string RolesConfigKey { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Virtual class for Custom Principal
        /// </summary>
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        /// <summary>
        /// Overrides the OnAutharization()
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                #region Old Code
                //var authorizedUsers = String.Empty; //ConfigurationManager.AppSettings[UsersConfigKey];
                //var authorizedRoles = String.Empty;//ConfigurationManager.AppSettings[RolesConfigKey];

                //Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                //Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

                //if (!String.IsNullOrEmpty(Roles))
                //{
                //    if (!CurrentUser.IsInRole(Roles))
                //    {
                //        filterContext.Result = new RedirectToRouteResult(new
                //        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                //        // base.OnAuthorization(filterContext); //returns to login url
                //    }
                //}

                //if (!String.IsNullOrEmpty(Users))
                //{
                //    if (!Users.Contains(CurrentUser.Id.ToString()))
                //    {
                //        filterContext.Result = new RedirectToRouteResult(new
                //     RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));

                //        // base.OnAuthorization(filterContext); //returns to login url
                //    }
                //}
                #endregion

                #region New Code
                //if (CurrentUser == null)
                //{
                //    filterContext.Result = new RedirectToRouteResult(new
                //    RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                //    // base.OnAuthorization(filterContext); //returns to login url
                //}
                //else
                //{
                   
                //}

                #endregion

            }

        }

        #endregion
    }
}