using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IChatBlockService : IRepository<ChatBlock>
    {
        bool BlockingStatus(string whoId, string whonId);

        bool UserBlockingStatus(string whoId, string whonId);

        void Delete(string whoId, string whonId);

        int GetChatBlockIdByWhonIdAndWhoId(string whoId, string whonId);

        ServiceModel<UserBlockListModel> UserBlockList(string userId, PagingModel pagingModel);

        void UserBlocking(string userId, string whonId);

        void ChatBlockRemove(int chatBlockId, string whonId, string userId);
    }
}