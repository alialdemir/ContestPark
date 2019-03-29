using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ICompletedMissionService : IRepository<CompletedMission>
    {
        bool MissionStatus(string userId, Entities.Enums.Missions mission);

        CompletedMission UserMission(string userId, Entities.Enums.Missions mission);
    }
}