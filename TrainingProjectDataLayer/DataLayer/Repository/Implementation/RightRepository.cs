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
    ///  Used to peform the operations on Rights
    /// </summary>
    public class RightRepository : IRepository<Right>
    {
        private UnitofWork _unitOfWork;
        internal DbSet<Right> dbSet;

        #region Methods

        /// <summary>
        /// Initializes instance of unit of work
        /// </summary>
        /// <param name="unitOfWork"></param>
        public RightRepository(UnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            dbSet = unitOfWork.Db.Set<Right>();
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Right> GetAll()
        {
            //return dbSet.Select(a => new Smart_DMS.Entities.Right { CreatedByRight = string.Concat(_unitOfWork.RightsRepo.Get(q => q.RightId == a.CreatedBy).First_Name, " ", _unitOfWork.RightsRepo.Get(q => q.RightId == a.CreatedBy).Last_Name), ModifiedByRight = a.LastModifiedBy == null ? string.Empty : string.Concat(_unitOfWork.RightsRepo.Get(s => s.RightId == a.LastModifiedBy).First_Name, " ", _unitOfWork.RightsRepo.Get(q => q.RightId == a.LastModifiedBy).Last_Name), First_Name = a.First_Name, Last_Name = a.Last_Name, Dept= a.Dept, RightStatus = a.RightStatus, PlantId = a.PlantId, Rights = a.Rights });
            return dbSet.Where(r => r.RightId != 0).ToList();
        }

        /// <summary>
        /// Get Right with id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Right Get(object Id)
        {
            return dbSet.Find(Id);
        }

        /// <summary>
        /// Add Right entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Right entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Add multiple Right entites 
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<Right> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <summary>
        /// Update Right entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Right entity)
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
            Right ent = _unitOfWork.Db.Set<Right>().Find(Id);
            _unitOfWork.Db.Entry(ent).State = EntityState.Detached;

        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Right entity)
        {
            dbSet.Remove(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Deleted;

        }

        /// <summary>
        /// Remove entity 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<Right> entities)
        {
            dbSet.RemoveRange(entities);
            //_unitOfWork.Db.Entry(entities).State = EntityState.Deleted;

        }

        /// <summary>
        /// Remove entity 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveAll()
        {
            dbSet.RemoveRange(dbSet.AsEnumerable());
        }


        /// <summary>
        /// Get all Right with predicate 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Right> GetAll(Func<Right, bool> predicate = null)
        {
            if (predicate != null)
            {
                if (predicate != null)
                {
                    return dbSet.Where(predicate).AsEnumerable().Where(r=>r.RightId!=0).AsEnumerable();
                }
            }
            return dbSet.ToList();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Right Get(Func<Right, bool> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        #endregion
    }
}
