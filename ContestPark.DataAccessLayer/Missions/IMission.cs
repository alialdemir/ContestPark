namespace ContestPark.DataAccessLayer.Missions
{
    public interface IMission// : IEntityRepository<Mission>
    {
        Entities.Enums.Missions Mission { get; }

        bool MissionComplete(string userId);
    }
}