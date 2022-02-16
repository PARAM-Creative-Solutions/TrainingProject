using TrainingProject;
using TrainingProject.Security;
using TrainingProject.ViewModels.Report;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using Foolproof;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using MvcReportViewer;

namespace TrainingProject.Controllers
{
    public class ReportController : BaseController
    {

        #region Constructor
        /// <summary>
        /// Initilizes instance of unit of work
        /// </summary>
        public ReportController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }

        #endregion

        #region Methods

        #region Report Type

        #region INDEX
        // GET: Report
        public ActionResult Index()
        {
            return View(uow.ReportTypeRepository.GetAll());
        }
        #endregion

        #region Details
        // GET: Report/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportsType department = uow.ReportTypeRepository.Get(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }


        #endregion

        #region Create

        // GET: Report/Create
        public ActionResult Create()
        {
            IEnumerable<string> t = (IEnumerable<string>) uow.Db.Database.SqlQuery<string>("SELECT name FROM sys.views");
            
            ViewBag.ReportType = new SelectList( t);

            var ReportDesignTypes = from SystemEnums.ReportDesignType d in Enum.GetValues(typeof(SystemEnums.ReportDesignType))
                             select new { ID = (int)d, Name = d.ToString() };
            ViewBag.ReportDesignTypes = new SelectList(ReportDesignTypes, "ID", "Name");

          
            return View();
        }


        /// <summary>
        /// Post Method To create ReportsType
        /// </summary>
        /// <param name="ReportsType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReportsType reportsType)
        {
            if (ModelState.IsValid)
            {
               
                uow.ReportTypeRepository.Add(reportsType);
                uow.SaveChanges();
                return Json(new { Result = true, Message = "Report Type Created Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Create Report Type !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        // GET: Report/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ReportsType Rtype = uow.ReportTypeRepository.Get(id);
                if (Rtype == null)
                {
                    return HttpNotFound();
                }
                uow.ReportTypeRepository.Remove(Rtype);
                uow.SaveChanges();
                return Json(new { success = true, Message = "Report Type Deleted Successfully !!" });
            }
            catch (Exception ex)
            {
                CustomErrorHandler.writelog(ex);
                return Json(new { success = false, Message = "Fail to Delete Report Type !" });
            }
        }


        #endregion

        #region Edit

        #region Edit Get Method

        /// <summary>
        /// Get ReportType to Edit With Its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportsType Report = uow.ReportTypeRepository.Get(id);
            if (Report == null)
            {
                return HttpNotFound();
            }
            IEnumerable<string> t = (IEnumerable<string>)uow.Db.Database.SqlQuery<string>("SELECT name FROM sys.views");

            ViewBag.ReportType = new SelectList(t);


            var ReportDesignTypes = from SystemEnums.ReportDesignType d in Enum.GetValues(typeof(SystemEnums.ReportDesignType))
                                    select new { ID = (int)d, Name = d.ToString() };
            ViewBag.ReportDesignTypes = new SelectList(ReportDesignTypes, "ID", "Name");

            return View(Report);
        }
        #endregion

        #region Edit Post Method

        /// <summary>
        /// Post Method to Submit Edited Data
        /// </summary>
        /// <param name="ReportsType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ReportsType reortType)
        {
            
            if (ModelState.IsValid)
            {
                
                uow.ReportTypeRepository.Update(reortType);
                uow.SaveChanges();
                return Json(new { Result = true, Message = "Report Type Updated Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Update Report Type !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        #endregion

        #region Reports

        /// <summary>
        /// Display actual report
        /// </summary>
        /// <returns></returns>
        public ActionResult Report()
        {
            return View();
        }

        #region List Of Available Reports
        /// <summary>
        /// Gernertae Report GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateReport()
        {
           
            ReportsViewModel reportVM = new ReportsViewModel();
            reportVM.ReportsTypes = uow.ReportTypeRepository.GetAll();
            return View(reportVM);
        }

        /// <summary>
        /// DatasetToXML String
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string ToStringAsXml(DataSet ds)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
            string s = sw.ToString();
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportsViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateReport(ReportsViewModel reportsViewModel)
        {
            string reporttitle = uow.ReportTypeRepository.GetAll(w => w.ReportTypeID == Convert.ToInt32(reportsViewModel.SelectedReport)).FirstOrDefault().ReportHeader;
            BL_Report bl = new BL_Report(uow);
            bl.report = reportsViewModel;
            bl.reporttitle = reporttitle;
            SystemEnums.ReportDesignType reportDesignType = (SystemEnums.ReportDesignType)uow.ReportTypeRepository.GetAll(w => w.ReportTypeID ==Convert.ToInt32(reportsViewModel.SelectedReport)).FirstOrDefault().ReportDesignType;
            ViewBag.Title = reportsViewModel.SelectedReport;
            string Domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string rptPath = string.Empty;
            bool IsTestingMails = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsTestingMails"]);
            if ((Domain.ToUpper() == "LEO.LOCAL" || Environment.UserName == "Vikrant") && IsTestingMails)
            {
                string basePath = System.Web.HttpContext.Current.Server.MapPath("@/bin");
                if (basePath.Contains("Hosted Site"))
                {
                    rptPath = System.Web.HttpContext.Current.Server.MapPath(@"~/bin/Reports/");

                }
                else
                    rptPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath(""))), "TrainingProjectDataLayer\\Reports");
            }
            else
                rptPath = System.Web.HttpContext.Current.Server.MapPath(@"~/bin/Reports/");

            MvcReportViewerIframe report = null;
            
            switch (reportDesignType)
            {
                case SystemEnums.ReportDesignType.TableReport:
                case SystemEnums.ReportDesignType.TableDetailReport:
                    #region TableDetailReport 
                    //ReportViewer1.LocalReport.ReportPath
                    //    = Server.MapPath(@"~/Report/User Wise Reports\Designer\" + temp.ToString() + ".rdlc");
                    //ReportData rpt = new ReportData();
                    //typeof(ReportData).GetMethod(temp.ToString()).Invoke(rpt, null);

                    //foreach (string item in rpt.ReportDataSource.Keys)
                    //{
                    //    ReportDataSource datasource = new ReportDataSource(item, rpt.ReportDataSource[item]);
                    //    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    //}
                    #endregion
                    break;
                case SystemEnums.ReportDesignType.Other:
                    #region Other Reports    
                    SystemEnums.OtherReports otherReports = (SystemEnums.OtherReports)Convert.ToInt32(reportsViewModel.SelectedReport);                  
                    report =new MvcReportViewerIframe(System.IO.Path.Combine(rptPath, otherReports + ".rdlc"));
                    ViewBag.Title = HelperMethods.GetDescription(otherReports);                  
                    object res=  typeof(ReportsViewModel).GetMethod(otherReports.ToString()).Invoke(reportsViewModel, null);
                    ReportDataSource datasource = new ReportDataSource("DataSet1", res);
                    report.LocalDataSource("DataSet1", datasource);
                    if (reportsViewModel.DataSet2!=null)
                    {
                        ReportDataSource datasource2 = new ReportDataSource("DataSet2", reportsViewModel.DataSet2);
                        report.LocalDataSource("DataSet2", datasource2);
                    }
                   
                    var parameters = new List<ReportParameter>()
                    {
                        new ReportParameter("SelectedDateRange", reportsViewModel.StartDate.Value.ToString("d-MMM-yy") + " TO "  + reportsViewModel.EndDate.Value.ToString("d-MMM-yy"))
                    };
                    report.ReportParameters(parameters);

                    #endregion
                    break;
                default:
                    #region default
                    List<User> users = uow.UserRepository.GetAll().ToList();
                    report = new MvcReportViewerIframe(Server.MapPath("~/Reports/SimpleReportTest.rdlc"));
                    ReportDataSource datasource1 = new ReportDataSource("DataSet1", users);
                    report.LocalDataSource("DataSet1", datasource1);
                    #endregion
                    break;
            }
            report.ProcessingMode(ProcessingMode.Local);
            //var parameters = new List<ReportParameter>()
            //{
            //    new ReportParameter("Parameter1", new [] { "Value 1", "Value 2" }, true),
            //    new ReportParameter("Parameter2", new [] { "1/1/2010", "2/2/2015" }, false),
            //    new ReportParameter("Parameter3", new [] { "12345", "9876", "123" }, true),
            //    new ReportParameter("Parameter4", "6/20/2014", false)
            //};
            //report.ReportParameters(parameters);
            //report.EventsHandlerType(typeof(SubreportEventHandlers));
            return View("Report", report);
        }

        #endregion

        #region Load Report
        public RedirectResult ReportsLoad()
        {
            return Redirect(@"~\Views\Report\Report.aspx");
        }
        #endregion

        #endregion

        #endregion


        #region Dispose
        /// <summary>
        /// Dispose the object
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion






    }
}






