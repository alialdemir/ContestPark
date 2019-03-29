using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ILanguageService : IRepository<Language>
    {
        Languages GetUserLangId(string userId);
    }
}