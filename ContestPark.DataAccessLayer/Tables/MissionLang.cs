namespace ContestPark.DataAccessLayer.Tables
{
    public partial class MissionLang : EntityBase
    {
        public int MissionLangId { get; set; }
        public string MissionName { get; set; }
        public string MissionDescription { get; set; }
        public Language Language { get; set; }
        public byte LanguageId { get; set; }
        public Mission Task { get; set; }
        public int MissionId { get; set; }
    }
}