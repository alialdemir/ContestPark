using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IChatReplyService : IRepository<ChatReply>
    {
        int UserChatVisibilityCount(string userId);

        void ChatSeen(string receiverId, int chatId);

        Task<bool> ChatSeen(string userId);

        bool Delete(string userId, string receiverUserId);
    }
}