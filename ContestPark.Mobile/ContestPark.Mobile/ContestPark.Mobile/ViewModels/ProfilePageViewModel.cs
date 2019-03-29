using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Components;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static ContestPark.Mobile.ViewModels.FollowsPageViewModel;

namespace ContestPark.Mobile.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase<PostListModel>
    {
        #region Private variable

        private readonly INavigationService _navigationService;
        private readonly IPostsService _PostsService;
        private readonly IUserDataModule _userDataModule;
        private readonly IAccountService _accountService;
        private readonly IDuelInfosService _duelInfosService;
        private readonly IFollowsService _followsService;

        #endregion Private variable

        #region Constructors

        public ProfilePageViewModel(INavigationService navigationService,
                                    IPostsService PostsService,
                                    IUserDataModule userDataModule,
                                    IAccountService accountService,
                                    IDuelInfosService duelInfosService,
                                    IFollowsService followsService) : base(navigationService)
        {
            Title = ContestParkResources.Profile;
            _navigationService = navigationService;
            _PostsService = PostsService;
            _userDataModule = userDataModule;
            _accountService = accountService;
            _duelInfosService = duelInfosService;
            _followsService = followsService;
        }

        #endregion Constructors

        #region Properties

        private bool _isVisibleBackArrow = true;

        public bool IsVisibleBackArrow
        {
            get { return _isVisibleBackArrow; }
            set { SetProperty(ref _isVisibleBackArrow, value); }
        }

        private bool _isFollow;

        public bool IsFollow
        {
            get { return _isFollow; }
            set { SetProperty(ref _isFollow, value, nameof(IsFollow)); }
        }

        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Private backing field to hold the gameCount
        /// </summary>
        private int _gameCount = 0;

        /// <summary>
        /// Public property to set and get the GameCount of the item
        /// </summary>
        public int GameCount
        {
            get { return _gameCount; }
            set { SetProperty(ref _gameCount, value, nameof(GameCount)); }
        }

        /// <summary>
        /// Private backing field to hold the followersCount
        /// </summary>
        private int _followersCount = 0;

        /// <summary>
        /// Kullanıcının takipçi sayısı
        /// </summary>
        public int FollowersCount
        {
            get { return _followersCount; }
            set { SetProperty(ref _followersCount, value, nameof(FollowersCount)); }
        }

        /// <summary>
        /// Private backing field to hold the followUpCount
        /// </summary>
        private int _followUpCount = 0;

        /// <summary>
        /// Kullanıcının takipçi sayısı
        /// </summary>
        public int FollowUpCount
        {
            get { return _followUpCount; }
            set { SetProperty(ref _followUpCount, value, nameof(FollowUpCount)); }
        }

        private UserProfilePageModel _profileInfo = new UserProfilePageModel();

        public UserProfilePageModel ProfileInfo
        {
            get { return _profileInfo; }
            set { SetProperty(ref _profileInfo, value, nameof(ProfileInfo)); }
        }

        #endregion Properties

        #region Methods

        protected override Task LoadItemsAsync()
        {
            if (IsBusy)
                return Task.FromResult(false);

            IsBusy = true;
            LoadUserProfileCommand.Execute(null);
            LoadPostListCommand.Execute(null);
            IsBusy = false;

            return Task.FromResult(false);
        }

        /// <summary>
        /// Profilin üst kısmındaki bilgileri yükler
        /// </summary>
        private async Task LoadUserProfileAsync()
        {
            if (String.IsNullOrEmpty(UserName))
                return;

            ProfileInfo = await _accountService.UserProfileAsync(UserName);

            LoadGameCountCommand.Execute(null);
            LoadFollowersCountCommand.Execute(null);
            LoadFollowUpCountCommand.Execute(null);
            if (UserName != _userDataModule.UserModel.UserName) FollowStatusCommand.Execute(null);
        }

        /// <summary>
        /// Kullanıcının Post listesini çeker
        /// </summary>
        private async Task LoadPostListAsync()
        {
            if (!String.IsNullOrEmpty(UserName))
            {
                ServiceModel = await _PostsService.PostListAsync(UserName, ServiceModel);
                base.LoadItemsAsync();
            }
        }

        /// <summary>
        /// Yapılan toplam deüllo sayısı
        /// </summary>
        private async Task LoadGameCountAsync()
        {
            if (ProfileInfo != null && !String.IsNullOrEmpty(ProfileInfo.UserId))
                GameCount = await _duelInfosService.GameCountAsync(ProfileInfo.UserId);
        }

        /// <summary>
        /// Takip edenlerin sayısı
        /// </summary>
        private async Task LoadFollowersCountAsync()
        {
            if (ProfileInfo != null && !String.IsNullOrEmpty(ProfileInfo.UserId))
                FollowersCount = await _followsService.FollowersCountAsync(ProfileInfo.UserId);
        }

        /// <summary>
        /// Takip ettiklerim sayısı
        /// </summary>
        private async Task LoadFollowUpCountAsync()
        {
            if (ProfileInfo != null && !String.IsNullOrEmpty(ProfileInfo.UserId))
                FollowUpCount = await _followsService.FollowUpCountAsync(ProfileInfo.UserId);
        }

        /// <summary>
        /// listType göre takip edilenler/ takip ettiklerim sayfasını açar
        /// </summary>
        /// <param name="listType">FollowsPage listeleme tipi</param>
        private async Task GotoFollowsAsync(ListTypes listType)
        {
            await PushNavigationPageAsync(nameof(FollowsPage), new NavigationParameters
            {
                {"ListType",listType },
                {"FollowedUserId", ProfileInfo.UserId }
            });
        }

        /// <summary>
        /// modalName göre modal açar
        /// </summary>
        /// <param name="modalName">Açılacak modalda gösterilecek resim</param>
        private async Task GotoPhotoModalPageAsync(string modalName)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (modalName == "Profile")
            {
                await PushModalAsync(nameof(PhotoModalView), new NavigationParameters
                {
                    {
                        "UserPictureList", new UserPictureListModel
                                                                {
                                                                    PicturePath = ProfileInfo.UserProfilePicturePath,
                                                                    PictureTypeId  = 1
                                                                }
                    }
                });
            }
            else if (modalName == "Cover")
            {
                await PushModalAsync(nameof(PhotoModalView), new NavigationParameters
                {
                    {
                        "UserPictureList", new UserPictureListModel
                                                                {
                                                                    PicturePath = ProfileInfo.CoverPicture,
                                                                    PictureTypeId=2
                                                                }
                    }
                });
            }
            IsBusy = false;
        }

        /// <summary>
        /// Login olan kullanıcı profili açılan kullanıcıyı takip ediyor mu
        /// </summary>
        private async Task FollowStatusAsync()
        {
            if (ProfileInfo != null && !String.IsNullOrEmpty(ProfileInfo.UserId))
                IsFollow = await _followsService.IsFollowUpStatusAsync(ProfileInfo.UserId);
        }

        private async Task FollowProcesAsync()
        {
            if (IsBusy || UserName == _userDataModule.UserModel.UserName)
                return;

            IsBusy = true;
            bool isSuccess;
            if (IsFollow) isSuccess = await _followsService.UnFollowAsync(ProfileInfo.UserId);
            else isSuccess = await _followsService.FollowUpAsync(ProfileInfo.UserId);

            if (isSuccess)
                IsFollow = !IsFollow;
            IsBusy = false;
        }

        /// <summary>
        /// ChatDetailsPage sayfasına gönderir
        /// </summary>
        private async Task GotoChatPageAsync()
        {
            await PushNavigationPageAsync(nameof(ChatDetailsPage), new NavigationParameters
            {
                { "UserName", UserName },
                { "UserFullName", ProfileInfo.FullName },
                { "SenderUserId", ProfileInfo.UserId }
            });
        }

        #endregion Methods

        #region Commands

        private ICommand LoadUserProfileCommand => new Command(async () => await LoadUserProfileAsync());
        private ICommand LoadPostListCommand => new Command(async () => await LoadPostListAsync());
        private ICommand LoadGameCountCommand => new Command(async () => await LoadGameCountAsync());
        private ICommand LoadFollowersCountCommand => new Command(async () => await LoadFollowersCountAsync());
        private ICommand LoadFollowUpCountCommand => new Command(async () => await LoadFollowUpCountAsync());
        private ICommand FollowStatusCommand => new Command(async () => await FollowStatusAsync());
        public ICommand GotoFollowsCommand => new Command<string>(async (listTypes) => await GotoFollowsAsync((ListTypes)Convert.ToInt32(listTypes)));
        public ICommand GotoPhotoModalPageCommand => new Command<string>(async (listTypes) => await GotoPhotoModalPageAsync(listTypes));
        public ICommand GotoBackCommand => new Command(() => GoBackAsync());
        public ICommand FollowProcesCommand => new Command(async () => await FollowProcesAsync());
        public ICommand GotoChatPageCommand => new Command(async () => await GotoChatPageAsync());
        public INavigationService NavigationService { get { return _navigationService; } }

        #endregion Commands

        #region Navigations

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("UserName")) UserName = parameters.GetValue<string>("UserName");
            if (parameters.ContainsKey("IsVisibleBackArrow")) IsVisibleBackArrow = parameters.GetValue<bool>("IsVisibleBackArrow");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigations
    }
}