using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfOpenSubCategoryDal : EfEntityRepositoryBase<OpenSubCategory>, IOpenSubCategoryRepository
    {
        #region Constructors

        public EfOpenSubCategoryDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcı id'ye göre ilgili alt kategori açık mı kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <returns>Açık ise true değilse false</returns>
        public bool IsSubCategoryOpen(string userId, int subCategoryId)
        {
            return DbContext
                        .Set<SubCategory>()
                        .Where(sc => DbSet.Where(osc => osc.SubCategoryId == subCategoryId && osc.UserId == userId).Any() || (sc.SubCategoryId == subCategoryId && sc.Price == 0))
                        .Any();
        }

        #endregion Methods
    }
}