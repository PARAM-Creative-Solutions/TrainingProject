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
    ///  Used to peform the operations on Roles
    /// </summary>
    public class RoleRepository : IRepository<Role>
    {
        private UnitofWork _unitOfWork;
        internal DbSet<Role> dbSet;

        #region Methods

        /// <summary>
        /// Initializes instance of unit of work
        /// </summary>
        /// <param name="unitOfWork"></param>
        public RoleRepository(UnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            dbSet = unitOfWork.Db.Set<Role>();
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetAll()
        {
            //return dbSet.Select(a => new Smart_DMS.Entities.Role { CreatedByRole = string.Concat(_unitOfWork.RolesRepo.Get(q => q.RoleId == a.CreatedBy).First_Name, " ", _unitOfWork.RolesRepo.Get(q => q.RoleId == a.CreatedBy).Last_Name), ModifiedByRole = a.LastModifiedBy == null ? string.Empty : string.Concat(_unitOfWork.RolesRepo.Get(s => s.RoleId == a.LastModifiedBy).First_Name, " ", _unitOfWork.RolesRepo.Get(q => q.RoleId == a.LastModifiedBy).Last_Name), First_Name = a.First_Name, Last_Name = a.Last_Name, Dept= a.Dept, RoleStatus = a.RoleStatus, PlantId = a.PlantId, Roles = a.Roles });
            return dbSet.Where(r => r.RoleId != 1).ToList();
        }

        /// <summary>
        /// Get Role with id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Role Get(object Id)
        {
            return dbSet.Find(Id);
        }

        /// <summary>
        /// Add Role entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Role entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Add multiple Role entites 
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<Role> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <summary>
        /// Update Role entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Role entity)
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
            Role ent = _unitOfWork.Db.Set<Role>().Find(Id);
            _unitOfWork.Db.Entry(ent).State = EntityState.Detached;

        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Role entity)
        {
            dbSet.Remove(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Deleted;

        }

        /// <summary>
        /// Remove entity 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<Role> entities)
        {
            dbSet.RemoveRange(entities);
            _unitOfWork.Db.Entry(entities).State = EntityState.Deleted;

        }


        /// <summary>
        /// Get all Role with predicate 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Role> GetAll(Func<Role, bool> predicate = null)
        {
            if (predicate != null)
            {
                if (predicate != null)
                {
                    return dbSet.Where(predicate).AsEnumerable().Where(r=>r.RoleId!=1).AsEnumerable();
                }
            }
            return dbSet.ToList();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Role Get(Func<Role, bool> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        #endregion
    }
}
