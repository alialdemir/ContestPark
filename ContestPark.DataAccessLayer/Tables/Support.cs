namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Support : EntityBase
    {
        public int SupportId { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public byte SupportTypeId { get; set; }
        public SupportType SupportType { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}