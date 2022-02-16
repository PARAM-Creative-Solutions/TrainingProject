using TrainingProjectDataLayer.DataLayer.Repository.Interface;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectDataLayer.DataLayer.Repository.Implementation
{
    /// <summary>
    /// Class for repository with all methods
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly UnitofWork _unitOfWork;
        internal DbSet<TEntity> dbSet;

        #region Methods

        /// <summary>
        /// Checks the instance of unit of work
        /// </summary>
        /// <param name="unitOfWork"></param>
        public Repository(UnitofWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = unitOfWork;
            this.dbSet = _unitOfWork.Db.Set<TEntity>();
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        /// <summary>
        /// Get entity with id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TEntity Get(object Id)
        {
            return dbSet.Find(Id);
        }

        /// <summary>
        /// Get all entites with predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate = null)
        {
            if (predicate != null)
            {
                if (predicate != null)
                {
                    return dbSet.Where(predicate);
                }
            }
            return dbSet.ToList();
        }

        /// <summary>
        /// Get single entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Get(Func<TEntity, bool> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Add entity and save back to the server
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Get Document path
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                                Func<IQueryable<TEntity>,
                                                IOrderedQueryable<TEntity>> orderBy = null,
                                                string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Add new entity and save back to server
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <summary>
        /// Update entity and save back to the server
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Modified;

        }

        /// <summary>
        /// Update entities and save back to the server
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateAll(IEnumerable<TEntity> entities)
        {
            foreach (TEntity item in entities)
            {
                dbSet.Attach(item);
                _unitOfWork.Db.Entry(item).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Remove entity with id
        /// </summary>
        /// <param name="Id"></param>
        public void Remove(object Id)
        {
            TEntity ent = _unitOfWork.Db.Set<TEntity>().Find(Id);
            _unitOfWork.Db.Entry(ent).State = EntityState.Detached;

        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Deleted;

        }

        /// <summary>
        /// Remove multiple entities 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);

        }

        #endregion
    }
}
