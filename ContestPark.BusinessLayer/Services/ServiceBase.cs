using ContestPark.DataAccessLayer.Abctract;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.Entities.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.BusinessLayer.Services
{
    public class ServiceBase<TEntity> : Disposable, IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private readonly IRepository<TEntity> _repository;

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = _unitOfWork.Repository<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            Check.IsNull(entity, nameof(entity));

            _repository.Insert(entity);
            SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            Check.IsNull(entity, nameof(entity));
            _repository.Update(entity);
            SaveChanges();
        }

        public virtual void Delete(int id)
        {
            Check.IsLessThanZero(id, nameof(id));
            _repository.Delete(id);
            SaveChanges();
        }

        public int SaveChanges()
        {
            return _unitOfWork.Commit<TEntity>();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _repository.Find(predicate);
        }
    }
}