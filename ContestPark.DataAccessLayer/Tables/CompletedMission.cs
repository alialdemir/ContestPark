namespace ContestPark.DataAccessLayer.Tables
{
    public partial class CompletedMission : EntityBase
    {
        public int CompletedMissionId { get; set; }
        public int MissionId { get; set; }
        public Mission Mission { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool MissionComplate { get; set; }
    }
}