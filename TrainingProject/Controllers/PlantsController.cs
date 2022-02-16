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
using TrainingProject.Models.BLL;
using TrainingProjectDataLayer.Constants;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// Plants Controller
    /// </summary>
    public class PlantsController : BaseController
    {

        #region Constructor
        /// <summary>
        /// Creates the instance of Unit of work 
        /// </summary>
        public PlantsController()
        {
            uow = new UnitofWork(new TrainingProjectDataLayer.DataLayer.Entities.DAL.TrainingProjectEntities());
        }
        #endregion

        #region Method

        #region Index
        // GET: Plants
        /// <summary>
        /// Get All List Of Plants Data
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(uow.PlantRepository.GetAll());
        }
        #endregion

        #region Details
        /// <summary>
        ///  // GET: Plants/Details/5
        /// Get Details of Plants
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = uow.PlantRepository.Get(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        #endregion

        #region Create 

        #region Get
        // GET: Plants/Create
        /// <summary>
        /// Get Data To Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            //ViewBag.MdlLevelIds = new SelectList(uow.MDLLevelRepository.GetAll(), "MDLLevelId", "LevelName");
            ViewBag.CheckScopeSheetApplicable = HelperMethods.GetYesNoSelectList(false);
            ViewBag.CheckDraftsmanApplicable = HelperMethods.GetYesNoSelectList(false);
            ViewBag.CheckMultipleEnggDocumentation = HelperMethods.GetYesNoSelectList(false);
            return View();
        }
        #endregion

        #region Post

        /// <summary>
        ///  // POST: Plants/Create
        ///  //Save Created Data On Server
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Plant plant)
        {
            if (ModelState.IsValid)
            {
                BL_Plants objBL_Plants = new BL_Plants();
                objBL_Plants.PostCreatePlant(plant);
                return Json(new { Result = true, Message = "Plant Created Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Fail To Create Plant !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Edit

        #region Get
        /// <summary>
        /// Get Data To Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Plants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = uow.PlantRepository.Get(id);
            var status = uow.PlantStatusRepository.GetAll().ToList();
            ViewBag.Status = new SelectList(status, "PlantStatusId", "Status", plant.PlantStatusId);
            if (plant == null)
            {
                return HttpNotFound();
            }
            BL_Plants objBL_Plants = new BL_Plants();
            return View(plant);
        }
        #endregion

        #region Post
        /// <summary>
        /// Post Updated Data On Server
        ///  // POST: Plants/Edit/5
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Plant plant)
        {
            if (ModelState.IsValid)
            {
                BL_Plants objBL_Plants = new BL_Plants();
                objBL_Plants.PostEditPlant(plant);
                return Json(new { Result = true, Message = "Plant Updated Successfully !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = false, Message = "Failt To Update Plant !" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Delete

        /// <summary>
        ///  // GET: Plants/Delete/5
        ///  //Delete Data
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
                Plant plant = uow.PlantRepository.Get(id);
                if (plant == null)
                {
                    return HttpNotFound();
                }
                uow.PlantRepository.Remove(plant);
                uow.SaveChanges();
                return Json(new { success = true, result = "Plant Deleted Successfully !!" });
            }
            catch (Exception ex)
            {
                CustomErrorHandler.writelog(ex);
                return Json(new { Result = false, Message = "Fail to Delete Plant !" });
            }
        }

        #endregion

        #region Plant Admin Index
        /// <summary>
        /// Get Plant Admin List
        /// </summary>
        /// <returns></returns>
        public ActionResult PlantAdminIndex()
        {
            List<User> _user = uow.UserRepository.GetAll(x => x.Department.Name == HelperMethods.GetDescription(SystemEnums.Departments.ADMIN)).Select(s => { s.Password = Security.CustomEncryption.Decrypt(s.Password); return s; }).ToList();
            return View(_user);
        }
        #endregion

        #region Create Plant Admin

        #region Get
        /// <summary>
        /// Get Details To Create Plant Admin Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreatePlantAdmin()
        {
            ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name");
            ViewBag.Roles = new SelectList(uow.RoleRepository.GetAll(), "RoleId", "RoleName");
            ViewBag.Plant = new SelectList(uow.PlantRepository.GetAll(), "PlantId", "PlantName");

            return View();
        }
        #endregion

        #region Post
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatePlantAdmin(User user)
        {
            BL_Plants bl_plant = new BL_Plants();
            bl_plant.CreateNewPlantAdmin(user);
            return Json(new { Result = bl_plant.Result, Message = bl_plant.Message }, JsonRequestBehavior.AllowGet);
         }
        #endregion
        #endregion

        #region Plant Admin Detail
        /// <summary>
        /// Gets the details of Plant Admin
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult PlantAdminDetail(int? id)
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
            ViewBag.AllRoles = new MultiSelectList(uow.RoleRepository.GetAll().ToList(), "RoleId", "RoleName", user.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToArray());
            var status = uow.UserStatusRepository.GetAll().ToList();
            ViewBag.Status = new SelectList(status, "StatusId", "Status", user.UserStatus);
            return View(user);
        }
        #endregion

        #region EditPlantAdmin

        #region EditPlantAdmin Get Method
        /// <summary>
        /// Gets the view to edit the user details
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult EditPlantAdmin(int? id)
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
            ViewBag.Status = new SelectList(status, "StatusId", "Status",user.UserStatus);
            user.Password = CustomEncryption.Decrypt(user.Password);
            ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name", user.DeptId);
            ViewBag.AllRoles = new MultiSelectList(uow.RoleRepository.GetAll().ToList(), "RoleId", "RoleName", user.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToArray());
            return View(user);
        }
        #endregion

        #region EditPlantAdmin Post Method

        /// <summary>
        /// Save changes the details of user to the server
        /// </summary>
        /// <param name="user">object of user</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlantAdmin(User user)
        {
            BL_Plants bl_plant = new BL_Plants();
            bl_plant.EditAdmin(user);
            return Json(new { Result = bl_plant.Result, Message = bl_plant.Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region DeletePlantAdmin

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public ActionResult DeletePlantAdmin(int? id)
        {
            BL_Plants bl_plant = new BL_Plants();
            bl_plant.DeleteAdmin(id);
            return Json(new { Result = bl_plant.Result, Message = bl_plant.Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose The Object
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
