namespace ContestPark.Entities.Models
{
    public class QuestionUpdateModel
    {
        public string[] Answers = new string[4];
        public int QuestionId { get; set; }
        public int SubCategoryId { get; set; }//Burası enumdan gelse iyi olurdu...
    }
}