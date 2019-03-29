using ContestPark.Entities.Enums;

namespace ContestPark.Entities.Models
{
    public class DuelQuestionModel : BaseModel
    {
        public string Question { get; set; }
        public string[] Answers = new string[4];
        public Stylish? TrueAnswer { get; set; }
        public Stylish? FounderUserAnswer { get; set; }//Duelloyu başlatanın verdiği cevap
        public byte FounderTime { get; set; }//Kurucunun cevap verdiği süre
        public Stylish? CompetitorUserAnswer { get; set; }//Rakibin verdiği cevap
        public byte CompetitorTime { get; set; }//Rakibin cevap verdiği süre
        public string Link { get; set; }//Müzik yarışması için müzik linki
        public QuestionTypes QuestionType { get; set; }
        public AnswerTypes AnswerType { get; set; }
    }
}