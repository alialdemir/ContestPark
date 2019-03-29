using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IFollowCategoryService : IRepository<FollowCategory>
    {
        void Delete(string userId, int subCategoryId);

        int FollowersCount(int subCategoryId);//Takipçi sayısı

        bool IsFollowUpStatus(string userId, int subCategoryId);

        ServiceModel<SubCategoryModel> FollowingSubCategoryList(string userId, PagingModel pagingModel);

        ServiceModel<SubCategorySearchModel> FollowingSubCategorySearch(string userId, PagingModel pagingModel);
    }
}