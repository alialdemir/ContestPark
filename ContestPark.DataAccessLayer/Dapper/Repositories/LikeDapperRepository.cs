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
    public class LikeDapperRepository : DapperRepositoryBase<Like>, ILikeRepository
    {
        #region Constructor

        public LikeDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Parametreden gelen kullanıcı id'sinin yine parametreden gelen kim ne yapıyor id'sini beğenme durumu
        /// Beğendiyse true beğenmediyse false dönder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="postId">Kim ne yapıyor id</param>
        /// <returns>Beğendiyse true beğenmediyse false</returns>
        public bool IsLike(string userId, int postId)//Beğenme durumu kontrol
        {
            string sql = @"SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS [EMPTY] FROM [Likes] as [l]
                           where [l].[UserId]=@UserId and [l].[PostId]=@PostId
                           ) THEN 1 ELSE 0 END) AS[value]";
            return Connection.Query<bool>(sql, new { UserId = userId, PostId = postId }).FirstOrDefault();
        }

        /// <summary>
        /// Parametreden gelen kim ne yapıyor id'sini beğenen kullanıcı sayısı
        /// </summary>
        /// <param name="postId">Kim ne yapıyor id</param>
        /// <returns>Beğenen kullanıcı sayısı</returns>
        public int LikeCount(int postId)
        {
            string sql = @"SELECT count([l].[LikeId]) FROM [Likes] as [l] where [l].[PostId]=@PostId";
            return Connection.Query<int>(sql, new { PostId = postId }).FirstOrDefault();
        }

        /// <summary>
        /// Kim ne yapıyoru beğenmekten vazgeç
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="postId">Kim ne yapıyor id</param>
        public void DisLike(string userId, int postId)
        {
            string sql = "delete from Likes where UserId=@UserId and PostId=@PostId";
            int row = Connection.Execute(sql, new { UserId = userId, PostId = postId });
            if (row <= 0) Check.BadStatus("ServerMessage_thisLinkUnLike");
        }

        /// <summary>
        /// Parametreden gelen kim ne yapıyor id'sini beğenen kullanıcılar
        /// ve
        /// yine parametreden gelen kullanıcının o postu beğenme durumu
        /// IsFollowUpStatus: beğendiyse true beğenmediyse false
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="postId">Kim ne apıyor id</param>
        /// <param name="pagingModel">Sayfalama</param>
        /// <returns>Beğenen kullanıcı bilgileri</returns>
        public ServiceModel<LikesModel> Likes(string userId, int postId, PagingModel pagingModel)
        {
            string sql = @"select
                           [u].[Id] as [FollowUpUserId],
                           [u].[FullName],
                           [u].[UserName],
                           [u].[ProfilePicturePath],
                           (SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS [EMPTY] FROM [Follows] as [f]
                           where [f].[FollowUpUserId]=@UserId and [f].[FollowedUserId]=[u].[Id]
                           ) THEN 1 ELSE 0 END) AS[value]) as [IsFollowUpStatus]
                           from [Likes] AS [l]
                           INNER JOIN [AspNetUsers] AS [u] on [u].[Id]=[l].[UserId]
                           where [l].[PostId]=@PostId";
            return Connection.QueryPaging<LikesModel>(sql, new { UserId = userId, PostId = postId, }, pagingModel);
        }

        #endregion Methods
    }
}