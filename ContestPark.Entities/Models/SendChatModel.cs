namespace ContestPark.Entities.Models
{
    public class SendChatModel
    {
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public string PublicKey { get; set; }
    }
}