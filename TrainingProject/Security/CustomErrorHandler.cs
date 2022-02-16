using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Security
{
    /// <summary>
    /// Handles the unhandled exceptions in the application
    /// </summary>    
    public class CustomErrorHandler : FilterAttribute, IExceptionFilter, IDisposable
    {
        #region Properties

        TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork uow;

        #endregion

        #region Methods
        #region OnException
        /// <summary>
        /// Called when any exception occured in application and that is not handled by user
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            //uow = new TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation.UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());     
            //if (!filterContext.ExceptionHandled)
            //{
            //    CustomPrincipal currentUser = HttpContext.Current.User as CustomPrincipal;
            //    filterContext.Result = new RedirectResult("/Error/ErrorInProcessing");
            //    Controller cd = (System.Web.Mvc.Controller)filterContext.Controller;
            //    string ControllerName = cd.Request.RequestContext.RouteData.Values["Controller"].ToString();
            //    string ActionName = cd.Request.RequestContext.RouteData.Values["Action"].ToString();
            //    string LoggedInUser = currentUser != null ? currentUser.FullName : "Not Authorised User";

            //    #region Create Exception Log
            //    ExceptionLog exceptionLog = new ExceptionLog();
            //    exceptionLog.SessionId = ((HttpSessionStateWrapper)((Controller)filterContext.Controller).Session).SessionID;
            //    exceptionLog.ActionName = ActionName;
            //    exceptionLog.ControllerName = ControllerName;
            //    exceptionLog.LoggedInUser = LoggedInUser;
            //    exceptionLog.OccuredOn = DateTime.Now.Date;

            //    if (filterContext.Exception.GetType() == typeof(DbEntityValidationException))
            //    {
            //        DbEntityValidationException dbe = (DbEntityValidationException)filterContext.Exception;
            //        List<string> Errors = new List<string>();
            //        foreach (var eve in dbe.EntityValidationErrors)
            //        {
            //            string err = "Entity of type :"+ eve.Entry.Entity.GetType().Name + " in state :"+ eve.Entry.State +" has the following validation errors:";
            //            string PropErrMsg = string.Empty;
            //            foreach (var ve in eve.ValidationErrors)
            //            {
            //                PropErrMsg = PropErrMsg + "Property : " + ve.PropertyName + " Error: " + ve.ErrorMessage;
            //            }
            //            Errors.Add(err + PropErrMsg);
            //        }
            //        exceptionLog.ExceptionDetails = "Message :- EntityValidationErrors " + string.Join(",", Errors);
            //    }
            //    else
            //    {
            //        exceptionLog.ExceptionDetails = "Message :-" + filterContext.Exception.Message + "" + Environment.NewLine + " Inner Exception :-" + filterContext.Exception.InnerException + "DateTime :" + DateTime.Now;

            //    }
            //    #endregion
            //    uow.ExceptionLogsRepository.Add(exceptionLog);
            //    uow.SaveChanges();
            //    filterContext.ExceptionHandled = true;




            try
            {
                if (!filterContext.ExceptionHandled)
                {

                    filterContext.Result = new RedirectResult("/Error/ErrorInProcessing");
                    writelog(filterContext.Exception);
                    filterContext.ExceptionHandled = true;
                }
            }catch(Exception ex)
            {

            }  
            }


        #region Writelog
        /// <summary>
        /// Write Log IN DATA BASE
        /// </summary>
        /// <param name="filterContext"></param>
        public static void writelog(Exception ex)
        {
            using (UnitofWork uow1 = new UnitofWork(new TrainingProjectEntities()))
            {


                try
                {

                    string ActionName = "NA";
                    string ControllerName = "NA";
                    string LoggedInUser = "";
                    string SessionId = "NA";

                    //GET CURRENT CONTEXT
                    HttpContext context = System.Web.HttpContext.Current;
                    
                    CustomPrincipal currentUser = context.User as CustomPrincipal;
                    LoggedInUser = currentUser?.FullName?.ToString() ?? "Not Authorised User";
                    //IF CURRENT CONTEXT IS NOT FOUND
                    if (context != null)
                    {
                        ActionName = context.Request.RequestContext.RouteData.Values["action"].ToString();
                        ControllerName = context.Request.RequestContext.RouteData.Values["controller"].ToString();
                        SessionId = context.Session.SessionID;
                    }

                    string ExceptionDetails = ex.Message;
                    try
                    {



                        if (ex.GetType() == typeof(DbEntityValidationException))
                        {
                            DbEntityValidationException dbe = (DbEntityValidationException)ex;
                            List<string> Errors = new List<string>();
                            foreach (var eve in dbe.EntityValidationErrors)
                            {
                                string err = "Entity of type :" + eve.Entry.Entity.GetType().Name + " in state :" + eve.Entry.State + " has the following validation errors:";
                                string PropErrMsg = string.Empty;
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    PropErrMsg = PropErrMsg + "Property : " + ve.PropertyName + " Error: " + ve.ErrorMessage;
                                }
                                Errors.Add(err + PropErrMsg);
                            }
                            ExceptionDetails = "Message :- EntityValidationErrors " + string.Join(",", Errors);
                        }

                    }
                    catch (Exception)
                    {


                    }
                    int lineNumber = 0;
                    System.Reflection.MethodBase objMethodBase = ex.TargetSite;
                    uow1.ExceptionLogsRepository.Add(new ExceptionLog
                    {
                        SessionId = SessionId,
                        ActionName = ActionName,
                        ControllerName = ControllerName,
                        InnerException = ex.InnerException?.Message ?? "NA",
                        Message = ExceptionDetails,
                        MethodName = objMethodBase?.Name ?? "NA",
                        Source = ex.TargetSite?.ReflectedType?.FullName ?? "NA",
                        LoggedInUser = LoggedInUser,
                        OccuredOn = DateTime.Now.Date,
                         LineNumber= lineNumber
                    });

                    uow1.SaveChanges();
                    uow1.Dispose();
                }
                catch (Exception ex1)
                {


                }
            }
        }
        #endregion

        #endregion

        #region Dispose
        /// <summary>
        /// Disposes the currrent unit of work object
        /// </summary>
        public void Dispose()
        {
            if (uow != null)
            {
                uow.Dispose();
            }
        }

        #endregion
        #endregion
    }
}