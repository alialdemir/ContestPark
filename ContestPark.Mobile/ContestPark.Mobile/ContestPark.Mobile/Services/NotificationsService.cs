using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class NotificationsService : ServiceBase, INotificationsService
    {
        #region Constructors

        public NotificationsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Login olan kullanicinin tüm bildirimleri
        /// </summary>
        /// <returns>Bildirim listesi</returns>
        public async Task<ServiceModel<UserNotificationListModel>> UserNotificationAllAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<UserNotificationListModel>>($"Notifications{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Login olan kullanicinin görünmeyen bildirim sayisiný verir
        /// </summary>
        /// <returns>Görülmeyen bildirim sayisi</returns>
        public async Task<int> UserNotificationVisibilityCountAsync()
        {
            var result = await RequestProvider.GetDataAsync<int>($"Notifications/UserNotificationVisibilityCount");
            return result.Data;
        }

        /// <summary>
        /// Bildirim sil
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int notificationId)
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"Notifications/{notificationId}");
            return result.IsSuccess;
        }

        /// <summary>
        /// Login olan kullanicinin tüm bildirimlerini görüldü yapar
        /// </summary>
        /// <returns></returns>
        public async Task<bool> NotificationSeenAsync()
        {
            var result = await RequestProvider.PostDataAsync<string>($"Notifications/Allseen");
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface INotificationsService
    {
        Task<ServiceModel<UserNotificationListModel>> UserNotificationAllAsync(PagingModel pagingModel);

        Task<int> UserNotificationVisibilityCountAsync();

        Task<bool> DeleteAsync(int notificationId);

        Task<bool> NotificationSeenAsync();
    }
}