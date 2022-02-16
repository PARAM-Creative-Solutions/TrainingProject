using TrainingProject.Security;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProjectDataLayer.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TrainingProject.ViewModels.Report
{

    public class BaseReport
    {
        

        [Display(Name = "Reports")]
        public string SelectedReport { get; set; }


        CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }


        //public SalesOrder checkISLineItemOrSalesOrder1(LineItem lineItem)
        //{
        //    return (lineItem == null ? lineItem.SalesOrder : lineItem.SalesOrder);
        //}

        /// <summary>
        /// THIS METHOD WORK AS PREDICATE WHEN DATA IS FETCHED FROM DATABASE
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public  bool ApplyFilter(object Data)
        {            
            if (Data == null)
                return true;

          

            return true;
        }
    }

    /// <summary>
    /// Report View Model
    /// </summary>
    public class ReportsViewModel: Reports
    {
        /// <summary>
        /// All report type in system
        /// </summary>
        public IEnumerable<ReportsType> ReportsTypes { get; set; }

        [Display(Name = "Reports")]
        public string SelectedReport { get; set; }

       public List<Reports> DataSet2 = null;


        CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }


      //public  List<Reports> SupplierWiseCallreport()
      //  {
      //      List<Reports> SupplierWiseCallreportList = new List<Reports>();
      //      using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
      //      {

      //          List<InspectionCallRequest> InspectionCallRequests = uow.InspectionCallRequestRepository.GetAll(ApplyFilter).Where(GetSubmited).ToList();
      //          //uow.InspectionCallRequestRepository.GetAll().ToList();
      //          //

      //          SupplierWiseCallreportList = (from insp in InspectionCallRequests
      //                                        group insp by new { insp.User.VendorUsersLists.FirstOrDefault()?.Vendor.VendorName, insp.Plant.PlantCode }
      //                                       into gcs

      //                                       select new Reports()
      //                                       {
      //                                           SupplierName = gcs.Key.VendorName,
      //                                           PlantCode = gcs.Key.PlantCode,
      //                                           EndDate=EndDate, StartDate=StartDate.Value,

      //                                           NumberOfCallRaised = gcs.Count(),
      //                                          // NumberOfCallattended=gcs.Where(x=>x.InspectionCallStatus==(int)SystemEnums.InspectionCallStatus.Completed).Count(),
      //                                          // NumberOfCallWaived = gcs.Where(x=>x.InspectionCallStatus==(int)SystemEnums.InspectionCallStatus.Waived).Count(),
                                                  
      //                                       }).ToList();



      //      }

      //      return SupplierWiseCallreportList;

      //  }

        //public List<Reports> RejectedCallReport()
        //{
        //    List<Reports> SupplierWiseCallreportList = new List<Reports>();
        //    using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
        //    {

        //        List<InspectionCallRequest> InspectionCallRequests = uow.InspectionCallRequestRepository.GetAll(ApplyFilter).ToList();

        //        SupplierWiseCallreportList = (from insp in InspectionCallRequests
        //                                      group insp by new { insp.User.VendorUsersLists.FirstOrDefault()?.Vendor.VendorName, insp.Plant.PlantCode }
        //                                     into gcs

        //                                      select new Reports()
        //                                      {
        //                                          SupplierName = gcs.Key.VendorName,
        //                                          PlantCode = gcs.Key.PlantCode,
        //                                          EndDate = EndDate,
        //                                          StartDate = StartDate.Value,
        //                                          NumberOfCallWaived = gcs.Where(x => x.InspectionCallStatus == (int)SystemEnums.InspectionCallStatus.Rejected).Count(),

        //                                      }).ToList();



        //   List<InspectionCallStatusRemark> InspectionCallStatusRemark = uow.InspectionCallStatusRemarkRepository.GetAll(ApplyFilter).ToList().Where(x=>x.InspectionCallRequest.InspectionCallStatus ==(int)SystemEnums.InspectionCallStatus.Rejected).ToList();
        //        DataSet2 = new List<Reports>();
        //        InspectionCallStatusRemark.ForEach(x=>
        //            {
        //                DataSet2.Add(new Reports() {
        //                    SupplierName = x.InspectionCallRequest.User.VendorUsersLists?.FirstOrDefault().Vendor.VendorName,
        //                    PlantCode = x.InspectionCallRequest.Plant.PlantCode,
        //                    Remark = x.Remark

        //                });
        //            });


        //    }


           

        //    return SupplierWiseCallreportList;

        //}
        //public List<Reports> WaivedCallReport()
        //{
        //    List<Reports> SupplierWiseCallreportList = new List<Reports>();
        //    using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
        //    {

        //        List<InspectionCallRequest> InspectionCallRequests = uow.InspectionCallRequestRepository.GetAll(ApplyFilter).Where(GetSubmited).ToList();

        //        SupplierWiseCallreportList = (from insp in InspectionCallRequests
        //                                      group insp by new { insp.User.VendorUsersLists.FirstOrDefault()?.Vendor.VendorName, insp.Plant.PlantCode }
        //                                     into gcs

        //                                      select new Reports()
        //                                      {
        //                                          SupplierName = gcs.Key.VendorName,
        //                                          PlantCode = gcs.Key.PlantCode,
        //                                          EndDate = EndDate,
        //                                          StartDate = StartDate.Value,
        //                                          NumberOfCallWaived = gcs.Where(x => x.InspectionCallStatus == (int)SystemEnums.InspectionCallStatus.Waived).Count(),

        //                                      }).ToList();



        //        List<InspectionCallStatusRemark> InspectionCallStatusRemark = uow.InspectionCallStatusRemarkRepository.GetAll(ApplyFilter)
        //            .ToList().Where(x => x.InspectionCallRequest.InspectionCallStatus== (int)SystemEnums.InspectionCallStatus.Waived).ToList();
        //        DataSet2 = new List<Reports>();
        //        InspectionCallStatusRemark.ForEach(x =>
        //        {
        //            DataSet2.Add(new Reports()
        //            {
        //                SupplierName = x.InspectionCallRequest.User.VendorUsersLists.FirstOrDefault()?.Vendor.VendorName,
        //                PlantCode = x.InspectionCallRequest.Plant.PlantCode,
        //                Remark = x.Remark

        //            });
        //        });


        //    }




        //    return SupplierWiseCallreportList;

        //}
        //public List<Reports> PendingCallReport()
        //{
        //    List<Reports> SupplierWiseCallreportList = new List<Reports>();
        //    using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
        //    {

        //        List<InspectionCallRequest> InspectionCallRequests = uow.InspectionCallRequestRepository.GetAll(ApplyFilter).Where(GetSubmited).ToList();

        //        SupplierWiseCallreportList = (from insp in InspectionCallRequests
        //                                      group insp by new { insp.User.VendorUsersLists.FirstOrDefault()?.Vendor.VendorName, insp.Plant.PlantCode }
        //                                     into gcs

        //                                      select new Reports()
        //                                      {
        //                                          SupplierName = gcs.Key.VendorName,
        //                                          PlantCode = gcs.Key.PlantCode,
        //                                          EndDate = EndDate,
        //                                          StartDate = StartDate.Value,
        //                                          NumberOfCallWaived = gcs.Where(x => x.InspectionCallStatus == (int)SystemEnums.InspectionCallStatus.Confirmed).Count(),

        //                                      }).ToList();



              
            


        //    }




        //    return SupplierWiseCallreportList;

        //}
        //public List<Reports> AgeAnalysis()
        //{
        //    List<Reports> SupplierWiseCallreportList = new List<Reports>();
        //    using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
        //    {

        //        List<InspectionCallRequest> InspectionCallRequests = uow.InspectionCallRequestRepository.GetAll(ApplyFilter).Where(GetSubmited).ToList().Where(x=>x.InspectionCallStatus==(int)SystemEnums.InspectionCallStatus.Completed).ToList();
        //        InspectionCallRequests.ForEach(x=>
        //            {
        //                SupplierWiseCallreportList.Add(new Reports()
        //                {
        //                    SupplierName = x.User.VendorUsersLists.FirstOrDefault()?.Vendor.VendorName,
        //                    PlantCode = x.Plant.PlantCode,
        //                    InspectionCallRaisedOn=x.CreatedOn,
        //                    InspectionCallDate =x.DateOfRaise.Value,
        //                    InspectionCallAttendedOn=x.DateOfInspection.Value

        //                });

        //            });

        //    }
        //    return SupplierWiseCallreportList;
        //}
        //public List<Reports> InspectionStageWiseData()
        //{
        //    List<Reports> InspectionStageWiseDataList = new List<Reports>();
        //    using (UnitofWork uow = new UnitofWork(new TrainingProjectEntities()))
        //    {
        //        uow.InspectionStageRepository.GetAll().ToList().ForEach(x =>
        //        {
        //            InspectionStageWiseDataList.Add(new Reports()
        //            {   //InspectionStageName
        //                SupplierName = x.InspectionStageName,

        //                NumberOfCallRaised = x.InspectionCallWiseStages.Where(st => st.InspectionStageId == x.InspectionStageId).Select(t=>t.InspectionCallRequest).Where(ApplyFilter).
        //                Where(i =>  (i.IsSubmit==true || i.InspectionCallStatus==(int)SystemEnums.InspectionCallStatus.Rejected)).Count()
        //        });
        //        }

        //        );
        //    }

        //        return InspectionStageWiseDataList;
        //}
        //private bool ApplyFilter(object Data)

        //{
        //    if (Data == null)
        //        return true;
        //    #region CHECK DATE FROM USER INTERFACE FILETER AND DATA FROM QUERY

        //    //IF NO START DATE // END DATE FILTER
        //    if (StartDate != null && EndDate != null)
        //    {
        //        DateTime startdate = StartDate.Value;
        //        DateTime enddate = EndDate.Value;
        //        if (Data is InspectionCallRequest)
        //        {
        //            //GET CURRENT USER DATA
        //            if (((InspectionCallRequest)Data).PlantId!=CurrentUser.PlantId)
        //            {
        //                return false;
        //            }

        //            DateTime DateOfRaise = ((InspectionCallRequest)Data).DateOfRaise.Value.Date;
        //            return DateOfRaise >= startdate.Date && DateOfRaise <= enddate.Date ? true : false;
        //        }
        //        if (Data is InspectionCallStatusRemark)
        //        {
        //            DateTime DateOfRaise = ((InspectionCallStatusRemark)Data).CreatedOn.Date;
        //            return DateOfRaise >= startdate.Date && DateOfRaise <= enddate.Date ? true : false;
        //        }

        //    }
        //    #endregion
        //    return true;

        //}

        //private bool GetSubmited(InspectionCallRequest Data)

        //{
        //    return Data.IsSubmit;
        //}

        }
}
