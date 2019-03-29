namespace ContestPark.DataAccessLayer.Tables
{
    public partial class ChatBlock : EntityBase
    {
        public int ChatBlockId { get; set; }
        public string WhoId { get; set; }
        public User WhonUser { get; set; }
        public string WhonId { get; set; }
        public User WhoUser { get; set; }
    }
}