namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Like : EntityBase
    {
        public int LikeId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}