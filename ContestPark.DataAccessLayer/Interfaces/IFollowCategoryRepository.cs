using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IFollowCategoryRepository : IRepository<FollowCategory>
    {
        int FollowersCount(int subCategoryId);//Takipçi sayısı

        bool IsFollowUpStatus(string userId, int subCategoryId);

        ServiceModel<SubCategoryModel> FollowingSubCategoryList(string userId, PagingModel pagingModel);

        ServiceModel<SubCategorySearchModel> FollowingSubCategorySearch(string userId, PagingModel pagingModel);
    }
}