using ContestPark.Entities.Enums;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class DuelInfo : EntityBase
    {
        public int DuelInfoId { get; set; }
        public int DuelId { get; set; }
        public Duel Duel { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public Stylish FounderUserAnswer { get; set; }//Duelloyu başlatanın verdiği cevap
        public Stylish CompetitorUserAnswer { get; set; }//Rakibin verdiği cevap
        public Stylish TrueAnswer { get; set; }
        public byte FounderTime { get; set; }//Kurucunun cevap verdiği süre
        public byte CompetitorTime { get; set; }//Rakibin cevap verdiği süre
        public virtual ICollection<Score> Scores { get; set; } = new HashSet<Score>();
    }
}