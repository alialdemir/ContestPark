namespace ContestPark.DataAccessLayer.Tables
{
    public partial class SubCategoryLang : EntityBase
    {
        public int SubCategoryLangId { get; set; }
        public string SubCategoryName { get; set; }
        public byte LanguageId { get; set; }
        public Language Language { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}