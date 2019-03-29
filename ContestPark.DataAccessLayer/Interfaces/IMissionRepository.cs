using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IMissionRepository : IRepository<Mission>
    {
        MissionListModel MissionList(string userId, PagingModel pagingModel);

        int MissionGold(Entities.Enums.Missions mission);
    }
}