using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        int SubCategoryPrice(int subCategoryId);

        string SubCategoryPicture(int subCategoryId);
    }
}