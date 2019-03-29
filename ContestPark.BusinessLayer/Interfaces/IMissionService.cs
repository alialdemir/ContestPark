using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IMissionService : IRepository<Mission>
    {
        void MissionComplete(string userId, params Missions[] missions);

        MissionListModel MissionList(string userId, PagingModel pagingModel);

        void TakesMissionGold(string userId, Missions mission);

        int MissionGold(Missions mission);
    }
}