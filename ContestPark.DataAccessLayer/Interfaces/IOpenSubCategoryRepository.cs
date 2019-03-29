using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IOpenSubCategoryRepository : IRepository<OpenSubCategory>
    {
        bool IsSubCategoryOpen(string userId, int subCategoryId);
    }
}