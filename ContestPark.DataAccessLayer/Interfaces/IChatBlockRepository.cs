using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IChatBlockRepository : IRepository<ChatBlock>
    {
        bool BlockingStatus(string whoId, string whonId);

        bool UserBlockingStatus(string whoId, string whonId);

        int GetChatBlockIdByWhonIdAndWhoId(string whoId, string whonId);

        ServiceModel<UserBlockListModel> UserBlockList(string userId, PagingModel pagingModel);

        void UserBlocking(string whoId, string whonId);
    }
}