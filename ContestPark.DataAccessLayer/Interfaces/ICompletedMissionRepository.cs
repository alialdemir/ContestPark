using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ICompletedMissionRepository : IRepository<CompletedMission>
    {
        bool MissionStatus(string userId, Entities.Enums.Missions task);

        CompletedMission UserMission(string userId, Entities.Enums.Missions mission);
    }
}