using ContestPark.Entities.Enums;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class CpInfo : EntityBase
    {
        public int CpInfoId { get; set; }
        public int CpId { get; set; }
        public Cp Cp { get; set; }
        public int CpSpent { get; set; }
        public ChipProcessNames ChipProcessName { get; set; }
    }
}