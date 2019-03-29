using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfSubCategoryDal : EfEntityRepositoryBase<SubCategory>, ISubCategoryRepository
    {
        #region Constructors

        public EfSubCategoryDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// SubCategoryId ye göre alt kategori resmi verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Picture Path</returns>
        public string SubCategoryPicture(int subCategoryId)
        {
            return DbSet
                    .Where(p => p.SubCategoryId == subCategoryId)
                    .Select(p => p.PictuePath)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Alt kategorinin fiyatını verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori fiyatı</returns>
        public int SubCategoryPrice(int subCategoryId)
        {
            return DbSet
                    .Where(p => p.SubCategoryId == subCategoryId)
                    .Select(p => p.Price)
                    .FirstOrDefault();
        }

        #endregion Methods
    }
}