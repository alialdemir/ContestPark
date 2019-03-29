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
    public class ChatDapperRepository : DapperRepositoryBase<Chat>, IChatRepository
    {
        #region Constructor

        public ChatDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// İki kullanıcı arasında mesaj gönderilmiş mi kontrol eder
        /// </summary>
        /// <param name="receiverId">Gönderen kullanıcı Id</param>
        /// <param name="senderId">Alıcı kullanıcı adı</param>
        /// <returns>İki kullanıcı arasındaki mesaj Id</returns>
        public int Conversations(string receiverId, string senderId)
        {
            string sql = @"select [c].[ChatId] from [Chats] as [c]
                           where ([c].ReceiverId = @ReceiverId and [c].SenderId = @SenderId) or ([c].ReceiverId = @SenderId and [c].SenderId = @ReceiverId)";
            return Connection.Query<int>(sql, new { ReceiverId = receiverId, SenderId = senderId }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcılar arasındaki mesaj geçmişi
        /// </summary>
        /// <param name="receiverId">Gönderem Kullanıcı Id</param>
        /// <param name="senderId">Alıcı Kullanıcı Id</param>
        /// <returns>Mesaj geçmişi listesi</returns>
        public ServiceModel<ChatHistoryModel> ChatHistory(string receiverId, string senderId, PagingModel pagingModel)//Kullanıcı mesajlarının geçmişi(detayı) son 10
        {
            string sql = @"select [cr].[CreatedDate] as [Date], [cr].[Message], [cr].[UserId] as [SenderId], [u].[ProfilePicturePath] as [PicturePath], [u].[UserName] from [Chats] as [c]
                           INNER JOIN [ChatReplies] as [cr] on [c].[ChatId] = [cr].[ChatId]
                           INNER JOIN [AspNetUsers] as [u] on [cr].UserId = [u].[Id]
                           where (([c].[ReceiverId] = @ReceiverId and [c].[SenderId] = @SenderId) or ([c].[ReceiverId] = @SenderId and [c].[SenderId] = @ReceiverId))
                           and (([cr].[UserId] = @ReceiverId and [cr].[ReceiverDeletingStatus]=1) or ([cr].[UserId] = @SenderId and [cr].[SenderDeletingStatus]=1))
                           order by [cr].[CreatedDate] desc";
            return Connection.QueryPaging<ChatHistoryModel>(sql, new { ReceiverId = receiverId, SenderId = senderId }, pagingModel);
        }

        /// <summary>
        /// Kullanıcının mesaj listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kullanıcıya ait mesajlar</returns>
        public ServiceModel<ChatListModel> UserChatList(string userId, PagingModel pagingModel)//Kullanıcı mesaj listesi
        {
            string sql = @"SELECT [u].[UserName],
                           [u].[FullName] AS [UserFullName],
                           [u].[Id] AS [SenderUserId],
                           [cr].[CreatedDate] AS [Date],
                           CASE
                           WHEN  [cr].[UserId] <> @UserId
                           THEN [cr].[VisibilityStatus]
                           ELSE CAST(0 AS BIT)
                           END AS [VisibilityStatus],
                           CASE WHEN [cr].[UserId] = @UserId THEN
                           CASE WHEN @LangId = 1
                           THEN N'Sen : ' +  [cr].[Message]
                           ELSE N'You :' + [cr].[Message]
                           END ELSE  [cr].[Message]
                           END AS [Message],
                           CASE
                           WHEN (
                           SELECT CASE
                           WHEN EXISTS (
                           SELECT 1
                           FROM [ChatBlocks] AS [Cp]
                           WHERE (([Cp].[WhoId] = @UserId) AND (([Cp].[WhonId] = CASE
                           WHEN [c].[SenderId] = @UserId
                           THEN [c].[ReceiverId] ELSE [c].[SenderId]
                           END) OR ([Cp].[WhonId] IS NULL AND CASE
                           WHEN [c].[SenderId] = @UserId
                           THEN [c].[ReceiverId] ELSE [c].[SenderId]
                           END IS NULL))) OR ((([Cp].[WhoId] = CASE
                           WHEN [c].[SenderId] = @UserId
                           THEN [c].[ReceiverId] ELSE [c].[SenderId]
                           END) OR ([Cp].[WhoId] IS NULL AND CASE
                           WHEN [c].[SenderId] = @UserId
                           THEN [c].[ReceiverId] ELSE [c].[SenderId]
                           END IS NULL)) AND ([Cp].[WhonId] = @UserId)))
                           THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
                           END
                           ) = 1
                           THEN '' ELSE [u].[ProfilePicturePath]
                           END AS [UserProfilePicturePath]
                           FROM [ChatReplies] AS [cr]
                           INNER JOIN [Chats] AS [c] on [cr].[ChatId] =[c].[ChatId]
                           INNER JOIN [AspNetUsers] AS [u] on CASE WHEN [c].[SenderId] = @UserId THEN [c].[ReceiverId] ELSE [c].[SenderId] END = [u].[Id]
                           Where ([c].[ReceiverId]=@UserId or [c].[SenderId]=@UserId)
                           and (([cr].[UserId] = @UserId and [cr].[ReceiverDeletingStatus]=1) or ([cr].[UserId] != @UserId and [cr].[SenderDeletingStatus]=1))
                           and [cr].[ChatReplyId] in (select max(cr1.ChatReplyId) from [ChatReplies] AS [cr1] group by [cr1].[ChatId])
                           order by [cr].[CreatedDate] desc";
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return Connection.QueryPaging<ChatListModel>(sql, new { UserId = userId, LangId = (byte)language }, pagingModel);
        }

        /// <summary>
        /// Online olma sırasına göre kullanıcı listesi login olan kullanıcı hariç
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Kullanıcı listesi</returns>
        public ServiceModel<ChatPeopleModel> ChatPeople(string userId, string search, PagingModel pagingModel)
        {
            string sql = @"SELECT [u].[FullName], [u].[UserName], [u].[LastActiveDate], [u].[ProfilePicturePath], [u].[Id] AS [UserId]
                           FROM [AspNetUsers] AS [u]
                           WHERE (((([u].[Status] = 1) AND ([u].[Id] <> @UserId)) AND NOT EXISTS (
                           SELECT 1
                           FROM [ChatBlocks] AS [c]
                           WHERE [u].[Id] = [c].[WhonId])) AND NOT EXISTS (
                           SELECT 1
                           FROM [ChatBlocks] AS [c0]
                           WHERE [u].[Id] = [c0].[WhoId])) AND ((((CHARINDEX(@search, LOWER([u].[FullName])) > 0)
                           OR (@search = N'')) OR ((CHARINDEX(@search, LOWER([u].[UserName])) > 0)
		                   OR (@search = N''))) OR (@search IS NULL OR (@search = N'')))
                           ORDER BY [u].[LastActiveDate] DESC";
            return Connection.QueryPaging<ChatPeopleModel>(sql, new { UserId = userId, Search = userId }, pagingModel);
        }

        #endregion Methods
    }
}