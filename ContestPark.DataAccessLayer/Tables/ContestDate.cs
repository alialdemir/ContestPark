using System;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class ContestDate : EntityBase
    {
        public int ContestDateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}