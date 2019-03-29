using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class ChatAllPageViewModel : ViewModelBase<ChatListModel>
    {
        #region Private variables

        private readonly IChatsService _chatsService;
        private readonly IChatRepliesService _chatRepliesService;

        #endregion Private variables

        #region Constructors

        public ChatAllPageViewModel(INavigationService navigationService,
                                IPageDialogService pageDialogService,
                                IChatsService chatsService,
                                IChatRepliesService chatRepliesService) : base(navigationService, pageDialogService)
        {
            Title = ContestParkResources.Chats;
            _chatsService = chatsService;
            _chatRepliesService = chatRepliesService;
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
            ServiceModel = await _chatsService.UserChatListAsync(ServiceModel);
            await base.LoadItemsAsync();

            if (!String.IsNullOrEmpty(BadgeCount))
            {
                bool isSuccess = await _chatRepliesService.ChatSeenAsync();
                if (isSuccess) BadgeCount = String.Empty;
            }

            IsBusy = false;
        }

        /// <summary>
        /// Mesaj sil
        /// </summary>
        /// <param name="chatId">Chat Id</param>
        public async Task ExecuteDeleteItemCommandAsync(string receiverUserId)
        {
            if (IsBusy)
                return;

            var selectedModel = Items.FirstOrDefault(i => i.SenderUserId == receiverUserId);
            if (selectedModel == null)
                return;

            IsBusy = true;
            bool isOk = await DisplayAlertAsync(
                  ContestParkResources.DeleteMessageTitle,
                  selectedModel.UserFullName + ContestParkResources.DeleteMessage,
                  ContestParkResources.Delete,
                  ContestParkResources.Cancel);
            if (isOk)
            {
                bool isSuccess = await _chatRepliesService.DeleteChatsAsync(receiverUserId);
                if (isSuccess) Items.Remove(selectedModel);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Mesaj detayına git
        /// </summary>
        /// <param name="receiverUserId">alıcının kullanıcı id</param>
        public async Task GotoChatDetails(string receiverUserId)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ChatListModel selectedModel = Items.FirstOrDefault(p => p.SenderUserId == receiverUserId);
            await PushNavigationPageAsync(nameof(ChatDetailsPage), new NavigationParameters
            {
                { "UserName", selectedModel.UserName},
                { "FullName", selectedModel.UserFullName},
                { "SenderUserId", selectedModel.SenderUserId},
            });
            IsBusy = false;
        }

        /// <summary>
        /// Kullanıcının görülmemiş mesaj sayısı
        /// </summary>
        private async Task UserChatVisibilityCountCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            BadgeCount = (await _chatRepliesService.UserChatVisibilityCountAsync()).ToString();
            IsBusy = false;
        }

        /// <summary>
        /// Profile sayfasına git
        /// </summary>
        /// <param name="userName">Profili açılacak kullanıcının kullanıcı adı</param>
        private async Task ExecuteGotoProfilePageCommand(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                await PushModalAsync(nameof(ProfilePage), new NavigationParameters
                {
                    {"UserName", userName }
                });
            }
        }

        #endregion Methods

        #region Commands

        public ICommand GoToUserChatListPageCommand => new Command(() => PushNavigationPageAsync(nameof(UserChatListPage)));
        public ICommand UserChatVisibilityCountCommand => new Command(async () => await UserChatVisibilityCountCommandAsync());
        private ICommand deleteItemCommand;

        /// <summary>
        /// Mesaj sil command
        /// </summary>
        public ICommand DeleteItemCommand
        {
            get { return deleteItemCommand ?? (deleteItemCommand = new Command<string>(async (senderUserId) => await ExecuteDeleteItemCommandAsync(senderUserId))); }
        }

        private ICommand gotoChatDetailsCommand;

        /// <summary>
        /// Mesaj sil command
        /// </summary>
        public ICommand GotoChatDetailsCommand
        {
            get { return gotoChatDetailsCommand ?? (gotoChatDetailsCommand = new Command<string>(async (senderUserId) => await GotoChatDetails(senderUserId))); }
        }

        private ICommand _gotoProfilePageCommand;

        public ICommand GotoProfilePageCommand =>
            _gotoProfilePageCommand ?? (_gotoProfilePageCommand = new Command<string>(async (userName) => await ExecuteGotoProfilePageCommand(userName)));

        #endregion Commands
    }
}