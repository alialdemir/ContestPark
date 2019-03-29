using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ICategoryService : IRepository<Category>
    {
        ServiceModel<CategoryModel> CategoryList(string userId, PagingModel pagingModel);

        ServiceModel<SubCategorySearchModel> SearchCategory(string userId, int subCategoryId, PagingModel pagingModel);
    }
}