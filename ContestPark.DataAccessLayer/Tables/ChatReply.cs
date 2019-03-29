namespace ContestPark.DataAccessLayer.Tables
{
    public partial class ChatReply : EntityBase
    {
        public int ChatReplyId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public bool VisibilityStatus { get; set; } = true;
        public bool ReceiverDeletingStatus { get; set; } = true;
        public bool SenderDeletingStatus { get; set; } = true;
    }
}