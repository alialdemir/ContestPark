namespace ContestPark.DataAccessLayer.Tables
{
    public partial class QuestionLang : EntityBase
    {
        public int QuestionLangId { get; set; }
        public string Questions { get; set; }
        public string Answer { get; set; }
        public string Stylish1 { get; set; }
        public string Stylish2 { get; set; }
        public string Stylish3 { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public byte LanguageId { get; set; }
        public Language Language { get; set; }
    }
}