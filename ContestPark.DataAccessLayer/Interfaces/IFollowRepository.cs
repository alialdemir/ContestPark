using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IFollowRepository : IRepository<Follow>
    {
        ServiceModel<ListOfFollowerModel> Followers(string followedUserId, string followUpUserId, PagingModel pageModel);//Kullanıcıyı takip edenler

        ServiceModel<ListOfFollowerModel> Following(string followedUserId, string followUpUserId, PagingModel pageModel);//Kullanıcının takip ettikleri

        bool IsFollowUpStatus(string followUpUserId, string followedUserId);

        void Delete(string followUpUserId, string followedUserId);

        int FollowersCount(string followedUserId);//Takipçi sayısı

        int FollowUpCount(string followUpUserId);//Takip ettiklerinin sayısı

        ServiceModel<ChatPeopleModel> FollowingChatList(string followedUserId, string search, PagingModel pageModel);
    }
}