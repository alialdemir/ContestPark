using System.Collections.Generic;

namespace ContestPark.Entities.Models
{
    public class QuestionEditModel
    {
        public string Question { get; set; }
        public string[] Answers = new string[4];
        public List<GetQuestionCategoryModel> SubCategories { get; set; }
        public int QuestionId { get; set; }
    }
}