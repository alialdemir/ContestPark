namespace ContestPark.DataAccessLayer.Tables
{
    public partial class FollowCategory : EntityBase
    {
        public int FollowCategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}