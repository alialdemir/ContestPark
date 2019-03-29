using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfSettingDal : EfEntityRepositoryBase<Setting>, ISettingRepository
    {
        #region Constructors

        public EfSettingDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının ilgili ayardaki seçtiği değeri getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="settingTypeId">Ayar Id</param>
        /// <returns>Ayar değeri</returns>
        public string GetSettingValue(string userId, byte settingTypeId)
        {
            return DbSet
                    .Where(s => s.UserId == userId && s.SettingTypeId == settingTypeId)
                    .Select(p => p.Value)
                    .FirstOrDefault();
        }

        public bool IsAddSetting(string userId, int settingTypeId)
        {
            return DbSet
                    .Where(s => s.UserId == userId && s.SettingTypeId == settingTypeId)
                    .Any();
        }

        #endregion Methods
    }
}