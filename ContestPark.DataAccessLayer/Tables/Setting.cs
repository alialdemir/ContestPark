namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Setting : EntityBase
    {
        public int SettingId { get; set; }
        public int SettingTypeId { get; set; }
        public SettingType SettingType { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
    }
}