using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ISubCategoryLangRepository : IRepository<SubCategoryLang>
    {
        string SubCategoryNameByLanguage(int subcategoryId);
    }
}