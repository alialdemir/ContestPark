using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class CategoryDapperRepository : DapperRepositoryBase<Category>, ICategoryRepository
    {
        #region Constructors

        public CategoryDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kategorilerin listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kategori listesi</returns>
        public ServiceModel<CategoryModel> CategoryList(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string sql = @"SELECT ( SELECT TOP(1) [p].[Name] FROM [CategoryLangs] AS [p] WHERE ([p].[LanguageId] = @LangId) AND ([sc].[CategoryId] = [p].[CategoryId]) ) AS [Name],
	                       [sc].[CategoryId] as [CategoryId],
                           [sc].[SubCategoryId],
                           (SELECT TOP(1) [scl].[SubCategoryName] FROM [SubCategoryLangs] AS [scl] WHERE ([scl].[LanguageId] = @LangId) AND ([sc].[SubCategoryId] = [scl].[SubCategoryId])) AS [SubCategoryName],
                           (case (SELECT
                           (CASE
                           WHEN EXISTS(
                           SELECT NULL AS [EMPTY]
                           FROM OpenSubCategories AS osc  where osc.UserId =@UserId and osc.SubCategoryId = sc.SubCategoryId
                           ) THEN 1
                           ELSE 0
                           END) )
                           when 1 then 0
                           else sc.Price
                           end) as Price,
                           (case
                           when [sc].[Price] = 0 then [sc].[PictuePath]
                           when (SELECT
                           (CASE
                           WHEN EXISTS(
                           SELECT NULL AS [EMPTY]
                           FROM [OpenSubCategories] AS [osc]  where [osc].[UserId] =@UserId and [osc].[SubCategoryId] = [sc].[SubCategoryId]
                           ) THEN 1
                           ELSE 0
                           END) ) = 1 then [sc].[PictuePath]
                           else @PicturePath
                           end) as PicturePath
                           FROM [Categories] AS [cc]
                           INNER JOIN [SubCategories] AS [sc] ON [cc].[CategoryId] = [sc].[CategoryId]
                           WHERE ([cc].[Visibility] = 1) AND ([sc].[Visibility] = 1)
                           ORDER BY [cc].[Order]",
                           offset = "OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY";
            var param = new
            {
                UserId = userId,
                LangId = (int)language,
                PicturePath = DefaultImages.DefaultLock,
                PageNumber = QueryableExtension.PageNumberCalculate(pagingModel),
                PageSize = pagingModel.PageSize
            };
            var lookup = new Dictionary<int, CategoryModel>();
            var data = Connection.Query<CategoryModel, SubCategoryModel, CategoryModel>(sql + offset, (category, subCategory) =>
            {
                if (!lookup.TryGetValue(category.CategoryId, out CategoryModel invoiceEntry))
                {
                    invoiceEntry = category;
                    if (invoiceEntry.SubCategories == null) invoiceEntry.SubCategories = new List<SubCategoryModel>();
                    lookup.Add(invoiceEntry.CategoryId, invoiceEntry);
                }
                invoiceEntry.SubCategories.Add(subCategory);
                return invoiceEntry;
            }, param, splitOn: "ContestCategoryId,SubCategoryId").ToList();
            return new ServiceModel<CategoryModel>
            {
                Items = lookup.Values.Distinct(),
                PageNumber = pagingModel.PageNumber,
                PageSize = pagingModel.PageSize,
                Count = Connection.Query<int>($"SELECT COUNT(*) FROM ({ "SELECT TOP (100) PERCENT " + sql.Substring(6)}) AS c;", param).First()
            };
        }

        /// <summary>
        /// Alt kategori Id'ye göre kategori listesi getirir 0 ise tüm kategoriler gelir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Aranan kategorilerin listesi</returns>
        public ServiceModel<SubCategorySearchModel> SearchCategory(string userId, int categoryId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string sql = @"select  * from (
                           select TOP (100) PERCENT
                           [sc].[SubCategoryId],
                           (SELECT TOP(1) [p].[Name] FROM [CategoryLangs] AS [p] WHERE ([p].[LanguageId] = @LangId) AND ([sc].[CategoryId] = [p].[CategoryId]) ) AS [Name],
                           (SELECT TOP(1) [scl].[SubCategoryName] FROM [SubCategoryLangs] AS [scl] WHERE ([scl].[LanguageId] = @LangId) AND ([sc].[SubCategoryId] = [scl].[SubCategoryId])) AS [SubCategoryName],
                           (case (SELECT
                           (CASE
                           WHEN EXISTS(
                           SELECT NULL AS [EMPTY]
                           FROM OpenSubCategories AS osc  where osc.UserId =@UserId and osc.SubCategoryId = sc.SubCategoryId
                           ) THEN 1
                           ELSE 0
                           END) )
                           when 1 then 0
                           else sc.Price
                           end) as Price,
                           (case
                           when [sc].[Price] = 0 then [sc].[PictuePath]
                           when (SELECT
                           (CASE
                           WHEN EXISTS(
                           SELECT NULL AS [EMPTY]
                           FROM [OpenSubCategories] AS [osc]  where [osc].[UserId] =@UserId and [osc].[SubCategoryId] = [sc].[SubCategoryId]
                           ) THEN 1
                           ELSE 0
                           END) ) = 1 then [sc].[PictuePath]
                           else @PicturePath
                           end) as PicturePath
                           from [SubCategories] as [sc]
                           INNER JOIN [Categories] as [cc] on [cc].[CategoryId]=[sc].[CategoryId]
                           where (([sc].[Visibility]=1 and [cc].[Visibility]=1) and [cc].[CategoryId]!=0)
                           order by [sc].[Price] desc
                           ) as d
                           order by d.Price asc";
            return Connection.QueryPaging<SubCategorySearchModel>(sql, new { UserId = userId, LangId = (int)language, PicturePath = DefaultImages.DefaultLock }, pagingModel);
        }

        #endregion Methods
    }
}