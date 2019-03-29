using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfFollowCategoryDal : EfEntityRepositoryBase<FollowCategory>, IFollowCategoryRepository
    {
        #region Constructors

        public EfFollowCategoryDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// SubCategoryId'ye göre o kategoriyi takip eden kullanıcı sayısı
        /// </summary>
        /// <param name="subCategoryId">SubCategoryId</param>
        /// <returns>Kategoriyi takip eden kullanıcı sayısı</returns>
        public int FollowersCount(int subCategoryId)
        {
            return DbSet
                        .Where(p => p.SubCategoryId == subCategoryId)
                        .Count();
        }

        /// <summary>
        /// Kullanıcının takip ettiği kategoriler
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Kullanıcının takip ettiği kategoriler</returns>
        public ServiceModel<SubCategoryModel> FollowingSubCategoryList(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from fc in DbSet
                    where fc.UserId == userId
                    join sc in DbContext.Set<SubCategory>() on fc.SubCategoryId equals sc.SubCategoryId
                    where sc.Visibility && sc.Category.Visibility
                    orderby fc.FollowCategoryId descending
                    select new SubCategoryModel
                    {
                        PicturePath = sc.PictuePath,
                        SubCategoryId = fc.SubCategoryId,
                        SubCategoryName = sc.SubCategoryLangs.Where(scl => scl.LanguageId == (byte)language).Select(scl => scl.SubCategoryName).FirstOrDefault(),
                        DisplayPrice = "0"
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Kullanıcının kategoriyi takip etme durumu
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategoriyi ise takip ediyor true etmiyorsa ise false</returns>
        public bool IsFollowUpStatus(string userId, int subCategoryId)
        {
            return DbSet
                        .Where(p => p.UserId == userId && p.SubCategoryId == subCategoryId)
                        .Any();
        }

        /// <summary>
        /// Takip ettiği kategorileri search sayfasında listeleme
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>search kategori listesi</returns>
        public ServiceModel<SubCategorySearchModel> FollowingSubCategorySearch(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from fc in DbSet
                    where fc.UserId == userId
                    join sc in DbContext.Set<SubCategory>() on fc.SubCategoryId equals sc.SubCategoryId
                    where sc.Visibility && sc.Category.Visibility
                    orderby fc.FollowCategoryId descending
                    let langId = (byte)language
                    select new SubCategorySearchModel
                    {
                        PicturePath = sc.PictuePath,
                        SubCategoryId = fc.SubCategoryId,
                        SubCategoryName = sc.SubCategoryLangs.Where(scl => scl.LanguageId == langId).Select(scl => scl.SubCategoryName).FirstOrDefault(),
                        DisplayPrice = "0",
                        CategoryName = sc.Category.CategoryLangs.Where(p => p.LanguageId == langId).Select(p => p.Name).FirstOrDefault()
                    }).ToServiceModel(pagingModel);
        }

        #endregion Methods
    }
}