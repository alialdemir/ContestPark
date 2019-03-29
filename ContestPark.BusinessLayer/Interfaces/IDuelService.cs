using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IDuelService : IRepository<Duel>
    {
        int GameCount(string userId, int subCategoryId);

        bool IsFounder(string userId, int duelId);

        DuelUserInfoModel DuelUserInfo(string userId, int duelId);
    }
}