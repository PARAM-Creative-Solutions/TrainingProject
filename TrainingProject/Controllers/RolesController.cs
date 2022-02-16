using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProject.Security;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// Roles Contoller
    /// </summary>
    public class RolesController : BaseController
    {
        #region Constructor
        /// <summary>
        /// Creates the instance of Unit of work 
        /// </summary>
        public RolesController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        #endregion

        #region Method

        #region Index

        /// <summary>
        /// Get All Role List 
        /// GET: Roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(uow.RoleRepository.GetAll());
        }
        #endregion

        #region Details

        /// <summary>
        /// Get Detail Of Role With ID 
        /// GET: Roles/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = uow.RoleRepository.Get(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllRights = new MultiSelectList(uow.RightRepository.GetAll(), "RightId", "RightName", role.RoleWiseRights.Where(x => x.RoleId == id).Select(x => x.RightId)).ToList();
            return View(role);
        }

        #endregion

        #region Create

        #region Get
        /// <summary>
        /// Create New Role 
        /// GET: Roles/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            ViewBag.AllRights = new SelectList(uow.RightRepository.GetAll(), "RightId", "RightName");
            return View();
        }
        #endregion


        /// <summary>
        /// Save Created Role Details On Server
        /// POST: Roles/Create
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        role.CreatedBy = CurrentUser.Id;
                        role.PlantId = CurrentUser.PlantId;
                        role.CreatedOn = DateTime.Now;
                        uow.RoleRepository.Add(role);
                        uow.SaveChanges();
                        //Code For  Add Selected Rights In RoleWiseRights Table. Here,We Are Using DualListBox
                        List<int> _selectedRights = role.SelectedRights;
                        if (_selectedRights.Count != 0)
                        {
                            foreach (var item in _selectedRights)
                            {
                                RoleWiseRight _roleWiseRight = new RoleWiseRight();
                                _roleWiseRight.RightId = item;
                                _roleWiseRight.RoleId = role.RoleId;
                                uow.RoleWiseRightRepository.Add(_roleWiseRight);
                                uow.SaveChanges();
                            }
                        }
                        transaction.Commit();
                        return Json(new { Result = true, Message = "Role Created Successfully !" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Result = false, Message = "Can't Create Role !" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    CustomErrorHandler.writelog(ex);
                    return Json(new { Result = false, Message = "Fail To Create Role !" }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }
        #endregion

        #region Edit

        #region Get
        /// <summary>
        /// Get Data To Edit Role
        /// GET: Roles/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = uow.RoleRepository.Get(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllRights = new MultiSelectList(uow.RightRepository.GetAll(), "RightId", "RightName", role.RoleWiseRights.Where(x => x.RoleId == id).Select(x => x.RightId)).ToList();

            return View(role);
        }
        #endregion

        #region Post
        /// <summary>
        /// Save Edited Role Details On Server
        ///  POST: Roles/Edit/5
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        List<int> _currentRightID = uow.RoleWiseRightRepository.GetAll(x => x.RoleId == role.RoleId).Select(x => x.RightId).ToList(); //Fetch All Role IDs
                        List<int> _newRightID = new List<int>();
                        if (role.SelectedRights != null)
                        {
                            _newRightID = role.SelectedRights.Except(_currentRightID).ToList(); // Fetch New Added Rights
                        }
                        if (_newRightID.Count != 0)
                        {
                            foreach (var item in _newRightID)
                            {
                                RoleWiseRight _roleWiseRight = new RoleWiseRight();
                                _roleWiseRight.RoleId = role.RoleId;
                                _roleWiseRight.RightId = item;
                                uow.RoleWiseRightRepository.Add(_roleWiseRight);
                                uow.SaveChanges();
                            }
                        }
                        List<int> _removedRights = new List<int>();
                        if (_currentRightID!=null)
                         _removedRights = _currentRightID.Except(role.SelectedRights).ToList();
                        if (_removedRights.Count != 0)
                        {
                            List<RoleWiseRight> _rolewiseRight = uow.RoleWiseRightRepository.GetAll(x => x.RoleId == role.RoleId && _removedRights.Contains(x.RightId)).ToList();
                            uow.RoleWiseRightRepository.RemoveRange(_rolewiseRight);
                            uow.SaveChanges();
                        }
                        uow.RoleRepository.Update(role);
                        uow.SaveChanges();
                        transaction.Commit();
                        return Json(new { Result = true, Message = "Role Updated Successfully !" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Result = false, Message = "Can't Update Role !" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    CustomErrorHandler.writelog(ex);
                    return Json(new { Result = false, Message = "Fail To Update Role !" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #endregion

        #region Delete 

        /// <summary>
        /// Delete Role With ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var transaction = uow.Db.Database.BeginTransaction())
            {
                try
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Role role = uow.RoleRepository.Get(id);
                    List<RoleWiseRight> roleWiseRight = uow.RoleWiseRightRepository.GetAll(x => x.RoleId == role.RoleId).ToList();
                    uow.RoleWiseRightRepository.RemoveRange(roleWiseRight);
                    uow.SaveChanges();
                    if (role == null)
                    {
                        return HttpNotFound();
                    }
                    uow.RoleRepository.Remove(role);
                    uow.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true, result = "Role Deleted Successfully !!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    CustomErrorHandler.writelog(ex);
                    return Json(new { Result = false, Message = "Fail to Delete Role !" });
                }
            }
        }
        #endregion

            #region Dispose
            /// <summary>
            /// Dispose Object
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

        #endregion

    }
}
