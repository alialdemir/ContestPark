namespace ContestPark.DataAccessLayer.Tables
{
    public partial class AskedQuestion : EntityBase
    {
        public int AskedQuestionId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}