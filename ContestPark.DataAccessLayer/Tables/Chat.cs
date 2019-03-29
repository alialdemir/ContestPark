using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Chat : EntityBase
    {
        public int ChatId { get; set; }
        public string ReceiverId { get; set; }//alıcı
        public User ReceiverUser { get; set; }
        public string SenderId { get; set; }//gönderen
        public User SenderUser { get; set; }
        public virtual ICollection<ChatReply> ChatReplys { get; set; } = new HashSet<ChatReply>();
    }
}