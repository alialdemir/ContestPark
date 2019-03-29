using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ILanguageRepository : IRepository<Language>
    {
        byte GetUserLangId(string userId);
    }
}