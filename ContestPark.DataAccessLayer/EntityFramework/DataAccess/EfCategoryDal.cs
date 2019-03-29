using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category>, ICategoryRepository
    {
        #region Constructors

        public EfCategoryDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        private List<SubCategoryModel> d(string userId, byte langId, int CategoryId)
        {
            string sqlQuery = @"select
 sc.SubCategoryId,
sc.Price,
 (case (SELECT
    (CASE
        WHEN EXISTS(
            SELECT NULL AS [EMPTY]
            FROM OpenSubCategories AS osc  where osc.UserId ='" + userId + @"' and osc.SubCategoryId = sc.SubCategoryId
            ) THEN 1
        ELSE 0
     END) )
  when 1 then 0
  else sc.Price
end) as DisplayName,
 (case
  when sc.Price = 0 then sc.PictuePath
  when (SELECT
    (CASE
        WHEN EXISTS(
            SELECT NULL AS [EMPTY]
            FROM OpenSubCategories AS osc  where osc.UserId ='" + userId + @"' and osc.SubCategoryId = sc.SubCategoryId
            ) THEN 1
        ELSE 0
     END) ) = 1 then sc.PictuePath
	 else '" + DefaultImages.DefaultLock + @"'
end) as PicturePath,
scl.SubCategoryName from SubCategories sc
INNER JOIN SubCategoryLangs scl on scl.SubCategoryId = sc.SubCategoryId and scl.LanguageId = " + langId + @"
where sc.Visibility=1 and sc.CategoryId =" + CategoryId + @"
order by DisplayName";

            List<SubCategoryModel> groups = new List<SubCategoryModel>();
            var conn = DbContext.Database.GetDbConnection();
            try
            {
                using (var command = conn.CreateCommand())
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                    command.CommandText = sqlQuery;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.ReadAsync().Result)
                        {
                            var row = new SubCategoryModel
                            {
                                SubCategoryId = reader.GetInt32(0),
                                Price = reader.GetInt32(1),
                                DisplayPrice = reader.GetInt32(2).ToString(),
                                PicturePath = reader.GetString(3),
                                SubCategoryName = reader.GetString(4)
                            };
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                //    conn.Close();
            }

            return groups;
        }

        /// <summary>
        /// Kategorilerin listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kategori listesi</returns>
        public ServiceModel<CategoryModel> CategoryList(string userId, PagingModel pagingModel)//Yarışma kategorilerini listeler
        {
            //var result = (from cc in DbSet//.AsEnumerable()
            //        where cc.Visibility
            //        join ccl in DbContext.Set<CategoryLang>() on cc.CategoryId equals ccl.CategoryId
            //        where ccl.LanguageId == (byte)languages
            //              orderby cc.Order ascending
            //        select new CategoryModel()
            //        {
            //            CategoryName = ccl.CategoryName,
            //            CategoryId = ccl.CategoryId,
            //         //   SubCategories = DbContext.Set<SubCategory>().FromSql(sqlQuery).AsParallel<SubCategoryModel>()
            //        }).ToServiceModel(pagingModel);
            //byte langId = (byte)languages;

            //List<CategoryModel> categories = new List<CategoryModel>();
            //foreach (var item in result.Items)
            //{
            //    var x =  d(userId, langId, item.CategoryId);
            //    CategoryModel row = new CategoryModel()
            //    {
            //        CategoryId = item.CategoryId,
            //        CategoryName = item.CategoryName,
            //        SubCategories = x
            //    };
            //    categories.Add(row);
            //}
            //result.Items = categories;
            //return result;
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from cc in DbSet
                    join sc in DbContext.Set<SubCategory>() on cc.CategoryId equals sc.CategoryId
                    where cc.Visibility && sc.Visibility
                    let langId = (byte)language
                    orderby cc.Order ascending
                    select new CategoryModel
                    {
                        CategoryName = cc.CategoryLangs.Where(p => p.LanguageId == langId).Select(p => p.Name).FirstOrDefault(),
                        CategoryId = cc.CategoryId,
                        SubCategories = cc.SubCategories
                                        .Where(sc => sc.Visibility)
                                        .OrderBy(sc => sc.Order)
                                        .Select(sc => new SubCategoryModel
                                        {
                                            SubCategoryId = sc.SubCategoryId,
                                            PicturePath = sc.PictuePath,
                                            Price = sc.OpenSubCategoris.Where(p => p.UserId == userId).Any() ? 0 : sc.Price,
                                            SubCategoryName = sc.SubCategoryLangs.Where(p => p.LanguageId == langId).Select(p => p.SubCategoryName).FirstOrDefault()
                                        })
                                        .OrderBy(sc => sc.Price)
                                        .ToList()
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Alt kategori Id'ye göre kategori listesi getirir 0 ise tüm kategoriler gelir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Aranan kategorilerin listesi</returns>
        public ServiceModel<SubCategorySearchModel> SearchCategory(string userId, int subCategoryId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from sc in DbContext.Set<SubCategory>()
                    orderby sc.Price
                    let langId = (byte)language
                    join cc in DbContext.Set<Category>() on sc.CategoryId equals cc.CategoryId
                    join ccl in DbContext.Set<CategoryLang>() on sc.CategoryId equals ccl.CategoryId
                    join scl in DbContext.Set<SubCategoryLang>() on sc.SubCategoryId equals scl.SubCategoryId
                    where ((sc.Visibility) && (cc.Visibility)) && (cc.CategoryId != 0) && ccl.LanguageId == langId && scl.LanguageId == langId
                    let osc = DbContext.Set<OpenSubCategory>().Where(p => p.UserId == userId && p.SubCategoryId == sc.SubCategoryId).Any()
                    orderby sc.Order descending
                    select new SubCategorySearchModel
                    {
                        CategoryName = ccl.Name,
                        SubCategoryId = sc.SubCategoryId,
                        Price = osc ? 0 : sc.Price,
                        PicturePath = sc.PictuePath,
                        SubCategoryName = scl.SubCategoryName,
                    })
                    .OrderBy(sc => sc.Price)
                    .ToServiceModel(pagingModel);
        }

        #endregion Methods
    }
}