namespace ContestPark.DataAccessLayer.Missions
{
    public interface IMissionCreator
    {
        IMission[] MissionFactory(params Entities.Enums.Missions[] missions);
    }
}