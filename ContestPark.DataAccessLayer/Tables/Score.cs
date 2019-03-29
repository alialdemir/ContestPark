using System;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Score : EntityBase
    {
        public int ScoreId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public int DuelInfoId { get; set; }
        public DuelInfo DuelInfo { get; set; }
        public byte Point { get; set; }
        public DateTime ScoreDate { get; set; }
    }
}