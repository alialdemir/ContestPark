using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Category : EntityBase
    {
        public int CategoryId { get; set; }
        public string PicturePath { get; set; }
        public bool Visibility { get; set; }
        public byte Order { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
        public virtual ICollection<CategoryLang> CategoryLangs { get; set; } = new HashSet<CategoryLang>();
    }
}