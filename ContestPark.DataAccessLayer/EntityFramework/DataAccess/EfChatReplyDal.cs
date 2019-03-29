using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfChatReplyDal : EfEntityRepositoryBase<ChatReply>, IChatReplyRepository
    {
        #region Constructors

        public EfChatReplyDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

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
                executeRowCount = DbContext.Database.ExecuteSqlCommand(
                    // Eğer Mesaj Gönderen ise ReceiverDeletingStatus false yapıldı böylece mesajları artık görmiycek
                    $"update ChatReplies set ReceiverDeletingStatus='False' where ChatId={chatId} and ReceiverDeletingStatus='True' and UserId in (select cr.UserId from ChatReplies cr where cr.UserId ='{userId}' and cr.ChatId={chatId});" +
                    // Eğer mesajı alan ise SenderDeletingStatus false yapldı çünkü mesaj tek row da tutulduğu için bir taraf sildimi diğer kullanıcıda da silincekti bu yüzden bool bir tip ile
                    // hangi taraf sildi silmedi kontrol ediyoruz mesaj listelemelerde bu değerlere göre kontrol edip listeliyor...
                    $"update ChatReplies set SenderDeletingStatus='False' where ChatId={chatId} and SenderDeletingStatus='True' and UserId not in (select cr.UserId from ChatReplies cr where cr.UserId ='{userId}' and cr.ChatId={chatId});");
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
            return (from c in DbContext.Set<Chat>()
                    where c.ReceiverId == userId || c.SenderId == userId
                    join cr in DbSet on c.ChatId equals cr.ChatId
                    where cr.VisibilityStatus && cr.UserId != userId
                    select cr.VisibilityStatus).Count();
        }

        /// <summary>
        /// Mesajı alan mesajı görmüş ise ilgili mesajı görme durumu false yapar
        /// </summary>
        /// <param name="receiverId">Mesajı alan kullanıcı Id</param>
        /// <param name="chatId">sohbet Id</param>
        /// <returns>İşlem durumu</returns>
        public void ChatSeen(string receiverId, int chatId)
        {
            int executeRowCount = DbContext.Database.ExecuteSqlCommand($"Update ChatReplies Set VisibilityStatus='False' where UserId={receiverId} and ChatId={chatId};");
            if (executeRowCount <= 0)
                BadStatus("ChatNotSeen");
        }

        /// <summary>
        /// Kullanıcının görmediği tüm mesajlarını görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>İşlem durumu</returns>
        public async Task<bool> ChatSeen(string userId)
        {
            int executeRowCount = await DbContext.Database.ExecuteSqlCommandAsync($"Update ChatReplies Set VisibilityStatus=0 where ChatReplyId in (SELECT cr.ChatReplyId FROM ChatReplies cr INNER JOIN Chats  c ON cr.ChatId=c.ChatId WHERE (c.ReceiverId={userId} or c.SenderId={userId}) and cr.VisibilityStatus =1)");
            return executeRowCount > 0;
        }

        #endregion Methods
    }
}