namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Boost : EntityBase
    {
        public int BoostId { get; set; }
        public string Name { get; set; }
        public byte Gold { get; set; }
        public string PicturePath { get; set; }
        public bool Visibility { get; set; }
    }
}