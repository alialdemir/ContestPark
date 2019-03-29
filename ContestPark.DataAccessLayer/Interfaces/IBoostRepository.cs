using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IBoostRepository : IRepository<Boost>
    {
        List<BoostModel> BoostList();

        byte FindBoostGold(int boostId);

        bool IsBoostIdControl(int boostId);
    }
}