using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TrainingProject.Security
{
    /// <summary>
    /// Validates the session of current logged in User
    /// </summary>
    public class SessionFilter : ActionFilterAttribute
    {
        #region Methods

        /// <summary>
        /// Gets fired when any action from controller is executing
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork uow = new TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities()))
            {
                {
                    var request = filterContext.HttpContext.Request;
                    var response = filterContext.HttpContext.Response;
                    if (SessionPersister.CurrentUser == null)
                    {
                        if (request.IsAjaxRequest())
                        {
                            //var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                            response.StatusCode = 590;
                            //response.Redirect(url.Action("Login", "Account"));
                        }
                        else
                        {
                            var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                            HttpContext.Current.Session.Add("SeessionExpire", true);
                            response.Redirect(url.Action("Login", "Account"));
                        }
                        filterContext.Result = new EmptyResult();
                    }
                    else
                    {
                        #region Profile picture
                        User userFromDB = uow.UserRepository.Get(SessionPersister.CurrentUser.Id);
                        if (userFromDB != null)
                        {
                            UploadedFile file = uow.UploadedFileRepository.Get(x => x.Id == userFromDB.Photo);

                            if (file != null)
                            {
                                string FilePath = Path.Combine(file.FilePath, userFromDB.Photo.ToString(), file.FileName);
                                if (File.Exists(FilePath))
                                {
                                    byte[] byteData = System.IO.File.ReadAllBytes(Path.Combine(file.FilePath, userFromDB.Photo.ToString(), file.FileName));
                                    filterContext.Controller.ViewBag.UserPhoto = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(byteData));
                                }
                                else
                                    filterContext.Controller.ViewBag.UserPhoto = "~/Images/img.jpg";
                            }
                            else
                                filterContext.Controller.ViewBag.UserPhoto = "~/Images/img.jpg";
                        }
                        #endregion
                        //**********added on 01-01-2018 by vikrant to get and update the system information of the user system
                        //string clientMachineName = Common.Constants.GetHostDetails(filterContext.HttpContext);
                        string CLientIP_Address = HelperMethods.GetIPAddress(filterContext.HttpContext);
                        //**************
                        UserLog userlogdetail = uow.UserLogsRepository.Get(x => x.SessionId == SessionPersister.CurrentUser.SessionId);
                        if (userlogdetail != null)
                        {
                            if (userlogdetail.OnlineStatus && userlogdetail.IPAddress != CLientIP_Address)
                            {
                                var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                                response.Redirect(url.Action("LoggedOff", "Error"));
                                filterContext.Result = new EmptyResult();
                            }
                        }
                    }
                    base.OnActionExecuting(filterContext);
                }
            }
        }

        #endregion
    }
}