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
using Newtonsoft.Json;
using TrainingProjectDataLayer.Constants;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// Right Controller
    /// </summary>
    public class RightsController : BaseController
    {

        #region Constructor
        /// <summary>
        /// Creates the instance of Unit of work 
        /// </summary>
        public RightsController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        #endregion

        #region Method

        #region Index
        /// <summary>
        /// Get All Rights Details
        /// </summary>
        /// <returns></returns>
        // GET: Rights
        public ActionResult Index()
        {

            return View(uow.RightRepository.GetAll());
        }
        #endregion

        #region Details
        // GET: Rights/Details/5
        /// <summary>
        /// Get Details Of Rights
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateSystemRights()
        {
            uow.RightRepository.RemoveAll();
            uow.RightRepository.AddRange(HelperMethods.GetEnumRightIntoList());
            uow.SaveChanges();  
            return Json(new { Result = true, Message = "System Rights Updated Successfully !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Details
        // GET: Rights/Details/5
        /// <summary>
        /// Get Details Of Rights
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Right right = uow.RightRepository.Get(id);
            if (right == null)
            {
                return HttpNotFound();
            }
            return View(right);
        }
        #endregion

        #region Create

        #region Get
        /// <summary>
        /// Create New Right 
        /// GET: Rights/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Post

        /// <summary>
        /// Save Created Right Details On Server
        ///POST: Rights/Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RightId,RightName,Description,Controller,Action")] Right right)
        {
            if (ModelState.IsValid)
            {
                uow.RightRepository.Add(right);
                uow.SaveChanges();
                return Json(new { Result = true, Message = "Right Created Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Create Right !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Edit

        #region Get
        /// <summary>
        /// Get Data From Server To Edit
        /// GET: Rights/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Right right = uow.RightRepository.Get(id);
            if (right == null)
            {
                return HttpNotFound();
            }
            return View(right);
        }

        #endregion

        #region Post

        /// <summary>
        /// Save Updated Right Details On Server
        /// POST: Rights/Edit/5
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RightId,RightName,Description,Controller,Action")] Right right)
        {
            if (ModelState.IsValid)
            {
                uow.RightRepository.Update(right);
                uow.SaveChanges();
                return Json(new { Result = true, Message = "Right Updated Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Update Right !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Delete
        /// <summary>
        /// Delete Right Data
        /// GET: Rights/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
             try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Right right = uow.RightRepository.Get(id);
                if (right == null)
                {
                    return HttpNotFound();
                }
                uow.RightRepository.Remove(right);
                uow.SaveChanges();
                return Json(new { success = true, result = "Right Deleted Successfully !!" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Fail to Delete Right !" });
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
