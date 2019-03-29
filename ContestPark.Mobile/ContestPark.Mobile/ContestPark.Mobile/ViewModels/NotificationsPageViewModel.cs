using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase<UserNotificationListModel>
    {
        #region Private varaibles

        private readonly INotificationsService _notificationsService;
        private readonly IDuelInfosService _duelInfosService;

        #endregion Private varaibles

        #region Constructors

        public NotificationsPageViewModel(INotificationsService notificationsService,
                                          IDuelInfosService duelInfosService)
        {
            Title = ContestParkResources.Notifications;
            _notificationsService = notificationsService;
            _duelInfosService = duelInfosService;
        }

        #endregion Constructors

        #region Properties

        private string _badgeCount;

        public string BadgeCount
        {
            get { return _badgeCount; }
            set { SetProperty(ref _badgeCount, value, nameof(BadgeCount)); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ServiceModel = await _notificationsService.UserNotificationAllAsync(ServiceModel);
            await base.LoadItemsAsync();

            if (!String.IsNullOrEmpty(BadgeCount))
            {
                bool isSuccess = await _notificationsService.NotificationSeenAsync();
                if (isSuccess) BadgeCount = String.Empty;
            }
            IsBusy = false;
        }

        /// <summary>
        /// Düelloyu kabul et
        /// </summary>
        /// <param name="userNotificationListModel">Düello modeli</param>
        public void ExecuteAcceptsDuelCommandAsync(int notificationId)
        {
            if (IsBusy)
                return;

            var selectedModel = Items.FirstOrDefault(i => i.NotificationId == notificationId);
            if (selectedModel == null)
                return;
            Task.Run(async () =>
            {
                IsBusy = true;
                bool isSuccess = await _duelInfosService.AcceptsDuelWithNotificationAsync(selectedModel.NotificationId, selectedModel.SubCategoryId);
                if (isSuccess)
                {
                    isSuccess = await _notificationsService.DeleteAsync(notificationId);
                    if (isSuccess)
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Items.Remove(selectedModel));
                        //DuelModule.IsRobot = false;// Botu kapattık
                        //await Navigation.PushAsync(new DuelEnterScreenPage("0", int.Parse(selectedModel.Link), selectedModel.SubCategoryId, false));
                    }
                }
                IsBusy = false;
            });
        }

        /// <summary>
        /// Düello yenil
        /// </summary>
        /// <param name="notificationId">bildirim Id</param>
        public void ExecuteSmotherDuelCommandAsync(int notificationId)
        {
            if (IsBusy)
                return;

            var selectedModel = Items.FirstOrDefault(i => i.NotificationId == notificationId);
            if (selectedModel == null)
                return;
            Task.Run(async () =>
            {
                IsBusy = true;
                bool isOk = await DisplayAlertAsync(ContestParkResources.Defeat + "?",
                                                    ContestParkResources.AdmitDefeat,
                                                    ContestParkResources.Defeat,
                                                    ContestParkResources.Cancel);

                if (isOk)
                {
                    bool isSuccess = await _duelInfosService.SmotherDuelAsync(int.Parse(selectedModel.Link), selectedModel.SubCategoryId);

                    if (isSuccess)
                    {
                        isSuccess = await _notificationsService.DeleteAsync(notificationId);
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            if (isSuccess) Items.Remove(selectedModel);
                        });
                    }
                }
                IsBusy = false;
            });
        }

        /// <summary>
        /// notification sil
        /// </summary>
        /// <param name="notificationId">notification Id</param>
        public void ExecuteDeleteItemCommandAsync(int notificationId)
        {
            if (IsBusy)
                return;

            var selectedModel = Items.FirstOrDefault(i => i.NotificationId == notificationId);
            if (selectedModel == null)
                return;
            Task.Run(async () =>
            {
                IsBusy = true;
                bool isContest = (byte)NotificationTypes.Contest == selectedModel.NotificationTypeId;
                string notificationRemoveMessage = isContest ?
                    ContestParkResources.DuelDefeatMessage :
                    ContestParkResources.NotificationRemoveMessage;

                bool isOk = await DisplayAlertAsync(ContestParkResources.NotificationRemove + "?",
                                                    notificationRemoveMessage,
                                                    ContestParkResources.Delete,
                                                    ContestParkResources.Cancel);
                if (isOk)
                {
                    bool isSuccess = await _notificationsService.DeleteAsync(notificationId);
                    if (isSuccess)
                    {
                        if (isContest) ExecuteSmotherDuelCommandAsync(selectedModel.NotificationId);
                        else Device.BeginInvokeOnMainThread(() => Items.Remove(selectedModel));
                    }
                }
                IsBusy = false;
            });
        }

        /// <summary>
        /// Kullanıcının görülmemiş bildirim sayısı
        /// </summary>
        private async Task UserNotificationVisibilityCountCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            BadgeCount = (await _notificationsService.UserNotificationVisibilityCountAsync()).ToString();
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        public ICommand UserNotificationVisibilityCountCommand => new Command(async () => await UserNotificationVisibilityCountCommandAsync());

        #endregion Commands
    }
}