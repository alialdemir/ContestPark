using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class ChatDetailsPageViewModel : ViewModelBase<ChatHistoryModel>
    {
        #region Private variable

        private string _fullName, _userName, _message = String.Empty;
        private readonly IChatsService _chatsService;
        private readonly IChatBlocksService _chatBlocksService;
        private readonly IChatRepliesService _chatRepliesService;
        public readonly IUserDataModule _userDataModule;
        private readonly IChatsSignalRService _chatsSignalRService;

        #endregion Private variable

        #region Constructors

        public ChatDetailsPageViewModel(IPageDialogService pageDialogService,
                                        IChatsService chatsService,
                                        IChatBlocksService chatBlocksService,
                                        IChatRepliesService chatRepliesService,
                                        IUserDataModule userDataModule,
                                        IChatsSignalRService chatsSignalRService) : base(dialogService: pageDialogService)
        {
            _chatsService = chatsService;
            _chatBlocksService = chatBlocksService;
            _chatRepliesService = chatRepliesService;
            _userDataModule = userDataModule;
            _chatsSignalRService = chatsSignalRService;
        }

        #endregion Constructors

        #region SignalR

        private void StartHub()
        {
            _chatsSignalRService.OnDataReceived += Client_OnDataReceived;
            _chatsSignalRService.ChatProxy();
        }

        /// <summary>
        /// Gelen mesajları dinleme
        /// </summary>
        private void Client_OnDataReceived(object sender, ChatHistoryModel chatHistory)
        {
            if (SenderUserId == chatHistory.SenderId)
            {
                Items.Add(chatHistory);
                Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 3), () =>
                {
                    ListViewScrollToBottomCommand.Execute(Items.Count - 1);
                    return false;
                });
            }
            // tab dan badge attır ve chat list yenile
        }

        #endregion SignalR

        #region Properties

        public string SenderUserId { get; private set; }

        /// <summary>
        /// message property
        /// </summary>
        public string Message
        {
            get { return _message.Trim(); }
            set { SetProperty(ref _message, value, nameof(Message)); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ServiceModel = await _chatsService.ChatHistoryAsync(SenderUserId, ServiceModel);
            await base.LoadItemsAsync();
            ListViewScrollToBottomCommand.Execute(ServiceModel.PageNumber == 1 ? Items.Count - 1 : Items.Count / 3);
            IsBusy = false;
        }

        /// <summary>
        /// Mesaj gönder
        /// </summary>
        private async Task ExecuteSendMessageCommand()
        {
            if (IsBusy || String.IsNullOrEmpty(_message))
                return;

            IsBusy = true;
            Items.Add(new ChatHistoryModel
            {
                Date = DateTime.Now,
                Message = _message,
                PicturePath = _userDataModule.UserModel.UserProfilePicturePath,
                SenderId = _userDataModule.UserModel.UserId
            });
            await _chatsService.SendChat(new SendChatModel
            {
                Message = _message,
                ReceiverId = SenderUserId,
                PublicKey = "675b5dce-10cc-4bcd-b635-1e911f6c4eaa"// TODO: config gibi bir yerden çekilmeli
            });
            ListViewScrollToBottomCommand.Execute(Items.Count - 1);
            Message = String.Empty;
            IsBusy = false;
        }

        /// <summary>
        /// Kullanıcı engelle
        /// </summary>
        /// <param name="isBlocking"></param>
        private async Task ExecuteBlockingProgressCommandAsync(bool isBlocking)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            await _chatBlocksService.BlockingProgressAsync(_fullName, SenderUserId, isBlocking);
            IsBusy = false;
        }

        /// <summary>
        /// Mesaj sil execute
        /// </summary>
        private async Task ExecuteDeleteMessageCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            bool isRemoveMessages = await _chatRepliesService.DeleteChatsAsync(SenderUserId);
            if (isRemoveMessages) Items.Clear();
            IsBusy = false;
        }

        /// <summary>
        /// Mesaj ayarları
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteChatSettingsCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            bool isBlocking = await _chatBlocksService.BlockingStatusAsync(SenderUserId);
            string buttons = isBlocking ? ContestParkResources.RemoveBlock : ContestParkResources.Block;
            string selectedActionSheet = await DisplayActionSheetAsync(
                ContestParkResources.ChatSettings,
                ContestParkResources.Cancel, null,
                ContestParkResources.Deleteconversation, buttons);
            IsBusy = false;
            //////////////////if (selectedActionSheet == ContestParkResources.Deleteconversation) DeleteMessageCommand.Execute(null);
            //////////////////else if (selectedActionSheet == buttons) BlockingProgressCommand.Execute(isBlocking);
        }

        #endregion Methods

        #region Commands

        /// <summary>
        /// Sohbet ayaralrı toolbaritem command
        /// </summary>
        private Command chatSettingsCommand;

        public Command ChatSettingsCommand
        {
            get { return chatSettingsCommand ?? (chatSettingsCommand = new Command(async () => await ExecuteChatSettingsCommandAsync())); }
        }

        /// <summary>
        /// Listview scroll aşağıya çeker
        /// </summary>
        public Command ListViewScrollToBottomCommand { get; set; }

        private Command gotoMyProfileCommand;

        /// <summary>
        /// Kendi profil resmine tıklayınca profiline gitmesi için command
        /// </summary>
        //////////////////public Command GotoMyProfileCommand
        //////////////////{
        //////////////////    get { return gotoMyProfileCommand ?? (gotoMyProfileCommand = new Command(() => PushNavigationPageAsync($"{nameof(ProfilePage)}?UserName={_userDataModule.UserModel.UserName}"))); }
        //////////////////}
        private Command gotoProfileCommand;

        /// <summary>
        /// Konuştuğu kişinin profil resmine tıklayınca profiline gitmesi için command
        /// </summary>
        public Command GotoProfileCommand
        {
            get { return gotoProfileCommand ?? (gotoProfileCommand = new Command(async () => await ExecuteDeleteMessageCommandAsync())); }
        }

        private Command deleteMessageCommand;

        /// <summary>
        /// Mesaj sil command
        /// </summary>
        //////////////////////public Command DeleteMessageCommand
        //////////////////////{
        //////////////////////    get { return deleteMessageCommand ?? (deleteMessageCommand = new Command(() => PushNavigationPageAsync($"{nameof(ProfilePage)}?UserName={_userName}"))); }
        //////////////////////}
        private Command<bool> blockingProgressCommand;

        /// <summary>
        /// Engelleme işlemi command
        /// </summary>
        public Command<bool> BlockingProgressCommand
        {
            get { return blockingProgressCommand ?? (blockingProgressCommand = new Command<bool>(async (isBlocking) => await ExecuteBlockingProgressCommandAsync(isBlocking))); }
        }

        private Command sendMessageCommand;

        /// <summary>
        /// Mesaj gönder command
        /// </summary>
        public Command SendMessageCommand
        {
            get { return sendMessageCommand ?? (sendMessageCommand = new Command(async () => await ExecuteSendMessageCommand())); }
        }

        #endregion Commands

        #region Navigations

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("UserName")) _userName = parameters.GetValue<string>("UserName");
            if (parameters.ContainsKey("FullName")) _fullName = Title = parameters.GetValue<string>("FullName");
            if (parameters.ContainsKey("SenderUserId")) SenderUserId = parameters.GetValue<string>("SenderUserId");
            StartHub();
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigations
    }
}