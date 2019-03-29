using ContestPark.Entities.Enums;

namespace ContestPark.Entities.Models
{
    public class TrueAnswerControlModel
    {
        public bool IsTrueAnswer { get; set; }
        public byte ScorePoint { get; set; }
        public Stylish TrueAnswer { get; set; }
    }
}