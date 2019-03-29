namespace ContestPark.DataAccessLayer.Tables
{
    public partial class SupportTypeLang : EntityBase
    {
        public int SupportTypeLangId { get; set; }
        public string Description { get; set; }
        public byte LanguageId { get; set; }
        public Language Language { get; set; }
        public byte SupportTypeId { get; set; }
        public SupportType SupportType { get; set; }
    }
}