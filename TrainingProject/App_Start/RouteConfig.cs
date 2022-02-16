using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TrainingProject
{
    #region RouteConfig

    /// <summary>
    /// Class for RouteConfiguration
    /// </summary>
    public class RouteConfig
    {
        #region Method

        /// <summary>
        /// Method for registering Route
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }

        #endregion
    }

    #endregion
}
