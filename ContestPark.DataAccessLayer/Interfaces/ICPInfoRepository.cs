using ContestPark.DataAccessLayer.Tables;
using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ICpInfoRepository : IRepository<CpInfo>
    {
        DateTime LastDailyChipDateTime(string userId);
    }
}