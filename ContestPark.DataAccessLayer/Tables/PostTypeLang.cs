namespace ContestPark.DataAccessLayer.Tables
{
    public partial class PostTypeLang : EntityBase
    {
        public int PostTypeLangId { get; set; }
        public string Description { get; set; }
        public byte LanguageId { get; set; }
        public Language Language { get; set; }
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; }
    }
}