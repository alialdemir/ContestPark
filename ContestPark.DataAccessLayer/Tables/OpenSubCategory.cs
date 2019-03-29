namespace ContestPark.DataAccessLayer.Tables
{
    public partial class OpenSubCategory : EntityBase
    {
        public int OpenSubCategoryId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}