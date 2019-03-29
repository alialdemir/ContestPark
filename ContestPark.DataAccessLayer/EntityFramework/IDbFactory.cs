using Microsoft.EntityFrameworkCore;
using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IDbFactory : IDisposable
    {
        DbContext Init();
    }
}