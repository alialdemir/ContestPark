using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class CommentDapperRepository : DapperRepositoryBase<Comment>, ICommentRepository
    {
        #region Constructor

        public CommentDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// İlgili posta yapılan yorum sayısı
        /// </summary>
        /// <param name="postId">Kim ne yapıyor Id</param>
        /// <returns>Yorum sayısı</returns>
        public int CommentCount(int postId)
        {
            string sql = "select count(*) from [Posts] as [p] where [p].[PostId]=@PostId";
            return Connection.Query<int>(sql, new { PostId = postId }).FirstOrDefault();
        }

        /// <summary>
        /// İlgili posta yapılan yorumlar
        /// </summary>
        /// <param name="postId">Kim ne yapıyor Id</param>
        /// <returns>Post listesi</returns>
        public ServiceModel<CommentListModel> CommentList(int postId, PagingModel pagingModel)
        {
            string sql = @"select [u].[UserName], [u].[ProfilePicturePath], [c].[Text], [c].[CreatedDate] as [Date] from [Comments] as [c]
                           INNER JOIN [AspNetUsers] as [u] on [c].[UserId]=[u].[Id]
                           where [c].[PostId]=@PostId
                           order by [c].[CreatedDate] desc";
            return Connection.QueryPaging<CommentListModel>(sql, new { PostId = postId }, pagingModel);
        }

        /// <summary>
        /// Kullanıcının en son posta yaptığı yorumun tarihi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="postId">Kim ne yapıyor Id</param>
        /// <returns>En son yapılan yorumun tarihi</returns>
        public DateTime LastCommentTime(string userId, int postId)
        {
            string sql = @"select [c].[CreatedDate] from [Comments] as [c]
                           where [c].[UserId]=@UserId and [c].[PostId]=@PostId
                           order by [c].[CreatedDate] desc";
            var date = Connection.Query<DateTime>(sql, new { UserId = userId, PostId = postId }).First();
            return date != null ? date.Date : new DateTime();
        }

        #endregion Methods
    }
}