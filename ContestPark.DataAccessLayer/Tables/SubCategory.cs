using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class SubCategory : EntityBase
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string PictuePath { get; set; }
        public bool Visibility { get; set; }
        public byte Order { get; set; }
        public int Price { get; set; }
        public virtual ICollection<Question> QuizQuestions { get; set; } = new HashSet<Question>();
        public virtual ICollection<Score> Scores { get; set; } = new HashSet<Score>();
        public virtual ICollection<SubCategoryLang> SubCategoryLangs { get; set; } = new HashSet<SubCategoryLang>();
        public virtual ICollection<OpenSubCategory> OpenSubCategoris { get; set; } = new HashSet<OpenSubCategory>();
        public virtual ICollection<FollowCategory> FollowCategories { get; set; } = new HashSet<FollowCategory>();
        public virtual ICollection<AskedQuestion> AskedQuestions { get; set; } = new HashSet<AskedQuestion>();
    }
}