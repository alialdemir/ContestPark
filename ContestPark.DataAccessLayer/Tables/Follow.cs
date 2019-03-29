namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Follow : EntityBase
    {
        public int FollowId { get; set; }
        public string FollowedUserId { get; set; }//Takip edilen
        public User FollowedUser { get; set; }
        public string FollowUpUserId { get; set; }//Takip eden
        public User FollowUpUser { get; set; }
    }
}