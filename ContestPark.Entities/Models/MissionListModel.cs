namespace ContestPark.Entities.Models
{
    public class MissionListModel : ServiceModel<MissionModel>
    {
        public byte CompleteMissionCount { get; set; }
    }
}