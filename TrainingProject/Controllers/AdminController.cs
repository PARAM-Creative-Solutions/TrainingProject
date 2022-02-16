using TrainingProject.Security;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    public class AdminController : BaseController
    {

        #region Constructor

        /// <summary>
        /// Initilizes instance of unit of work
        /// </summary>
        public AdminController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }

        #endregion

        #region Index
        /// <summary>
        /// list of Admins
        /// </summary>
        /// <returns></returns>
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.users = uow.UserRepository.GetAll().Count();
            ViewBag.material = uow.UserRepository.GetAll().Count();
            return View();
        }
        #endregion

        #region Details
        // GET: Admin/Details/5
        /// <summary>
        /// Get Details Of Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View();
        }



        #endregion

        #region Create

        #region Get
        // GET: Admin/Create
        /// <summary>
        /// Create Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Post
        // POST: Admin/Create
        /// <summary>
        /// Save Created User Details On Server
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        #endregion

        #region Edit

        #region Get
        // GET: Admin/Edit/5
        /// <summary>
        /// Get Admin Details to Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            return View();
        }
        #endregion

        #region Post

        // POST: Admin/Edit/5
        /// <summary>
        /// Save Updated Admin Details on Server
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        #endregion

        #region Delete
        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Online users
        /// <summary>
        /// Gets the list of online users for plant admin with same plant
        /// by priyanka gaharwal 22/05/2019
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineUsers()
        {
            var plantid = uow.UserRepository.Get(x => x.UserId == CurrentUser.Id).Department.PlantId;
            IEnumerable<UserLog> onlineUsers = uow.UserLogsRepository.GetAll(x => x.OnlineStatus == true && x.User.Department.PlantId == plantid);
            return View(onlineUsers);
        }
        #endregion

        #region Logout users
        /// <summary>
        /// Logouts the selected user
        /// </summary>
        /// <param name="Id">Userid</param>
        /// <returns></returns>
        public JsonResult LogoutUser(int Id)
        {
            List<UserLog> UserLogs = uow.UserLogsRepository.GetAll(u => u.UserId == Id && u.OnlineStatus).ToList();
            if (UserLogs != null)
            {
                foreach (UserLog userLog in UserLogs)
                {
                    userLog.OnlineStatus = false;
                    //ulog.IsOfflineByAdmin = true;
                    uow.UserLogsRepository.Update(userLog);
                    uow.SaveChanges();
                    MyApplicationHub.LogOutUser(userLog.ConnectionId);
                }
               
            }
            return Json(new { Result = true, Message = "User Logout Successfully !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
