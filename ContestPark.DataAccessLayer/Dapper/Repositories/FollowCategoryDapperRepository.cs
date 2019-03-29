using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class FollowCategoryDapperRepository : DapperRepositoryBase<FollowCategory>, IFollowCategoryRepository
    {
        #region Constructors

        public FollowCategoryDapperRepository(IConfiguration configuration) : base(configuration)
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
            string sql = "select count([fc].[SubCategoryId]) from [FollowCategories] as [fc] where [fc].[SubCategoryId]=@SubcategoryId";
            return Connection.Query<int>(sql, new { SubCategoryId = subCategoryId }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının takip ettiği kategoriler
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Kullanıcının takip ettiği kategoriler</returns>
        public ServiceModel<SubCategoryModel> FollowingSubCategoryList(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string sql = @"select
                           [sc].[PictuePath] as [PicturePath],
                           [fc].[SubCategoryId] as [SubCategoryId],
                           (select top(1) [scl].[SubCategoryName] from [SubCategoryLangs] as [scl] where [scl].[LanguageId]=@LanguageId and [scl].[SubCategoryId]=[fc].[SubCategoryId]) as [SubCategoryName]
                           from [FollowCategories] as [fc]
                           INNER JOIN  [SubCategories] as [sc] on [fc].[SubCategoryId]=[sc].[SubCategoryId]
                           INNER JOIN [Categories] as [c] on [sc].[CategoryId]=[c].[CategoryId]
                           where [fc].[UserId]=@UserId and [sc].[Visibility]=1 and [c].[Visibility]=1
                           order by [fc].[FollowCategoryId] desc";
            return Connection.QueryPaging<SubCategoryModel>(sql, new { UserId = userId, LanguageId = (byte)language }, pagingModel);
        }

        /// <summary>
        /// Kullanıcının kategoriyi takip etme durumu
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategoriyi ise takip ediyor true etmiyorsa ise false</returns>
        public bool IsFollowUpStatus(string userId, int subCategoryId)
        {
            string sql = @"SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS [EMPTY] FROM [FollowCategories] as [fc]
                           where [fc].[UserId]=@UserId and [fc].[SubCategoryId]=@SubCategoryId
                           ) THEN 1 ELSE 0 END) AS[value]";
            return Connection.Query<bool>(sql, new { UserId = userId, SubCategoryId = subCategoryId }).FirstOrDefault();
        }

        /// <summary>
        /// Takip ettiği kategorileri search sayfasında listeleme
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>search kategori listesi</returns>
        public ServiceModel<SubCategorySearchModel> FollowingSubCategorySearch(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string sql = @"select
                           [sc].[PictuePath] as [PicturePath],
                           [fc].[SubCategoryId] as [SubCategoryId],
                           (select top(1) [scl].[SubCategoryName] from [SubCategoryLangs] as [scl] where [scl].[LanguageId]=@LanguageId and [scl].[SubCategoryId]=[fc].[SubCategoryId]) as [SubCategoryName],
                           (select top(1) [cl].[Name] from [CategoryLangs]  AS [cl] where [cl].[LanguageId]=@LanguageId and [sc].[CategoryId]=[cl].[CategoryId]) as [CategoryName]
                           from [FollowCategories] as [fc]
                           INNER JOIN  [SubCategories] as [sc] on [fc].[SubCategoryId]=[sc].[SubCategoryId]
                           INNER JOIN [Categories] as [c] on [sc].[CategoryId]=[c].[CategoryId]
                           where [fc].[UserId]=@UserId and [sc].[Visibility]=1 and [c].[Visibility]=1
                           order by [fc].[FollowCategoryId] desc";
            return Connection.QueryPaging<SubCategorySearchModel>(sql, new { UserId = userId, LanguageId = (byte)language }, pagingModel);
        }

        #endregion Methods
    }
}