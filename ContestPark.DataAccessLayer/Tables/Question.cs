using ContestPark.Entities.Enums;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Question : EntityBase
    {
        public int QuestionId { get; set; }

        //  [Required]
        //public string Questions { get; set; }
        //[Required]
        //public string Answer { get; set; }
        //[Required]
        //public string Stylish1 { get; set; }
        //[Required]
        //public string Stylish2 { get; set; }
        //[Required]
        //public string Stylish3 { get; set; }
        public string Link { get; set; }

        public AnswerTypes AnswerType { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public bool IsActive { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public virtual ICollection<AskedQuestion> AskedQuestions { get; set; } = new HashSet<AskedQuestion>();
        public virtual ICollection<QuestionLang> QuestionLangs { get; set; } = new HashSet<QuestionLang>();
        public virtual ICollection<DuelInfo> DuelInfoes { get; set; } = new HashSet<DuelInfo>();
    }
}