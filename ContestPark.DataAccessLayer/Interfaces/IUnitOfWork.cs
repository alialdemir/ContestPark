using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity, new();

        int Commit<TEntity>() where TEntity : class, IEntity, new();
    }
}