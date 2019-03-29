using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class ChatReplyDapperRepository : DapperRepositoryBase<ChatReply>, IChatReplyRepository
    {
        #region Constructor

        public ChatReplyDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Kullanıcı mesajını görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="chatId">Sohbet Id</param>
        /// <returns>İşlem durumu</returns>
        public bool Delete(string userId, int chatId)//Mesaj silme
        {
            int executeRowCount = 0;
            if (chatId > 0)
            {
                string sql =
                    // Eğer Mesaj Gönderen ise ReceiverDeletingStatus false yapıldı böylece mesajları artık görmiycek
                    $"update ChatReplies set ReceiverDeletingStatus='False' where ChatId=@ChatId and ReceiverDeletingStatus='True' and UserId in (select cr.UserId from ChatReplies cr where cr.UserId =@UserId and cr.ChatId=@ChatId);" +
                    // Eğer mesajı alan ise SenderDeletingStatus false yapldı çünkü mesaj tek row da tutulduğu için bir taraf sildimi diğer kullanıcıda da silincekti bu yüzden bool bir tip ile
                    // hangi taraf sildi silmedi kontrol ediyoruz mesaj listelemelerde bu değerlere göre kontrol edip listeliyor...
                    $"update ChatReplies set SenderDeletingStatus='False' where ChatId=@ChatId and SenderDeletingStatus='True' and UserId not in (select cr.UserId from ChatReplies cr where cr.UserId =@UserId and cr.ChatId=@ChatId);";
                executeRowCount = Connection.Execute(sql, new { UserId = userId, ChatId = chatId });
            }
            return executeRowCount > 0;
        }

        /// <summary>
        /// Kullanıcının okunmamış mesaj sayısı
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Okunmamış mesaj sayısı</returns>
        public int UserChatVisibilityCount(string userId)
        {
            string sql = @"select count([cr].[VisibilityStatus]) from [Chats] AS [c]
                           INNER JOIN [ChatReplies] AS [cr] on [c].[ChatId]=[cr].[ChatId]
                           where (([c].[ReceiverId]=@UserId or [c].[SenderId]=@UserId) and ([cr].[UserId]<>@UserId and [cr].[VisibilityStatus]=1))";
            return Connection.Query<int>(sql, new { UserId = userId }).FirstOrDefault();
        }

        /// <summary>
        /// Mesajı alan mesajı görmüş ise ilgili mesajı görme durumu false yapar
        /// </summary>
        /// <param name="receiverId">Mesajı alan kullanıcı Id</param>
        /// <param name="chatId">sohbet Id</param>
        /// <returns>İşlem durumu</returns>
        public void ChatSeen(string receiverId, int chatId)
        {
            string sql = "Update [ChatReplies] as [cr] Set [cr].[VisibilityStatus]='False' where [cr].[UserId]=@ReceiverId and [cr].[ChatId]=@ChatId";
            int executeRowCount = Connection.Execute(sql, new { ReceiverId = receiverId, ChatId = chatId });
            if (executeRowCount <= 0)
                Check.BadStatus("ChatNotSeen");
        }

        /// <summary>
        /// Kullanıcının görmediği tüm mesajlarını görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>İşlem durumu</returns>
        public async Task<bool> ChatSeen(string userId)
        {
            string sql = @"Update [ChatReplies] as [cr] Set [cr].[VisibilityStatus]=0 where [cr].[ChatReplyId] in (SELECT [cr].ChatReplyId FROM [ChatReplies] [cr]
                           INNER JOIN [Chats] [c] ON [cr].ChatId=[c].ChatId WHERE ([c].ReceiverId=@UserId or [c].SenderId=@UserId) and [cr].VisibilityStatus =1)";
            int executeRowCount = await Connection.ExecuteAsync(sql, new { UserId = userId });
            return executeRowCount > 0;
        }

        #endregion Methods
    }
}