using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class SessionPersister
    {
        /// <summary>
        /// CurrentUser
        /// </summary>
        public static CustomPrincipal CurrentUser
        {
            get
            {
                return HttpContext.Current.User as CustomPrincipal;
            }
            set
            {
                HttpContext.Current.User = value;
            }

        }
    }
}