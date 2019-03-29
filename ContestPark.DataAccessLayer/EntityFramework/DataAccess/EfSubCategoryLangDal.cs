using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Extensions;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfSubCategoryLangDal : EfEntityRepositoryBase<SubCategoryLang>, ISubCategoryLangRepository
    {
        #region Constructors

        public EfSubCategoryLangDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının diline göre alt kategori adını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subcategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori adı</returns>
        public string SubCategoryNameByLanguage(int subcategoryId)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return DbSet
                    .Where(p => p.SubCategoryId == subcategoryId && p.LanguageId == (byte)language)
                    .Select(p => p.SubCategoryName)
                    .FirstOrDefault();
        }

        #endregion Methods
    }
}