using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfLanguageDal : EfEntityRepositoryBase<Language>, ILanguageRepository
    {
        #region Constructors

        public EfLanguageDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcı id'sine ait dil id'sini döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Dil id</returns>
        public byte GetUserLangId(string userId)
        {
            return (from s in DbContext.Set<Setting>()
                    where s.UserId == userId && s.SettingTypeId == (byte)SettingTypes.Language
                    join l in DbContext.Set<Language>() on s.Value equals l.ShortName
                    select l.LanguageId)
                    .FirstOrDefault();
        }

        #endregion Methods
    }
}