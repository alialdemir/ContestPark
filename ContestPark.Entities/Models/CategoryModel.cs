using System.Collections.Generic;

namespace ContestPark.Entities.Models
{
    public class CategoryModel : BaseModel
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<SubCategoryModel> SubCategories { get; set; } = new List<SubCategoryModel>();
    }
}