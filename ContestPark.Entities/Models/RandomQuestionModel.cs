using ContestPark.Entities.Enums;

namespace ContestPark.Entities.Models
{
    public class RandomQuestionModel : BaseModel
    {
        public string Question { get; set; }
        public string[] Answers = new string[4];
        public int QuestionId { get; set; }
        public int DuelInfoId { get; set; }
        public string Link { get; set; }//Music yarışması için şarkının 30 saniyelik linki
        public byte QuestionType { get; set; }
        public byte AnswerType { get; set; }
        public Stylish? TrueAnswer { get; set; }
    }
}