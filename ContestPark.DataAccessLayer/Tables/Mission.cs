using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Mission : EntityBase
    {
        public int MissionId { get; set; }
        public int Gold { get; set; }
        public string MissionOpeningImage { get; set; }
        public string MissionCloseingImage { get; set; }
        public bool Visibility { get; set; }
        public virtual ICollection<MissionLang> MissionLangs { get; set; } = new HashSet<MissionLang>();
        public virtual ICollection<CompletedMission> CompletedMissions { get; set; } = new HashSet<CompletedMission>();
    }
}