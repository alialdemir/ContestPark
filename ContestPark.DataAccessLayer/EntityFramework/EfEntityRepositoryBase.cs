using ContestPark.DataAccessLayer.Abctract;
using ContestPark.Entities.ExceptionHandling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public class EfEntityRepositoryBase<TEntity> : Disposable, IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        #region Properties and fields

        private DbSet<TEntity> _dbSet;
        private DbContext _context;

        protected DbSet<TEntity> DbSet
        {
            get { return _dbSet ?? (_dbSet = DbContext.Set<TEntity>()); }
        }

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// DbContext sınıfı
        /// </summary>
        protected DbContext DbContext { get { return _context ?? (_context = DbFactory.Init()); } }

        #endregion Properties and fields

        #region Constructors

        public EfEntityRepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        #endregion Constructors

        #region Exception

        /// <summary>
        /// Özel hata fırlatma methodu
        /// </summary>
        /// <param name="exceptionMessage">Hata mesajı</param>
        public void BadStatus(string exceptionMessage)
        {
            if (String.IsNullOrEmpty(exceptionMessage))
                throw new ArgumentNullException(nameof(exceptionMessage));

            throw new NotificationException(exceptionMessage);
        }

        #endregion Exception

        #region CRUD operations

        /// <summary>
        /// Silme işlemi
        /// </summary>
        /// <param name="id">Primary id</param>
        public virtual void Delete(int id)
        {
            var deleteEntity = DbContext.Entry(GetById(id));
            deleteEntity.State = EntityState.Deleted;
            SaveChanges();
        }

        /// <summary>
        /// Entities içinde arama yapmar
        /// örneğin:  Find(f => f.UserId >0).ToList() OR FirstOrDefault(); vb.
        /// </summary>
        /// <returns>Aranan değerler</returns>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        /// <summary>
        /// Parametreden gelen primary key'e ait tek bir nesne
        /// </summary>
        /// <param name="id">Primary id</param>
        /// <returns>Entity</returns>
        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Entity ekleme
        /// </summary>
        /// <param name="entity">Entity modeli</param>
        public virtual void Insert(TEntity entity)
        {
            var addedEntity = DbContext.Entry(entity);
            addedEntity.State = EntityState.Added;
            SaveChanges();
        }

        /// <summary>
        /// Entity güncelleme
        /// </summary>
        /// <param name="entity">Entity modeli</param>
        public virtual void Update(TEntity entity)
        {
            var updatedEntity = DbContext.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            SaveChanges();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns>Değişiklik yapılan satır sayısı</returns>
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        #endregion CRUD operations

        //protected override void DisposeCore()
        //{
        //    //////_dbSet = null;
        //    //////if (DbFactory != null) DbFactory.Dispose();
        //    //////if (DbContext != null) DbContext.Dispose();
        //}
    }
}