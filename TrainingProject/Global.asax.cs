using Newtonsoft.Json;
using TrainingProject.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using Microsoft.AspNet.SignalR;

namespace TrainingProject
{
    /// <summary>
    /// https://programmer.help/blogs/global.asax-file-in-asp.net-mvc.html
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {

        /// <summary>
        ///Not every request is called
        ///Execute once in the life cycle of a Web application
        ///Invoked in the first application startup and application domain creation
        ///Initialization code suitable for processing application scope
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // JobScheduler.Start();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Application_BeginRequest()
        {
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
        }

        /// <summary>
        /// Occurs before executing validation, which is the starting point for creating validation logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    //Extract the forms authentication cookie
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    CustomPrincipal serializeModel = JsonConvert.DeserializeObject<CustomPrincipal>(authTicket.UserData);
                    if (serializeModel != null)
                    {
                        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name, serializeModel.Id, serializeModel.PlantId, serializeModel.DepartmentId, serializeModel.FullName, serializeModel.Rights, serializeModel.SessionId);
                        Context.User = newUser;
                    }
                }
            }
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Do stuff here
            }
            //if (FormsAuthentication.CookiesSupported == true)
            //{
            //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //    if (authCookie != null)
            //    {
            //        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value); ;
            //        CustomPrincipal serializeModel = JsonConvert.DeserializeObject<CustomPrincipal>(authTicket.UserData);
            //        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name, serializeModel.Id, serializeModel.PlantId, serializeModel.DepartmentId, serializeModel.FullName, serializeModel.Rights, serializeModel.SessionId);
            //        HttpContext.Current.User = newUser;
            //    }
            //}
        }



        void Session_Start(object sender, EventArgs e)
        {
            //Not every request is called
            //Execution at the beginning of a session

            //string ip = HttpContext.Current.Request.UserHostAddress;
        }

        void Session_End(object sender, EventArgs e)
        {
            //Not every request is called
            //Execution at the end or expiration of a session
            //This method will be invoked regardless of the explicit empty Session or automatic expiration of Session timeouts in the code
        }

        void Application_Init(object sender, EventArgs e)
        {
            //Not every request is called
            //Execute at the time of initialization of each HttpApplication instance
        }

        void Application_Disposed(object sender, EventArgs e)
        {
            //Not every request is called
            //After the application has been shut down for some time, it is called when the. net garbage collector is ready to reclaim the memory it occupies.
            //Execute before each HttpApplication instance is destroyed
        }

        void Application_Error(object sender, EventArgs e)
        {
            //Not every request is called
            //All unhandled errors lead to the execution of this method
            Exception ex = Server.GetLastError();
        }


        /*********************************************************************/
        //Each request executes the following events in sequence
        /*********************************************************************/

        void Application_BeginRequest(object sender, EventArgs e)
        {
            //The first event to start at each request, the first execution of this method
            DateTime dateTimeBeginRequest = DateTime.Now;

            HttpContext ctx = HttpContext.Current;
            ctx.Items["dateTimeBeginRequest"] = dateTimeBeginRequest;
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            //EndRequest is the last triggered event in response to Request
            //But before the object is released or newly created, it's appropriate to clean up the code at this time.
            DateTime dateTimeEndRequest = DateTime.Now;

            HttpContext ctx = HttpContext.Current;
            DateTime dateTimeBeginRequest =
                 (DateTime)ctx.Items["dateTimeBeginRequest"];

            TimeSpan duration = dateTimeEndRequest - dateTimeBeginRequest;
            //Response.Write("<b> This request took " +
            //    duration.ToString() + "</b></br>");
        }

        void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            //Execute when the security module has verified the authorization of the current user
        }

        void Application_ResolveRequestCache(object sender, EventArgs e)
        {
            //This occurs when ASP.NET completes authorization events to enable the cache module to service requests from the cache, thus skipping the execution of the handler (page or Web Service).
            //Doing so can improve the performance of the website, and this event can also be used to determine whether the text is obtained from Cache.
        }

        //------------------------------------------------------------------------
        //At this point, the request will be forwarded to the appropriate procedure. For example, web forms will be compiled and instantiated
        //------------------------------------------------------------------------

        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //Read the specific information required by Session and execute it before filling it in
        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //Call before the appropriate handler executes the request
            //At this point, Session will be available
        }

        //-------------------------------------------------
        //At this point, the page code will be executed and the page will be rendered as HTML.
        //-------------------------------------------------

        void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            //When the handler completes processing the request, it is called.
        }

        void Application_ReleaseRequestState(object sender, EventArgs e)
        {
            //Release request status
        }

        void Application_UpdateRequestCache(object sender, EventArgs e)
        {
            //For subsequent requests, the update response cache is invoked
        }

       

        void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //Called before sending the Http header to the client
        }

        void Application_PreSendRequestContent(object sender, EventArgs e)
        {
            //Called before sending the Http body to the client
        }
    }
}
