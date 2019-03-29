using ContestPark.DataAccessLayer.Abctract;
using ContestPark.DataAccessLayer.Interfaces.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ContestParkContext _dbContext;

        public DbFactory(ContestParkContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public DbContext Init()
        {
            return _dbContext;
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}