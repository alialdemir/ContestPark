using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ICpInfoService : IRepository<CpInfo>
    {
        DateTime LastDailyChipDateTime(string userId);
    }
}