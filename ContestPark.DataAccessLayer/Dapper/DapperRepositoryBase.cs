using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.Entities.Helpers;
using DapperExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.DataAccessLayer.Dapper
{
    public class DapperRepositoryBase<TEntity> : DatabaseConnection, IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        #region Constructor

        public DapperRepositoryBase(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region CRUD operations

        /// <summary>
        /// Silme işlemi
        /// </summary>
        /// <param name="id">Primary id</param>
        public void Delete(int id)
        {
            var entity = Connection.Get<TEntity>(id);
            bool isSuccess = Connection.Delete(entity);
            // TODO hata oluşursa mesaj döndür
        }

        /// <summary>
        /// Entities içinde arama yapmar
        /// örneğin:  Find(f => f.UserId >0).ToList() OR FirstOrDefault(); vb.
        /// </summary>
        /// <returns>Aranan değerler</returns>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Connection.GetList<TEntity>(predicate).AsQueryable();
        }

        /// <summary>
        /// Entity ekleme
        /// </summary>
        /// <param name="entity">Entity modeli</param>
        public void Insert(TEntity entity)
        {
            int id = Connection.Insert<TEntity>(entity);
            if (id <= 0) Check.BadStatus("Insert error");
        }

        /// <summary>
        /// Entity güncelleme
        /// </summary>
        /// <param name="entity">Entity modeli</param>
        public void Update(TEntity entity)
        {
            Connection.Update<TEntity>(entity);
        }

        #endregion CRUD operations
    }
}