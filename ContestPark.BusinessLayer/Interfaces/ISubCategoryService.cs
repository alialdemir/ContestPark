using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ISubCategoryService : IRepository<SubCategory>
    {
        int SubCategoryPrice(int subCategoryId);

        string SubCategoryPicture(int subCategoryId);
    }
}