using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProject.Security;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// Department Controller
    /// </summary>
    public class DepartmentsController : BaseController
    {

        #region Constructor
        /// <summary>
        /// Creates the instance of Unit of work 
        /// </summary>
        public DepartmentsController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        #endregion

        #region METHODS

        #region Index
        
        /// <summary>
        /// Get List of All Departments On Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(uow.DepartmentRepository.GetAll());
        }

        #endregion

        #region Details
        // GET: Departments/Details/5
        /// <summary>
        /// Get Details Of Department with its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = uow.DepartmentRepository.Get(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }
        #endregion

        #region Create

        #region Create Get Method
        // GET: Departments/Create
        /// <summary>
        ///Get Method To Create Department 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create Post Method

       
        /// <summary>
        /// Post Method To create Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                department.CreatedBy = CurrentUser.Id;
                department.CreatedOn = DateTime.Now;
                department.PlantId = CurrentUser.PlantId;
                //department.Users = null;
                uow.DepartmentRepository.Add(department);
                uow.SaveChanges();
                return Json(new { Result = true, Message = "Department Created Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Create Department !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Edit

        #region Edit Get Method
        
        /// <summary>
        /// Get Department to Edit With Its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = uow.DepartmentRepository.Get(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }
        #endregion

        #region Edit Post Method
       
        /// <summary>
        /// Post Method to Submit Edited Data
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                department.LastModifiedBy = CurrentUser.Id;
                department.LastModifiedOn = DateTime.Now;
                uow.DepartmentRepository.Update(department);
                uow.SaveChanges();
                return Json(new { Result = true, Message = "Department Updated Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Update Department !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Delete

        // GET: Departments/Delete/5
        /// <summary>
        /// Get Method to Delete Department With Its ID
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
                Department department = uow.DepartmentRepository.Get(id);
                if (department == null)
                {
                    return HttpNotFound();
                }
                uow.DepartmentRepository.Remove(department);
                uow.SaveChanges();
                return Json(new { success = true, result = "Department Deleted Successfully !!" });
            }
            catch (Exception ex)
            {
                CustomErrorHandler.writelog(ex);
                return Json(new { success = false, result = "Fail to Delete Department !" });
             }
        }

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

        #endregion
    }
}
