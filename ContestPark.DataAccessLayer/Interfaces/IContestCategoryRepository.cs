using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        ServiceModel<CategoryModel> CategoryList(string userId, PagingModel pagingModel);

        ServiceModel<SubCategorySearchModel> SearchCategory(string userId, int CategoryId, PagingModel pagingModel);
    }
}