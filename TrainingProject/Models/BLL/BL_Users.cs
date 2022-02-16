using TrainingProject.Models.FileUploadHelper;
using TrainingProject.Models.MailManager;
using TrainingProject.Security;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Models.BLL
{
    public class BL_Users : IDisposable
    {
        #region Properties
        public bool Result { get; set; }
        public string Message { get; set; }
        UnitofWork uow;
        CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }
        /// <summary>
        /// Property for PathManager
        /// </summary>
        public PathManager pathManager { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// BL_Email Constructor
        /// </summary>
        public BL_Users(UnitofWork puow)
        {
            uow = puow;
            //this.pathManager = new PathManager(uow);
        }

        #endregion

        #region Methods

        #region CreateNewUser
        /// <summary>
        /// This method is called while creating new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void CreateNewUser(User user)
        {
            
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    Department _dept = uow.DepartmentRepository.Get(c => c.DeptId == user.DeptId);
                    user.Password = CustomEncryption.Encrypt(user.Password);
                    user.CreatedBy = CurrentUser.Id;
                    user.CreatedOn = DateTime.Now;
                    user.PlantId = CurrentUser.PlantId;
                    user.UserStatus = (int)SystemEnums.UserStatus.Active;
                    user.PasswordLastModifiedOn = DateTime.Now;
                    uow.UserRepository.Add(user);
                    uow.SaveChanges();
                    //Code For Insert Multiple Roles In UserRoles Table Beacuse Here We Are Using DualListBox
                    List<int> NewRoleID = user.SelectedRoles;
                    #region ADDED BY ANAGHA , IF SUPPLIER ADD ROLE HARD CODED                   
                    if (NewRoleID == null)
                    {
                        if (_dept != null && _dept.Name == HelperMethods.GetDescription(SystemEnums.Departments.SUPPLIER))
                        {
                            NewRoleID = new List<int>() { 18 };
                        }
                   }
                    #endregion
                    if (NewRoleID != null)
                    {
                        foreach (var item in NewRoleID)
                        {
                            UserRole _UserRole = new UserRole();
                            _UserRole.RoleId = item;
                            _UserRole.CreatedOn = DateTime.Now;
                            _UserRole.CreatedBy = CurrentUser.Id;
                            _UserRole.UserId = user.UserId; //important to assign UserId which we are gettting from model data
                            uow.UserRoleRepository.Add(_UserRole);
                            uow.SaveChanges();
                        }
                    }                
                    //if (_dept != null && _dept.Name == HelperMethods.GetDescription(SystemEnums.Departments.SUPPLIER))
                    //{
                    //    uow.VendorUsersListRepository.Add(new VendorUsersList { UserId = user.UserId, VendorId = user.VendorListId });
                    //}

                    uow.SaveChanges();
                    transaction.Commit();
                    Result = true;
                    Message = "User Created Successfully !";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Result = false;
                    Message = "Fail To Create User !";
                    CustomErrorHandler.writelog(ex);
                }
            }
        }
        #endregion

        #region UpdateUserProfile
        /// <summary>
        /// For profile updation
        /// </summary>
        /// <param name="userProfile">Object Of User</param>
        /// <returns></returns>
        public void UpdateUserProfile(User userProfile)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    User objUser = uow.UserRepository.Get(x => x.UserId == userProfile.UserId);
                    if (objUser != null)
                    {
                        if(userProfile.httpFile!=null)
                        {
                            UploadedFile objUploadedFile = new UploadedFile();
                            pathManager.user = objUser;
                            //pathManager.setProfilePicturePath();
                            //HttpPostedFileBase objFile = new MemoryPostedFile(System.Convert.FromBase64String(userProfile.CroppedFile.Replace("data:image/png;base64,", string.Empty)), userProfile.httpFile.FileName, userProfile.httpFile.ContentType);
                            objUploadedFile = pathManager.SaveUploadedFileData(userProfile.httpFile);
                            objUser.Photo = objUploadedFile.Id;
                      
                        }
                        objUser.LastModifiedBy = CurrentUser.Id;
                        objUser.LastModifiedOn = DateTime.Now;
                        objUser.Password = CustomEncryption.Encrypt(userProfile.Password);
                        objUser.EmailNotifications = userProfile.EmailNotifications;
                        objUser.NotificationsInSystem = userProfile.NotificationsInSystem;

                        uow.UserRepository.Update(objUser);
                        uow.SaveChanges();
                        transaction.Commit();
                        Result = true;
                        Message = "Profile Updated Successfully !";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Result = false;
                    Message = "Failed to Update Profile !";
                    CustomErrorHandler.writelog(ex);
                }

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
