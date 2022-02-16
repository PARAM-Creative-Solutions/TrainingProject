using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectDataLayer.DataLayer.Repository.Interface
{
    /// <summary>
    /// Interface for Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Method Declatarions

        /// <summary>
        /// Declartion of GetAll() method
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate = null);

        /// <summary>
        /// Declartion of Get() with parameter Expression 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Get(Func<TEntity, bool> predicate);

        /// <summary>
        /// Declaratio of Get() of type TEntity with object type parameter
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        TEntity Get(object Id);

        /// <summary>
        /// Declaration of Add() with TEntity type parameter
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Declaration of AddRange() 
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Declaration of Update() with TEntity type method 
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Declaration of Remove() with object type parameter
        /// </summary>
        /// <param name="Id"></param>
        void Remove(object Id);

        /// <summary>
        /// Declaration of Remove() with TEntity type parameter
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Declaration of RemoveRange()
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);

        #endregion
    }
}
