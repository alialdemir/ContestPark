using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IDuelRepository : IRepository<Duel>
    {
        int GameCount(string userId, int subCategoryId);

        bool IsFounder(string userId, int duelId);

        DuelUserInfoModel DuelUserInfo(string userId, int duelId);
    }
}