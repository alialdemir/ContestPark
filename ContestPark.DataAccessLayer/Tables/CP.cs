using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Cp : EntityBase
    {
        public int CpId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CpAmount { get; set; }
        public virtual ICollection<CpInfo> CpInfos { get; set; } = new HashSet<CpInfo>();
    }
}