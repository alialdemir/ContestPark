using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IBoostService : IRepository<Boost>
    {
        List<BoostModel> BoostList();

        byte FindBoostGold(int boostId);

        bool IsBoostIdControl(int boostId);

        void BuyBoost(string userId, int boostId);
    }
}