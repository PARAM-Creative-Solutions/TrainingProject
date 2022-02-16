using System.Web;
using System.Web.Mvc;

namespace TrainingProject
{
    #region FilterConfig

    /// <summary>
    /// FilterConfig
    /// </summary>
    public class FilterConfig
    {
        #region Method

        /// <summary>
        /// Method for registering global filter
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Security.CustomErrorHandler());
        }

        #endregion
    }

    #endregion
}
