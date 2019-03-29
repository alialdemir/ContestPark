namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Comment : EntityBase
    {
        public int CommentId { get; set; }
        public int ParentId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}