using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Repository.Interface;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrainingProjectDataLayer.DataLayer.Repository.Implementation
{
    /// <summary>
    ///  Used to peform the operations on users
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private UnitofWork _unitOfWork;
        internal DbSet<User> dbSet;

        #region Methods

        /// <summary>
        /// Initializes instance of unit of work
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserRepository(UnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            dbSet = unitOfWork.Db.Set<User>();
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAll()
        { 
            return dbSet.Where(u=> (!u.UserRoles.Select(r=>r.RoleId).Contains(1))).ToList();
        }

        public IEnumerable<User> GetAllAsNoTracking(Func<User, bool> predicate = null)
        {
            if (predicate != null)
            {
                if (predicate != null)
                {
                    return dbSet.AsNoTracking().Where(predicate).AsEnumerable().Where(u => (!u.UserRoles.Select(r => r.RoleId).Contains(1))).AsEnumerable();
                }
            }
            return dbSet.ToList();
        }

        /// <summary>
        /// Get user with id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public User Get(object Id)
        {
            return dbSet.Find(Id);
        }

        /// <summary>
        /// Add user entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(User entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Add multiple user entites 
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<User> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <summary>
        /// Update user entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(User entity)
        {
            dbSet.Attach(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Modified;

        }

        /// <summary>
        /// Remove entity with id
        /// </summary>
        /// <param name="Id"></param>
        public void Remove(object Id)
        {
            User ent = _unitOfWork.Db.Set<User>().Find(Id);
            _unitOfWork.Db.Entry(ent).State = EntityState.Detached;

        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(User entity)
        {
            dbSet.Remove(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Deleted;

        }

        /// <summary>
        /// Remove entity 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<User> entities)
        {
            dbSet.RemoveRange(entities);
            _unitOfWork.Db.Entry(entities).State = EntityState.Deleted;

        }

        /// <summary>
        /// Validates the entity in database by checking the username and password
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public User ValidateUser(User entity)
        {
            User oValid = _unitOfWork.Db.Users.Where(x => x.UserName == entity.UserName && x.Password == entity.Password).FirstOrDefault();
            return oValid;
        }

        /// <summary>
        /// Get all user with predicate 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAll(Func<User, bool> predicate = null)
        {
            if (predicate != null)
            {
                if (predicate != null)
                {
                    return dbSet.Where(predicate).AsEnumerable().Where(u => (!u.UserRoles.Select(r => r.RoleId).Contains(1))).AsEnumerable();
                }
            }
            return dbSet.ToList();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public User Get(Func<User, bool> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        #region GetDraftsman
        /// <summary>
        /// Method returns the users which are Draftsman
        /// </summary>
        /// <returns>returns the collection of Draftsman</returns>
        public IEnumerable<User> GetDraftsman(int plantid, int pUserid,string role)
        {
            
            return _unitOfWork.Db.UserRoles.Where(x => (x.Role.RoleName == role && x.User.PlantId == plantid) || x.User.UserId == pUserid).Select(x => x.User).Distinct();
        }
        #endregion

        #region GetTop_Engineers
        // <summary>
        /// Method returns all the users which are TOP Engineers
        /// </summary>
        /// <returns>returns the collection of TOP Engineers</returns>
        public IEnumerable<User> GetTop_Engineers(int plantid,  string engg, string head)
        {
            return _unitOfWork.Db.UserRoles.Where(x => (x.Role.RoleName == engg || x.Role.RoleName == head) && x.User.PlantId == plantid).Select(x => x.User);
        }
        #endregion

        #region GetDeptHead
        // <summary>
        /// Method returns all the users which are TOP Engineers
        /// </summary>
        /// <returns>returns the collection of TOP Engineers</returns>
        public List<User> GetDeptHeadOrUsers(int DeptId)
        {
            Department dept = _unitOfWork.Db.Departments.Where(c => c.DeptId == DeptId).FirstOrDefault();
            if (dept.IsHead)
                return _unitOfWork.Db.Users.Where(x => (x.UserRoles.Select(v => v.RoleId).ToList().Contains((int)Constants.SystemEnums.Rights.ManagePlantLevelMasterData))).ToList();
            else
                return _unitOfWork.Db.Users.Where(x => x.DeptId == DeptId).ToList();
       }
        #endregion


        #region GET QA USERS
        ///// <summary>
        ///// Get all user with predicate 
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //public IEnumerable<User> GetAllQAUsers(Func<User, bool> predicate = null)
        //{
        //    if (predicate != null)
        //    {
        //        if (predicate != null)
        //        {
        //            return dbSet.Where(predicate).AsEnumerable().Where(x=>x.DeptId==(int)Constants.SystemEnums.Departments. ).Where(u => (!u.UserRoles.Select(r => r.RoleId).Contains(1))).AsEnumerable();
        //        }
        //    }
        //    return dbSet.ToList();
        //}

        #endregion

        #endregion
    }
}
