using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ISubCategoryLangService : IRepository<SubCategoryLang>
    {
        string SubCategoryNameByLanguage(string userId, int subcategoryId);
    }
}