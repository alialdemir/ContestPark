using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class FollowDapperRepository : DapperRepositoryBase<Follow>, IFollowRepository
    {
        #region Constructor

        public FollowDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        //string followUpUserId = takip eden , string followedUserId  = takip edilen
        /// <summary>
        /// İki kullanıcı arasındaki takip etme durumunu kaldırır
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        public void Delete(string followUpUserId, string followedUserId)
        {
            string sql = "delete from Follows where FollowUpUserId=@FollowUpUserId and FollowedUserId=@FollowedUserId";
            int executeRowCount = Connection.Execute(sql, new { FollowUpUserId = followUpUserId, FollowedUserId = followedUserId });
            if (executeRowCount < 0) Check.BadStatus("ServerMessages_thisUserFollowing");
        }

        /// summary>
        /// Parametreden gelen kullanıcı id'ye ait takip eden kullanıcı sayısını dönrürür
        /// </summary>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <returns>Takip eden kullanıcı sayısı</returns>
        public int FollowersCount(string followedUserId)
        {
            string sql = @"SELECT COUNT([f].[FollowId]) FROM [Follows] as [f]
                           where [f].[FollowedUserId]=@FollowedUserId";
            return Connection.Query<int>(sql, new { FollowedUserId = followedUserId }).FirstOrDefault();
        }

        /// <summary>
        /// Parametreden gelen kullanıcı id'ye ait takip ettiği kullanıcıların sayısını döndürür
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <returns>Takip ettiği kullanıcı sayısı</returns>
        public int FollowUpCount(string followUpUserId)//takip ettiklerinin sayısı
        {
            string sql = @"SELECT COUNT([f].[FollowId]) FROM [Follows] as [f]
                           where [f].[FollowUpUserId]=@FollowUpUserId";
            return Connection.Query<int>(sql, new { FollowUpUserId = followUpUserId }).FirstOrDefault();
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takip ettiği kullanıcı listesi
        /// </summary>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>kullanıcı listesi</returns>
        public ServiceModel<ChatPeopleModel> FollowingChatList(string followedUserId, string search, PagingModel pagingModel)
        {
            string sql = @"select [u].[LastActiveDate] as [LastActiveDate],
                           [u].[ProfilePicturePath] as [ProfilePicturePath],
                           [u].[Id] as [UserId],
                           [u].[FullName] as [FullName],
                           [u].[UserName] as [UserName]
                           from [Follows] as [f]
                           INNER JOIN [AspNetUsers] AS [u] on [f].[FollowedUserId]=[u].[Id]
                           where [f].[FollowUpUserId]=@FollowUpUserId and ([u].[FullName] like '%' + @Search + '%' or [u].[UserName] like '%' + @Search + '%' or @Search='')
                           order by [f].[FollowId] desc";
            return Connection.QueryPaging<ChatPeopleModel>(sql, new { FollowUpUserId = followedUserId, Search = search }, pagingModel);
        }

        /// <summary>
        /// Kullanıcının Takip edenler(Takipçileri)
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takipçi listesi</returns>
        public ServiceModel<ListOfFollowerModel> Followers(string followedUserId, string followUpUserId, PagingModel pagingModel)
        {
            string sql = @"select
                           [f].[FollowUpUserId] as [FollowUpUserId],
                           [u].[FullName],
                           [u].[UserName],
                           [u].[ProfilePicturePath] as [ProfilePicturePath],
                           (SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS [EMPTY] FROM [Follows] as [ff]
                           where [ff].[FollowUpUserId]=@FollowUpUserId and [ff].[FollowedUserId]=[f].[FollowUpUserId]
                           ) THEN 1 ELSE 0 END) AS[value]) as [IsFollowUpStatus]
                           from [Follows] as [f]
                           INNER JOIN [AspNetUsers] as [u] on [f].[FollowUpUserId]=[u].[Id]
                           where [f].[FollowedUserId]=@FollowedUserId
                           order by [f].[FollowId] desc";
            return Connection.QueryPaging<ListOfFollowerModel>(sql, new { FollowedUserId = followedUserId, FollowUpUserId = followUpUserId }, pagingModel);
        }

        /// <summary>
        /// Kullanıcının takip ettikleri
        /// </summary>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Takip ettiklerinin listesi</returns>
        public ServiceModel<ListOfFollowerModel> Following(string followedUserId, string followUpUserId, PagingModel pagingModel)
        {
            string sql = @"select
                           [f].[FollowUpUserId] as [FollowUpUserId],
                           [u].[FullName],
                           [u].[UserName],
                           [u].[ProfilePicturePath] as [ProfilePicturePath],
                           (SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS [EMPTY] FROM [Follows] as [ff]
                           where [ff].[FollowUpUserId]=@FollowUpUserId and [ff].[FollowedUserId]=[f].[FollowUpUserId]
                           ) THEN 1 ELSE 0 END) AS[value]) as [IsFollowUpStatus]
                           from [Follows] as [f]
                           INNER JOIN [AspNetUsers] as [u] on [f].[FollowedUserId]=[u].[Id]
                           where [f].[FollowUpUserId]=@FollowedUserId
                           order by [f].[FollowId] desc";
            return Connection.QueryPaging<ListOfFollowerModel>(sql, new { FollowedUserId = followedUserId, FollowUpUserId = followUpUserId }, pagingModel);
        }

        /// <summary>
        /// Takip etme durumu kontrol eder takip ediyorsa true etmiyorsa false döner
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <returns>Takip ediyorsa true etmiyorsa false</returns>
        public bool IsFollowUpStatus(string followUpUserId, string followedUserId)
        {
            string sql = @"(SELECT (CASE WHEN EXISTS(
                            SELECT NULL AS [EMPTY] FROM [Follows] as [ff]
                            where [ff].[FollowUpUserId]=@FollowUpUserId and [ff].[FollowedUserId]=@FollowedUserId
                            ) THEN 1 ELSE 0 END) AS[value])";
            return Connection.Query<bool>(sql, new { FollowUpUserId = followUpUserId, FollowedUserId = followedUserId }).FirstOrDefault();
        }

        #endregion Methods
    }
}