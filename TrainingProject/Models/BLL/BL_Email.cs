using TrainingProject.Models.MailManager;
using TrainingProject.Security;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingProjectDataLayer.Constants;

namespace TrainingProject.Models.BLL
{
    /// <summary>
    /// BL_Email
    /// </summary>
    public class BL_Email : IDisposable
    {
        #region Properties
        /// <summary>
        /// Result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        UnitofWork uow;
      
        /// <summary>
        /// Property for Current User
        /// </summary>
        public CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }


        #endregion

        #region Constructor
        /// <summary>
        /// BL_Email Constructor
        /// </summary>
        public BL_Email()
        {  
            uow = new UnitofWork(new TrainingProjectEntities());
        }

        public BL_Email(UnitofWork uow)
        {
            this.uow = uow;
        }

        #endregion

        #region Methods
        #region Mail method when TrainingProject is created

        /// <summary>
        /// If the Client ask vendor for revision of document then this mail will get send to vendor by KSB
        /// </summary>
        /// <returns></returns>
        public async void InspectionCallMail(int inspectionCallId , ControllerContext ControllerContext, ViewDataDictionary viewData)
        {
            try
            {
                //string UserEmail = uow.UserRepository.Get(x => x.UserId == UserId).EmailId;
                //string CurrentUserMail = uow.UserRepository.Get(x => x.UserId == CurrentUser.Id).EmailId;
                //InspectionCallRequest objInspectionCallRequest = uow.InspectionCallRequestRepository.Get(inspectionCallId);
                //new stage
               // objInspectionCallRequest.SelectedStages = new List<SelectListItem>();
                //objInspectionCallRequest.InspectionCallWiseStages.ToList().ForEach(x =>
                //{
                //  //  objInspectionCallRequest.SelectedStages.Add(new SelectListItem()
                //    {
                //        Text = uow.InspectionStageRepository.Get( x.InspectionStageId).InspectionStageName,

                //        Value = x.InspectionStageId.ToString()
                //    });
                //});
                //    User Objuser = uow.UserRepository.Get(x => x.UserId == CurrentUser.Id);
                //string CurrentUserMail = Objuser.EmailId;
                //MailData maildata = new MailData();
                //maildata.objInspectionCallRequest = objInspectionCallRequest;
                //SetMailData(maildata , CurrentUserMail);
              
                //maildata.UserName = Objuser.FirstName + " " + Objuser.LastName;
                //maildata.User = Objuser;
               
                //maildata.From = "";
                //maildata.ReplyBackTo = CurrentUserMail;
                //maildata.Subject = "KSB Training Project - " + objInspectionCallRequest.NotificationNumber;
                //string HtmlString = ConvertViewToString("~/Views/Email/InspectionCallEmail.cshtml", maildata, viewData, ControllerContext);
                //maildata.Body = HtmlString.Replace( "&quot;" , "\"");

               // bool mailResult = await Mail.SendMail(maildata);
                Result = true;
               
            }
            catch (Exception ex)
            {
                Result = false;
                Message = " But Email Notification Failed !";
                CustomErrorHandler.writelog(ex);
            }

        }

        #endregion

        #region MAIL WHEN Training Project IS FORWARED TO EXTERNAL AGENCY
        //public async void InspectionCallExternalAgencyMail(InspectionCallRequest objInspectionCallRequest, ControllerContext ControllerContext, ViewDataDictionary viewData)
        //{
        //    try
        //    {
        //        //string UserEmail = uow.UserRepository.Get(x => x.UserId == UserId).EmailId;
        //        //string CurrentUserMail = uow.UserRepository.Get(x => x.UserId == CurrentUser.Id).EmailId;
                 
        //        User Objuser = uow.UserRepository.Get(x => x.UserId == CurrentUser.Id);
        //        string CurrentUserMail = Objuser.EmailId;
        //        MailData maildata = new MailData();
               


        //        //objInspectionCallRequest.SelectedStages = new List<SelectListItem>();
        //        //objInspectionCallRequest.InspectionStageIds.ToList().ForEach(x =>
        //        //{
        //        //    objInspectionCallRequest.SelectedStages.Add(new SelectListItem()
        //        //    {
        //        //        Text = uow.InspectionStageRepository.Get(x).InspectionStageName,

        //        //        Value = x.ToString()
        //        //    });
        //        //});
        //        //maildata.objInspectionCallRequest = objInspectionCallRequest;
        //        //List<string> ToEmailIds = new List<string>();
        //        //List<string> CCEmailIds = new List<string>();

        //        #region TO EMAIL IDS
        //        //ToEmailIds.AddRange(maildata.objInspectionCallRequest.ExternalAgencyEmail?.Split(',').ToList());
               
        //        //if (maildata.objInspectionCallRequest.ExternalAgencyEmail != null && maildata.objInspectionCallRequest.ExternalAgencyEmail != string.Empty)
        //        //{
        //        //    ToEmailIds.AddRange(maildata.objInspectionCallRequest.ExternalAgencyEmail?.Split(',').ToList());

        //        //}
        //        #endregion

        //        #region CC EMAIL IDS
        //        //if (maildata.objInspectionCallRequest.ExternalAgencyCcEmail != null && maildata.objInspectionCallRequest.ExternalAgencyCcEmail != string.Empty)
        //        //{
        //        //    CCEmailIds.AddRange(maildata.objInspectionCallRequest.ExternalAgencyCcEmail?.Split(',').ToList());

        //        //}

        //        #region Get Plant Admin Mail
        //        int CurrentPlantId = maildata.objInspectionCallRequest.PlantId ?? 0;
        //        List<UserRole> RoleWithPlantAdmin = uow.UserRoleRepository.GetAll(x => x.Role.RoleWiseRights.Select(r => r.RightId).Contains((int)SystemEnums.Rights.ManagePlantLevelMasterData)).ToList();

        //        RoleWithPlantAdmin.ForEach(x =>
        //        {
        //           // if (x.User.PlantId == CurrentPlantId) CCEmailIds.Add(x.User.EmailId);

        //        });
        //        #endregion
        //        //Always add Current User in CC
        //        //CCEmailIds.Add(CurrentUserMail);
        //        #endregion

        //      //  maildata.To = string.Join(",", ToEmailIds);
        //      //  maildata.Cc = string.Join(",", CCEmailIds);
        //        maildata.UserName = Objuser.FirstName + " " + Objuser.LastName;
        //        maildata.User = Objuser;

        //        #region FILE ATTACHMENT

             
        //        List<long> uploadedFile = new List<long>();

        //        #region REFERNCE DOCMENT
        //        uow.InspectionCallWiseReferenceDocumentRepository.GetAll(x => x.InspectionCallId == maildata.objInspectionCallRequest.InspectionCallId).ToList().ForEach(f =>
        //             uploadedFile.Add(f.FileId)
        //            );

        //        #endregion
        //        #region INTERNALDOCUMENT
        //        uow.InspectionCallWiseIntenalInspectionDocumentRepository.GetAll(x => x.InspectionCallId == maildata.objInspectionCallRequest.InspectionCallId).ToList().ForEach(f =>
        //             uploadedFile.Add(f.FileId)
        //            );
        //        #endregion

        //        #region EarlierReport
        //        if (maildata.objInspectionCallRequest.EarlierInspectionReportId!=null && maildata.objInspectionCallRequest.EarlierInspectionReportId!=0)
        //        {
        //            uploadedFile.Add((long)maildata.objInspectionCallRequest.EarlierInspectionReportId);
        //        }
        //        #endregion
        //        #region Partail Approval Form
        //        //if (maildata.objInspectionCallRequest.JobOffered==(int)SystemEnums.InspectionJobOffered.PartialSupply && maildata.objInspectionCallRequest.PartialSupplierApprovalFormId != 0)
        //        //{
        //        //    uploadedFile.Add(maildata.objInspectionCallRequest.PartialSupplierApprovalFormId);
        //        //}
        //        #endregion
        //        maildata.Filepath = new List<string>();
        //        uploadedFile.ForEach(x =>
        //        {
        //            maildata.Filepath.Add(uow.UploadedFileRepository.Get(x).FilePath);
        //        });

        //        #endregion
        //        maildata.From = "";
        //        maildata.ReplyBackTo = CurrentUserMail;
        //        maildata.Subject = "KSB Training Project - " + objInspectionCallRequest.NotificationNumber;
        //        string HtmlString = ConvertViewToString("~/Views/Email/InspectionCallEmail.cshtml", maildata, viewData, ControllerContext);
        //        maildata.Body = HtmlString.Replace("&quot;", "\"");

        //        bool mailResult = await Mail.SendMail(maildata);
        //        Result = true;
        //        Message = "Call Forwarded SuccessFully !";
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = false;
        //        Message = "Call Forward Failed !";
        //        CustomErrorHandler.writelog(ex);
        //    }

        //}
        #endregion

        #region SetMailDataAccordingTo Flow
        /// <summary>
        /// SET TO AND CC EMAIL IDS DEPEND ON FLOW OF DATA
        /// KSB TO SUPPLIER / SUPPLIER TO KSB
        /// </summary>
        /// <param name="mailData"></param>
        /// <param name="CurrentUserMail"></param>
        void SetMailData(MailData mailData ,string CurrentUserMail)
        {
            List<string> ToEmailIds = new List<string>();
            List<string> CCEmailIds = new List<string>();
            #region SUPPLIER to KSB  

            if (!true)
            {
                #region TO EMAIL IDS
               
                List<UserRole> RoleWithKSBUpdateRight = uow.UserRoleRepository.GetAll(x => x.Role.RoleWiseRights.Select(r => r.RightId).Contains((int)SystemEnums.Rights.UpdateInspectionCallByKSB)).ToList();

                //RoleWithKSBUpdateRight.ForEach(x =>
                //{
                //    if (x.User.PlantId == (int)mailData.objInspectionCallRequest.PlantId) ToEmailIds.Add(x.User.EmailId);

                //});


                #endregion

                #region CC EMAILIDS
              
              
                //if (mailData.objInspectionCallRequest.PurcahseEmailId != null && mailData.objInspectionCallRequest.PurcahseEmailId != string.Empty)
                //{
                //    CCEmailIds.AddRange(mailData.objInspectionCallRequest.PurcahseEmailId?.Split(',').ToList());
                //}
                //if (mailData.objInspectionCallRequest.CMEmailId != null && mailData.objInspectionCallRequest.CMEmailId != string.Empty)
                //{
                //    CCEmailIds.AddRange(mailData.objInspectionCallRequest.CMEmailId?.Split(',').ToList());
                //}
               

                #endregion


            }
            #endregion


            #region KSB  TO Supplier
            else
            {
                //SUPPLIER EMAIL ID
                //string SupplierEmailID = uow.UserRepository.Get(mailData.objInspectionCallRequest.CreatedBy).EmailId;
                //if (SupplierEmailID != null)
                //{
                //    ToEmailIds.Add(SupplierEmailID);
                //}
            }
            #endregion

            #region Get Plant Admin Mail
          //  int CurrentPlantId = mailData.objInspectionCallRequest.PlantId ?? 0;
            List<UserRole> RoleWithPlantAdmin = uow.UserRoleRepository.GetAll(x => x.Role.RoleWiseRights.Select(r => r.RightId).Contains((int)SystemEnums.Rights.ManagePlantLevelMasterData)).ToList();

            //RoleWithPlantAdmin.ForEach(x =>
            //{
            //    if (x.User.PlantId == CurrentPlantId) CCEmailIds.Add(x.User.EmailId);

            //});
            #endregion

            //Always add Current User in CC
            CCEmailIds.Add(CurrentUserMail);
            mailData.To = string.Join(",", ToEmailIds);
            mailData.Cc = string.Join(",", CCEmailIds);
        }
        #endregion

        #region Mail method when New user is created

        /// <summary>
        /// If the Client ask vendor for revision of document then this mail will get send to vendor by KSB
        /// </summary>
        /// <returns></returns>
        public async void NewUserMail(int UserId, ControllerContext ControllerContext, ViewDataDictionary viewData)
        {
            try
            {
                string UserEmail = uow.UserRepository.Get(x => x.UserId == UserId).EmailId;
                string CurrentUserMail = uow.UserRepository.Get(x => x.UserId == CurrentUser.Id).EmailId;
                User user = uow.UserRepository.Get(x => x.UserId == UserId);
               
                MailData maildata = new MailData();
                maildata.To = UserEmail;
                maildata.Cc = CurrentUserMail;
                maildata.UserName = user.FirstName + " " + user.LastName;
                maildata.User = user;
                maildata.From = "";
                maildata.ReplyBackTo = CurrentUserMail;
                maildata.Subject = "Welcome to KSB";
                string HtmlString = ConvertViewToString("~/Views/Email/NewUser.cshtml", maildata, viewData, ControllerContext);
                maildata.Body = HtmlString;

                bool mailResult = await Mail.SendMail(maildata);
                Result = true;
                Message = "User Created Successfully !";
            }
            catch (Exception ex)
            {
                Result = false;
                Message = "User is created Successfully But Email Failed To Be Sent !";
                CustomErrorHandler.writelog(ex);
            }

        }

        #endregion

        #region Mail method when New user is created

        /// <summary>
        /// If the Client ask vendor for revision of document then this mail will get send to vendor by KSB
        /// </summary>
        /// <returns></returns>
        public async void ForgotPassword(string Username_mailID, ControllerContext ControllerContext, ViewDataDictionary viewData)
        {
            try
            {
                User user = uow.UserRepository.GetAll(x => x.UserName.ToUpper() == Username_mailID.ToUpper()).FirstOrDefault();
                if (user != null)
                {
                    MailData maildata = new MailData();
                    maildata.To = user.EmailId;
                    maildata.UserName = user.FirstName + " " + user.LastName;
                    maildata.User = user;
                    maildata.From = "";
                    maildata.Subject = "Password Recovery";
                    string HtmlString = ConvertViewToString("~/Views/Email/ForgotPassword.cshtml", maildata, viewData, ControllerContext);
                    maildata.Body = HtmlString;

                    bool mailResult = await Mail.SendMail(maildata);
                    Result = true;
                    Message = "Password Has Been Send To Your Email !";
                }
                else
                {
                    Result = false;
                    Message = "Username Or EmailId Does Not Exists !";
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Message = "Username Or EmailId Does Not Exists !";
                CustomErrorHandler.writelog(ex);
            }

        }

        #endregion

        #region Convert View To String

        /// <summary>
        /// This method is call for converting view to string 
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="viewData"></param>
        /// <param name="ControllerContext"></param>
        /// <returns></returns>
        public string ConvertViewToString(string viewName,object model,ViewDataDictionary viewData,ControllerContext ControllerContext)
        {       
            viewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(ControllerContext, vResult.View, viewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            uow?.Dispose();
        }
        #endregion

        #endregion

    }
}