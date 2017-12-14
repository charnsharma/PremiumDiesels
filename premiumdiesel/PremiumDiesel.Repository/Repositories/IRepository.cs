using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PremiumDiesel.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Get

        TEntity Get(object id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        #endregion Get

        #region Add

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        #endregion Add

        #region Update

        void Update(TEntity entityToUpdate);

        #endregion Update

        #region Delete

        //void Delete(object id);

        void Delete(TEntity entity);

        //void DeleteRange(IEnumerable<TEntity> entities);

        #endregion Delete
    }
}