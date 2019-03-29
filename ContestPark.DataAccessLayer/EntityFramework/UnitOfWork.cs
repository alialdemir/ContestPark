using ContestPark.DataAccessLayer.Abctract;
using ContestPark.DataAccessLayer.Interfaces;
using System;
using System.Collections;

namespace ContestPark.DataAccessLayer
{
    public class UnitOfWork : Disposable, IUnitOfWork
    {
        #region Private Variables

        private readonly IDbFactory _dbFactory;
        private Hashtable _repositories;

        #endregion Private Variables

        #region Constructors

        public UnitOfWork(IDbFactory context)
        {
            _dbFactory = context;
        }

        #endregion Constructors

        #region Methods

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity, new()
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(IRepository<>);
            var repository = new EfEntityRepositoryBase<TEntity>(_dbFactory);

            _repositories.Add(type, repository);

            return (IRepository<TEntity>)_repositories[type];
        }

        public int Commit<TEntity>() where TEntity : class, IEntity, new()
        {
            return 1;
            //return Repository<TEntity>().SaveChanges();
        }

        #endregion Methods

        #region Dispose

        protected override void DisposeCore()
        {
            if (_dbFactory != null) _dbFactory.Dispose();
            foreach (IDisposable repository in _repositories.Values)
            {
                repository.Dispose();
            }
            base.DisposeCore();
        }

        #endregion Dispose
    }
}