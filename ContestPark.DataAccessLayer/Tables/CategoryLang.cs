namespace ContestPark.DataAccessLayer.Tables
{
    public partial class CategoryLang : EntityBase
    {
        public int CategoryLangId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public byte LanguageId { get; set; }
        public Language Language { get; set; }
    }
}