using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class ChatsService : ServiceBase, IChatsService
    {
        #region Constructors

        public ChatsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ListTypes göre mesajlaşabileceği kullanıcıları yada takip ettiği kullanıcıların listesini getir
        /// </summary>
        /// <returns>Mesajlaşabileceği insanlar</returns>
        public async Task<ServiceModel<ChatPeopleModel>> ChatPeopleAsync(string search, PagingModel pageModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ChatPeopleModel>>($"Chats{pageModel.ToString()}&search={search}");
            return result.Data;
        }

        /// <summary>
        /// Parametreden geln kullanıcı arasındaki sohbet geçmişi
        /// </summary>
        /// <param name="senderId">kullanıcı Id</param>
        /// <returns>Sohbet geçmiþinin listesi</returns>
        public async Task<ServiceModel<ChatHistoryModel>> ChatHistoryAsync(string senderUserId, PagingModel pagingModel)
        {
            var result = await RequestProvider.PostDataAsync<ServiceModel<ChatHistoryModel>>($"Chats/{senderUserId}{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Login olan kullanıcının mesaj listesi
        /// </summary>
        /// <returns>Kullanıcı chat listesi</returns>
        public async Task<ServiceModel<ChatListModel>> UserChatListAsync(PagingModel pageModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ChatListModel>>($"Chats/User{pageModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Mesaj gönder
        /// </summary>
        /// <returns>Başarılı olma durumu</returns>
        public async Task<bool> SendChat(SendChatModel sendChat)
        {
            var result = await RequestProvider.PostDataAsync<ServiceModel<ChatListModel>>($"Chats", sendChat);
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface IChatsService
    {
        Task<bool> SendChat(SendChatModel sendChat);

        Task<ServiceModel<ChatListModel>> UserChatListAsync(PagingModel pageModel);

        Task<ServiceModel<ChatHistoryModel>> ChatHistoryAsync(string senderUserId, PagingModel pagingModel);

        Task<ServiceModel<ChatPeopleModel>> ChatPeopleAsync(string search, PagingModel pageModel);
    }
}