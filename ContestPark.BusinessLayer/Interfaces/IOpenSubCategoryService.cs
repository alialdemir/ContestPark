using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IOpenSubCategoryService : IRepository<OpenSubCategory>
    {
        bool OpenCategory(string userId, int subCategoryId);

        bool IsSubCategoryOpen(string userId, int subCategoryId);
    }
}