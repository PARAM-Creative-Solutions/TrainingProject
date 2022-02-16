using TrainingProject.Security;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static TrainingProjectDataLayer.Constants.SystemEnums;

namespace TrainingProject.Models.BLL
{
    /// <summary>
    /// Business Layer for Plants Controller
    /// </summary>
    public class BL_Plants :IDisposable
    {
        #region Properties

        UnitofWork uow;
        /// <summary>
        /// Property for Current User
        /// </summary>
        public CustomPrincipal Currentuser { get { return HttpContext.Current.User as CustomPrincipal; } }
       
        /// <summary>
        /// Property for Result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Property For Message
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// BL_Plants Constructor
        /// </summary>
        public BL_Plants()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }

        #endregion

        #region Method
        #region PostCreatePlant
        /// <summary>
        /// Post Method to Create Plant
        /// </summary>
        public bool PostCreatePlant(Plant plant)
        {
            plant.CreatedBy = Currentuser.Id;
            plant.CreatedOn = DateTime.Now;
            plant.PlantStatusId = (int)PlantStatus.Active;
            uow.PlantRepository.Add(plant);
            uow.SaveChanges();
            return true;
        }
        #endregion

        #region PostEditPlant
        /// <summary>
        /// Post Method to Update Plant
        /// </summary>
        public bool PostEditPlant(Plant plant)
        {
            plant.LastModifiedBy = Currentuser.Id;
            plant.LastModifiedOn = DateTime.Now;
            uow.PlantRepository.Update(plant);
            uow.SaveChanges();
            return true;
        }

        #endregion

        #region CreateNewPlantAdmin
        /// <summary>
        /// This method is called while creating new plant Admin
        /// </summary>
        /// <param name="user">Provide User Model</param>
        /// <returns></returns>
        public void CreateNewPlantAdmin(User user)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    user.Password = CustomEncryption.Encrypt(user.Password);
                    user.CreatedBy = Currentuser.Id;
                    user.CreatedOn = DateTime.Now;
                    user.PlantId = Currentuser.PlantId;
                    user.UserStatus = (int)SystemEnums.UserStatus.Active;
                    //user.AllStatu.Status = HelperMethods.GetDescription(DMSEnumUserStatus.Active);
                    uow.UserRepository.Add(user);
                    uow.SaveChanges();
                    //Code For Insert Multiple Roles In UserRoles Table Beacuse Here We Are Using DualListBox
                    List<int> NewRoleID = user.SelectedRoles;
                    if (NewRoleID.Count != 0)
                    {
                        foreach (var item in NewRoleID)
                        {
                            UserRole _UserRole = new UserRole();
                            _UserRole.RoleId = item;
                            _UserRole.CreatedOn = DateTime.Now;
                            _UserRole.CreatedBy = Currentuser.Id;
                            _UserRole.UserId = user.UserId; //important to assign UserId which we are gettting from model data
                            uow.UserRoleRepository.Add(_UserRole);
                            uow.SaveChanges();
                        }
                    }
                    uow.SaveChanges();
                    transaction.Commit();
                    Result = true;
                    Message = "Plant Admin Created Successfully !";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Result = false;
                    Message = "Fail To Create Plant Admin !";
                }
            }
        }
        #endregion

        #region EditPlantAdmin
        /// <summary>
        /// Update Admin Data
        /// </summary>
        /// <param name="user">Object Of User</param>
        public void EditAdmin(User user)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (user != null)
                        {
                            List<UserRole> UserAllRoles = uow.UserRoleRepository.GetAll(u => u.UserId == user.UserId).ToList();
                            uow.UserRoleRepository.RemoveRange(UserAllRoles);
                            user.SelectedRoles.All(r => {
                                uow.UserRoleRepository.Add(new UserRole()
                                {
                                    RoleId = r,
                                    CreatedBy = Currentuser.Id,
                                    CreatedOn = DateTime.Now,
                                    UserId = user.UserId
                                }); return true;
                            });
                            user.Password = CustomEncryption.Encrypt(user.Password);//Crypto.Hash(user.Password);
                            user.LastModifiedBy = Currentuser.Id;
                            user.LastModifiedOn = DateTime.Now;
                       
                            uow.UserRepository.Update(user);
                            uow.SaveChanges();
                            transaction.Commit();
                        Result = true;
                        Message = "Plant Admin Updated Successfully !";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Result = false;
                    Message = "Failed To Update Plant Admin !";
                }
            }
        }
        /// <summary>
        /// Delete Method for Plant Admin
        /// </summary>
        /// <param name="id">Id of User</param>
        public void DeleteAdmin(int? id)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (id != null)
                    {
                        User user = uow.UserRepository.Get(id);
                        List<UserRole> _userroles = uow.UserRoleRepository.GetAll(x => x.UserId == id).ToList();
                       uow.UserRoleRepository.RemoveRange(_userroles);
                        uow.SaveChanges();
                        uow.UserRepository.Remove(user);
                        uow.SaveChanges();
                    }
                    transaction.Commit();
                    Result = true;
                    Message = "Plant Admin Deleted Successfully !!";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Result = false;
                    Message = "Fail To Delete Plant Admin !";
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