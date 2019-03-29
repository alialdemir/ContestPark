using ContestPark.Entities.Enums;

namespace ContestPark.Entities.Models
{
    public class NextDuelStartModel
    {
        public int DuelId { get; set; }
        public string UserId { get; set; }
        public Stylish Answer { get; set; }
        public byte Time { get; set; }
    }
}