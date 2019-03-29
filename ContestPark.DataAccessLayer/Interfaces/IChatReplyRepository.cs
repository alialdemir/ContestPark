using ContestPark.DataAccessLayer.Tables;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IChatReplyRepository : IRepository<ChatReply>
    {
        int UserChatVisibilityCount(string userId);

        void ChatSeen(string receiverId, int chatId);

        Task<bool> ChatSeen(string userId);

        bool Delete(string userId, int chatId);
    }
}