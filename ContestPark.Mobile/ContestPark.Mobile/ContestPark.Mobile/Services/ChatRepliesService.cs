using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class ChatRepliesService : ServiceBase, IChatRepliesService
    {
        #region Constructors

        public ChatRepliesService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının okunmamış mesaj sayısı
        /// </summary>
        /// <returns>Okunmamýþ mesaj sayısı</returns>
        public async Task<int> UserChatVisibilityCountAsync()
        {
            var result = await RequestProvider.GetDataAsync<int>($"ChatReplies");
            return result.Data;
        }

        /// <summary>
        /// Parametreden gelen chat id'yi görüldü yapar
        /// </summary>
        /// <param name="chatId">Mesaj Id</param>
        /// <returns></returns>
        public async Task<bool> ChatSeenAsync(int chatId)
        {
            var result = await RequestProvider.GetDataAsync<string>($"ChatReplies/{chatId}");
            return result.IsSuccess;
        }

        /// <summary>
        /// Kullanıcının görmediği tüm mesajlarını görüldü yapar
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ChatSeenAsync()
        {
            var result = await RequestProvider.PostDataAsync<bool>($"ChatReplies/Allseen");
            return result.Data;
        }

        /// <summary>
        /// İlgili chat'deki tüm mesajları sil
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteChatsAsync(string receiverUserId)
        {
            if (string.IsNullOrEmpty(receiverUserId)) return false;
            var result = await RequestProvider.DeleteDataAsync<bool>($"ChatReplies/{receiverUserId}");
            return result.Data;
        }

        #endregion Methods
    }

    public interface IChatRepliesService
    {
        Task<bool> DeleteChatsAsync(string receiverUserId);

        Task<bool> ChatSeenAsync();

        Task<bool> ChatSeenAsync(int chatId);

        Task<int> UserChatVisibilityCountAsync();
    }
}