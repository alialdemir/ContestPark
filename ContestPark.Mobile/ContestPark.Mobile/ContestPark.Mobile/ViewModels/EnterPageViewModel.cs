using ContestPark.Entities.Models;
using ContestPark.Mobile.Events;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Events;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class EnterPageViewModel : ViewModelBase<PostListModel>
    {
        #region Private variable

        private int _subCategoryId = 0;
        private readonly INavigationService _navigationService;
        private readonly IPostsService _PostsService;
        private readonly IDuelInfosService _duelInfosService;
        private readonly IFollowCategoryService _followCategoryService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDuelModule _duelModule;

        #endregion Private variable

        #region Constructors

        public EnterPageViewModel(INavigationService navigationService,
                                  IPostsService PostsService,
                                  IDuelInfosService duelInfosService,
                                  IFollowCategoryService followCategoryService,
                                  IEventAggregator eventAggregator,
                                  IDuelModule duelModule) : base(navigationService)
        {
            _navigationService = navigationService;
            _PostsService = PostsService;
            _duelInfosService = duelInfosService;
            _followCategoryService = followCategoryService;
            _eventAggregator = eventAggregator;
            _duelModule = duelModule;
            ServiceModel = new ServiceModel<PostListModel> { PageSize = 4 };
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Alt kategori takip etme durumu
        /// </summary>
        private bool _isSubCategoryFollowUpStatus;

        public bool IsSubCategoryFollowUpStatus
        {
            get { return _isSubCategoryFollowUpStatus; }
            set { SetProperty(ref _isSubCategoryFollowUpStatus, value); }
        }

        /// <summary>
        /// İlgili kategoriyi takip eden kişi sayısı
        /// </summary>
        private int _followersCount = 0;

        public int FollowersCount
        {
            get { return _followersCount; }
            set { SetProperty(ref _followersCount, value, nameof(FollowersCount)); }
        }

        /// <summary>
        /// Sorulan soru yüzdesi
        /// </summary>
        //public decimal SolvedQuestionsCount { get; set; }
        private string _subCategoryPicturePath;

        public string SubCategoryPicturePath
        {
            get { return _subCategoryPicturePath; }
            set { SetProperty(ref _subCategoryPicturePath, value, nameof(SubCategoryPicturePath)); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Kim ne yapıyor bilgisi çeker
        /// </summary>
        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ServiceModel = await _PostsService.ContestEnterScreenAsync(_subCategoryId, ServiceModel);
            await base.LoadItemsAsync();

            IsFollowUpStatusCommand.Execute(null);
            FollowersCountCommand.Execute(null);
            IsBusy = false;
        }

        /// <summary>
        /// Sorulan soru yüzdesi
        /// </summary>
        /// <returns>Sorulan soruların yüzdedel ifade</returns>
        //private async Task ExecuteSolvedQuestionsCommandAsync()
        //{
        //    //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
        //    //{
        //    SolvedQuestionsCount = await _duelInfosService.SolvedQuestionsAsync(_subCategoryId);
        //    IsBusy = false;
        //    // });
        //}
        /// <summary>
        /// Sorulan soru yüzdesi
        /// </summary>
        /// <returns>Yüzde</returns>
        private async Task ExecuteFollowersCountCommandAsync()
        {
            FollowersCount = await _followCategoryService.FollowersCountAsync(_subCategoryId);
        }

        /// <summary>
        /// Sosyal ağda paylaş methodu
        /// </summary>
        private void ExecuteShareCommandAsync()
        {
            _duelModule.SubCategoryShare(Title);
        }

        /// <summary>
        /// Alt kategori takip et takibi bırak methodu takip ediyorsa takibi bırakır takip etmiyorsa takip eder
        /// </summary>
        private async Task ExecuteSubCategoryFollowProgcessCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            bool isOk = await _followCategoryService.SubCategoryFollowProgcess(_subCategoryId, IsSubCategoryFollowUpStatus);
            if (isOk)
            {
                IsSubCategoryFollowUpStatus = !IsSubCategoryFollowUpStatus;
                if (IsSubCategoryFollowUpStatus) FollowersCount++;
                else FollowersCount--;
                _eventAggregator
                            .GetEvent<SubCategoryRefleshEvent>()
                            .Publish();
            }
            IsBusy = false;
        }

        /// <summary>
        /// Düello bahis panelini aç
        /// </summary>
        private async Task ExecuteduelOpenPanelCommandAsync()
        {
            await PushPopupPageAsync(nameof(DuelBettingPopupPage), new NavigationParameters
            {
                { "SubCategoryId", _subCategoryId }
            });
        }

        /// <summary>
        /// Sıralama sayfasına git
        /// </summary>
        private async Task ExecuteGoToRankingPageCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            await PushNavigationPageAsync(nameof(RankingPage), new NavigationParameters
            {
                {"SubCategoryId", _subCategoryId },
                {"SubCategoryName", Title },
                {"ListType", RankingPageViewModel.ListTypes.ScoreRanking },
            });
            IsBusy = false;
        }

        /// <summary>
        /// İlgili kategoriyi takip etme durumu
        /// </summary>
        private async Task ExecuteIsFollowUpStatusCommandAsync()
        {
            IsSubCategoryFollowUpStatus = await _followCategoryService.IsFollowUpStatusAsync(_subCategoryId);
        }

        #endregion Methods

        #region Commands

        //private ICommand solvedQuestionsCommand;
        ///// <summary>
        ///// Sorulan sayı command
        ///// </summary>
        //public ICommand SolvedQuestionsCommand
        //{
        //    get { return solvedQuestionsCommand ?? (solvedQuestionsCommand = new Command(() => ExecuteSolvedQuestionsCommandAsync())); }
        //}
        private ICommand followersCountCommand;

        /// <summary>
        /// Alt kategori takipçi sayısı command
        /// </summary>
        public ICommand FollowersCountCommand
        {
            get { return followersCountCommand ?? (followersCountCommand = new Command(() => ExecuteFollowersCountCommandAsync())); }
        }

        private ICommand shareCommand;

        /// <summary>
        /// Sosyal ağda paylaş command
        /// </summary>
        public ICommand ShareCommand
        {
            get { return shareCommand ?? (shareCommand = new Command(() => ExecuteShareCommandAsync())); }
        }

        private ICommand subCategoryFollowProgcessCommand;

        /// <summary>
        /// Alt kategori takip etme veya takip bırakma command
        /// </summary>
        public ICommand SubCategoryFollowProgcessCommand
        {
            get { return subCategoryFollowProgcessCommand ?? (subCategoryFollowProgcessCommand = new Command(async () => await ExecuteSubCategoryFollowProgcessCommandAsync())); }
        }

        private ICommand isFollowUpStatusCommand;

        /// <summary>
        /// Takip etme durmunu çalıştır command
        /// </summary>
        public ICommand IsFollowUpStatusCommand
        {
            get { return isFollowUpStatusCommand ?? (isFollowUpStatusCommand = new Command(async () => await ExecuteIsFollowUpStatusCommandAsync())); }
        }

        private ICommand goToRankingPageCommand;

        /// <summary>
        /// Sıralama sayfasına git command
        /// </summary>
        public ICommand GoToRankingPageCommand
        {
            get { return goToRankingPageCommand ?? (goToRankingPageCommand = new Command(async () => await ExecuteGoToRankingPageCommandAsync())); }
        }

        private ICommand duelOpenPanelCommand;

        /// <summary>
        /// Düello bahis paneli aç command
        /// </summary>
        public ICommand DuelOpenPanelCommand
        {
            get { return duelOpenPanelCommand ?? (duelOpenPanelCommand = new Command(async () => await ExecuteduelOpenPanelCommandAsync())); }
        }

        /// <summary>
        /// Düello bahis paneli aç command
        /// </summary>
        public INavigationService NavigationService
        {
            get { return _navigationService; }
        }

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("SubCategoryName")) Title = parameters.GetValue<string>("SubCategoryName");
            if (parameters.ContainsKey("SubCategoryPicturePath")) SubCategoryPicturePath = parameters.GetValue<string>("SubCategoryPicturePath");
            if (parameters.ContainsKey("SubCategoryId")) _subCategoryId = parameters.GetValue<int>("SubCategoryId");

            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}