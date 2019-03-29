using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Duel : EntityBase
    {
        public int DuelId { get; set; }
        public string FounderUserId { get; set; }
        public User FounderUser { get; set; }
        public string CompetitorUserId { get; set; }
        public User CompetitorUser { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public int Cp { get; set; }
        public virtual ICollection<DuelInfo> DuelInfos { get; set; } = new HashSet<DuelInfo>();
    }
}