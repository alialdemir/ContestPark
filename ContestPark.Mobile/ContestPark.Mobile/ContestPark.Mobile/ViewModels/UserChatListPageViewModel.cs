using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class UserChatListPageViewModel : ViewModelBase<ChatPeopleModel>
    {
        #region Private variable

        private readonly IChatsService _chatsService;
        private readonly IFollowsService _followsService;

        #endregion Private variable

        #region Constructors

        public UserChatListPageViewModel(INavigationService navigationService,
                                     IChatsService chatsService,
                                     IFollowsService followsService) : base(navigationService)
        {
            Title = ContestParkResources.NewChat;
            _chatsService = chatsService;
            _followsService = followsService;
        }

        #endregion Constructors

        #region Properties

        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                SetProperty(ref _search, value, nameof(Search));
                if (_search.Length >= 3 || String.IsNullOrEmpty(_search)) RefleshCommand.Execute(null);
            }
        }

        public ListTypes ListType { get; set; } = ListTypes.ChatPeople;
        private bool _isVisibleSearch = false;

        public bool IsVisibleSearch
        {
            get { return _isVisibleSearch; }
            set { SetProperty(ref _isVisibleSearch, value, nameof(IsVisibleSearch)); }
        }

        #endregion Properties

        #region Enums

        public enum ListTypes : byte
        {
            ChatPeople = 0,
            Following = 1,
        }

        #endregion Enums

        #region Methods

        public async Task ExecuteGoToChatDetailsPageCommand(ChatPeopleModel selectedModel)
        {
            if (selectedModel != null)
            {
                await PushNavigationPageAsync(nameof(ChatDetailsPage), new NavigationParameters
                {
                    {"UserName", selectedModel.UserName },
                    {"FullName", selectedModel.FullName },
                    {"SenderUserId", selectedModel.UserId }
                });
            }
        }

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (ListTypes.ChatPeople.HasFlag(ListType)) ServiceModel = await _chatsService.ChatPeopleAsync(Search, ServiceModel);
            else ServiceModel = await _followsService.FollowingChatListAsync(Search, ServiceModel);
            await base.LoadItemsAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Segmente tıklanınca listeleme tipi değiştir
        /// </summary>
        /// <param name="value">seçilen segment id</param>
        private void SegmentValueChanged(int selectedSegmentIndex)
        {
            ListType = selectedSegmentIndex == 0 ? ListTypes.ChatPeople : ListTypes.Following;
            Search = String.Empty;
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

        public ICommand SearchCommand => new Command(() => IsVisibleSearch = !IsVisibleSearch);
        public ICommand SegmentValueChangedCommand => new Command<int>((selectedSegmentIndex) => SegmentValueChanged(selectedSegmentIndex));
        private ICommand goToChatDetailsPageCommand;

        public ICommand GoToChatDetailsPageCommand =>
            goToChatDetailsPageCommand ?? (goToChatDetailsPageCommand = new Command<ChatPeopleModel>(async (selectedModel) => await ExecuteGoToChatDetailsPageCommand(selectedModel)));

        private ICommand _gotoProfilePageCommand;

        public ICommand GotoProfilePageCommand =>
            _gotoProfilePageCommand ?? (_gotoProfilePageCommand = new Command<string>(async (userName) => await ExecuteGotoProfilePageCommand(userName)));

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("ListType")) ListType = parameters.GetValue<ListTypes>("ListType");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}