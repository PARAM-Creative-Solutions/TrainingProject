using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingProject.Security;
using System.Web.Security;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProject.Models.BLL;
using TrainingProject.Models.MailManager;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TrainingProjectDataLayer.Constants;

namespace TrainingProject.Controllers
{

    /// <summary>
    /// Controller to perform opertaions on user 
    /// </summary>
    public class UsersController : BaseController
    {

        #region Constructor
        /// <summary>
        /// Creates the instance of Unit of work 
        /// </summary>
        public UsersController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        #endregion

        #region METHODS

        #region doesUserNameExist
        /// <summary>
        /// Checks the user name is already is there or not
        /// </summary>
        /// <param name="UserName">username from UI</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult doesUserNameExist(string UserName)
        {
            var user = uow.UserRepository.GetAll().Where(u => u.UserName.ToUpper() == UserName.ToUpper()).FirstOrDefault();
            return Json(user == null);
        }
        #endregion

        #region Index
        /// <summary>
        /// Gets the list of already created users
        /// </summary>
        /// <returns>returns the user list</returns>
        public ActionResult Index()
        {  //COMMENTED  ON  8Th july 2021 as All users will be visible to all PLANT Admin
           // List<User> _user = uow.UserRepository.GetAll(x => x.PlantId == CurrentUser.PlantId && x.DeptId!=(int)SystemEnums.Departments.ADMIN).ToList();
            List<User> _user = uow.UserRepository.GetAll().ToList();

            return View(_user);
        }

        /// <summary>
        /// Get the list of Users
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUserIndex()
        {
            List<User> _user = uow.UserRepository.GetAll()
            .Select(s => { s.Password = Security.CustomEncryption.Decrypt(s.Password); return s; }).ToList();
            return View("Index", _user);
        }


        #endregion

        #region Details
        /// <summary>
        /// Gets the details of user
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = uow.UserRepository.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = CustomEncryption.Decrypt(user.Password);
            ViewBag.AllRoles = new MultiSelectList(uow.RoleRepository.GetAll().ToList(), "RoleId", "RoleName", user.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToArray() /*user.UserRoles.Where(w => w.UserId != 0).Select(x => x.Role.RoleId)*/);
            return View(user);
        }
        #endregion

        #region Create

        #region Create Get Method

        /// <summary>
        /// Gets the view to create new user
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            int UserCount = uow.UserRepository.GetAll(c=>c.UserStatus== (int)SystemEnums.UserStatus.Active).Count();
            if (UserCount>110)
            {
                return Json(new { Result = true, Message = "110 User License Has Been Exceded !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {    //SHOW ALL DEPT TO SYSADMIN ONLY
                if (CurrentUser.IsWithRight(SystemEnums.Rights.ViewAllUsersData))
                {
                    ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name");
                 
                }
                else
                { //PLANT LEVEL ADMIN
                    //ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(x=>x.DeptId!=(int)SystemEnums.Departments.ADMIN), "DeptId", "Name");
                    ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name");

                }

                ViewBag.Roles = new SelectList(uow.RoleRepository.GetAll(), "RoleId", "RoleName");
                ViewBag.VendorList = new SelectList(uow.RoleRepository.GetAll().ToList(), "RoleId", "RoleName");
                ViewBag.Plant = new SelectList(uow.PlantRepository.GetAll(x => x.PlantId == CurrentUser.PlantId), "PlantId", "PlantName");
                ViewBag.AllEmails = JsonConvert.SerializeObject(uow.UserRepository.GetAll().Select(s => { return new TagsInputAutoCompleteItem() { id = s.UserId.ToString(), label = s.FirstName + " " + s.LastName, value = s.EmailId }; }).ToArray());
                return View();
            }          
        }

        #endregion

        #region Create Post Method

        /// <summary>
        /// Submits the Newly created user to the server
        /// </summary>
        /// <param name="user">object of user</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        //[Bind(Include = "UserId,FirstName,LastName,UserName,Password,EmailId,Init,DeptId,RoleID,CreatedBy,CreatedOn,VendorListId,LastModifiedOn,LastModifiedBy")]
        {
            if (ModelState.IsValid)
            {
                BL_Users newUser = new BL_Users(uow);
                newUser.CreateNewUser(user);
                if (!newUser.Result)
                {
                    return Json(new { Result = newUser.Result, Message = newUser.Message }, JsonRequestBehavior.AllowGet);
                }

                BL_Email bl = new BL_Email();
                bl.NewUserMail(user.UserId, ControllerContext, this.ViewData);
                return Json(new { Result = bl.Result, Message = bl.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail to create user." }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Edit

        #region Edit Get Method

        /// <summary>
        /// Gets the view to edit the user details
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = uow.UserRepository.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var status = uow.UserStatusRepository.GetAll().ToList();
            ViewBag.Status = new SelectList(status, "StatusId", "Status", user.UserStatus);
            user.Password = CustomEncryption.Decrypt(user.Password);
            ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name", user.DeptId);
            ViewBag.VendorList = new SelectList(uow.RoleRepository.GetAll().ToList(), "RoleId", "RoleName"); //If User Department will change to Vendor then this List will be displayed
            ViewBag.AllRoles = new MultiSelectList(uow.RoleRepository.GetAll().ToList(), "RoleId", "RoleName", user.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToArray());
            int VendorId = 0;//user.VendorUsersLists?.FirstOrDefault()?.VendorId ?? 0;
            user.VendorListId = VendorId;
            return View(user);
        }

        #endregion

        #region Edit Post Method

        /// <summary>
        /// Save changes the details of user to the server
        /// </summary>
        /// <param name="user">object of user</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        int usercount = uow.UserRepository.GetAllAsNoTracking(c => c.UserStatus == (int)SystemEnums.UserStatus.Active).Count();
                        if (usercount <= 110 || user.UserStatus==(int)SystemEnums.UserStatus.InActive)                        {
                           if (user != null)
                           {
                             Department _dept = uow.DepartmentRepository.Get(c => c.DeptId == user.DeptId);
                             //if (_dept.Name == HelperMethods.GetDescription(SystemEnums.Departments.SUPPLIER))
                             //{
                             //    VendorUsersList _vendor = uow.VendorUsersListRepository.Get(x => x.UserId == user.UserId);
                             //    if (_vendor == null)
                             //    {
                             //        uow.VendorUsersListRepository.Add(new VendorUsersList { UserId = user.UserId, VendorId = user.VendorListId });
                             //    }
                             //    else
                             //    {
                             //        _vendor.VendorId = user.VendorListId;
                                      
                             //        uow.VendorUsersListRepository.Update(_vendor);
                             //    }
                             //}
                             //else
                             //{
                             //    List<VendorUsersList> _vendor = uow.VendorUsersListRepository.GetAll(x => x.UserId == user.UserId).ToList();
                             //    uow.VendorUsersListRepository.RemoveRange(_vendor);
                             //}
                             List<UserRole> UserAllRoles = uow.UserRoleRepository.GetAll(u => u.UserId == user.UserId).ToList();
                             uow.UserRoleRepository.RemoveRange(UserAllRoles);
                             user.SelectedRoles.All(r =>
                             {
                                uow.UserRoleRepository.Add(new UserRole()
                                {
                                    RoleId = r,
                                    CreatedBy = CurrentUser.Id,
                                    CreatedOn = DateTime.Now,
                                    UserId = user.UserId
                                }); return true;
                             });
                             user.Password = CustomEncryption.Encrypt(user.Password);//Crypto.Hash(user.Password);
                             user.LastModifiedBy = CurrentUser.Id;
                             user.LastModifiedOn = DateTime.Now;
                                //ADDED BY ANAGHA 
                                uow.Db.Entry(user).State = EntityState.Modified;

                                uow.UserRepository.Update(user);
                             uow.SaveChanges();
                             transaction.Commit();
                             return Json(new { Result = true, Message = "User Updated Successfully  !" }, JsonRequestBehavior.AllowGet);
                         }
                     }
                     else
                     {
                        return Json(new { Result = false, Message = "110 User License Has Been Exceded !!" }, JsonRequestBehavior.AllowGet);
                     }
                   }//model if close
                   return Json(new { Result = false, Message = "Can Not Update User" }, JsonRequestBehavior.AllowGet);
                }//try close
                catch (Exception ex)
                {
                    transaction.Rollback();
                    CustomErrorHandler.writelog(ex);
                    return Json(new { Result = false, Message = "Failed To Update User !" }, JsonRequestBehavior.AllowGet);
                }
               
            }
        }

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (id != null)
                    {
                        User user = uow.UserRepository.Get(id);
                        uow.UserRepository.Remove(user);
                        uow.SaveChanges();
                    }
                    transaction.Commit();
                    return Json(new { success = true, result = "User Deleted Successfully !!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    CustomErrorHandler.writelog(ex);
                    return this.Json(new { Result = false, Message = "Fail To Delete User !" }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        #endregion
        #region InActive

        /// <summary>
        /// InActives the user
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult InActive(int? id)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (id != null)
                    {
                        User user = uow.UserRepository.Get(id);
                        user.UserStatus = (int)SystemEnums.UserStatus.InActive;
                        uow.SaveChanges();
                    }
                    transaction.Commit();
                    return Json(new { success = true, result = "User Updated Successfully !!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    CustomErrorHandler.writelog(ex);
                    return this.Json(new { Result = false, Message = "Fail To Update User !" }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        #endregion

        #region User Profile

        /// <summary>
        ///Gets the Profile to the User 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserProfile()
        {
            User user = uow.UserRepository.Get(CurrentUser.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = CustomEncryption.Decrypt(user.Password);
            return View(user);
        }

        /// <summary>
        /// Updated changes of user profile get submited to the server
        /// </summary>
        /// <param name="newuser"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserProfile(User newuser)
        {
            BL_Users updateUser = new BL_Users(uow);
            updateUser.UpdateUserProfile(newuser);
            return Json(new { Result = updateUser.Result, Message = updateUser.Message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region ListStaff
        /// <summary>
        /// Post the staff list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListStaff(int userId)
        {
            List<TempUserRoles> listofStaff = new List<TempUserRoles>();
            List<Role> CurrentUserRole = uow.UserRoleRepository.GetAll(w => w.UserId == userId).Select(s => s.Role).ToList();
            //Rest of Active Staff to be display on the left side of the duallist

            foreach (Role item in uow.RoleRepository.GetAll())
            {
                TempUserRoles temp = new TempUserRoles() { RoleId = item.RoleId, RoleName = item.RoleName };
                if (!CurrentUserRole.Contains(item))
                    temp.selected = false;
                else
                    temp.selected = true;
                listofStaff.Add(temp);
            }
            return Json(new { Result = listofStaff, Message = "Submitted Successfully !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }

    #region TempUserRoles Class
    class TempUserRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool selected { get; set; }
    }
    #endregion

    class TagsInputAutoCompleteItem
    {
        public string id { get; set; }
        public string label { get; set; }
        public string value { get; set; }
    }

}
