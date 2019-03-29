using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Repository arayüzü
    /// </summary>
    /// <typeparam name="T">Generic entity</typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity, new()
    {
        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(int id);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}