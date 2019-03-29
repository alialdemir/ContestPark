using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        ServiceModel<ChatListModel> UserChatList(string userId, PagingModel pagingModel);

        ServiceModel<ChatHistoryModel> ChatHistory(string receiverId, string senderId, PagingModel pagingModel);

        ServiceModel<ChatPeopleModel> ChatPeople(string userId, string search, PagingModel pagingModel);

        int Conversations(string receiverId, string senderId);
    }
}