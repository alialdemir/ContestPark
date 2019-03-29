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
    public class RankingPageViewModel : ViewModelBase<ScoreRankingModel>
    {
        #region Enum

        public enum ListTypes
        {
            ScoreRanking = 0,
            ScoreRankingFollowing = 1
        }

        #endregion Enum

        #region Private variables

        private int _subCategoryId = 0;
        private readonly IScoresService _scoresService;

        #endregion Private variables

        #region Constructors

        public RankingPageViewModel(INavigationService navigationService,
                                    IScoresService scoresService) : base(navigationService)
        {
            _scoresService = scoresService;
        }

        #endregion Constructors

        #region Properties

        public ListTypes ListType { get; set; }
        private string _rankEmptyMessage;

        public string RankEmptyMessage
        {
            get { return _rankEmptyMessage; }
            set { SetProperty(ref _rankEmptyMessage, value, nameof(RankEmptyMessage)); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (ListTypes.ScoreRanking.HasFlag(ListType)) ServiceModel = await _scoresService.ScoreRankingAsync(_subCategoryId, ServiceModel);
            else if (ListTypes.ScoreRankingFollowing.HasFlag(ListType)) ServiceModel = await _scoresService.ScoreRankingFollowingAsync(_subCategoryId, ServiceModel);
            await base.LoadItemsAsync();
            LoadRankEmptyMessage();
            IsBusy = false;
        }

        /// <summary>
        /// ListTypes göre listview boş olduğunda çıkacak mesajı ayarlar
        /// </summary>
        public void LoadRankEmptyMessage()
        {
            switch (ListType)
            {
                case ListTypes.ScoreRanking: RankEmptyMessage = ContestParkResources.ThisCategoryRankNull; break;
                case ListTypes.ScoreRankingFollowing: RankEmptyMessage = ContestParkResources.RankFollowingNull; break;
            }
        }     /// <summary>

        /// Segmente tıklanınca listeleme tipi değiştir
        /// </summary>
        /// <param name="value">seçilen segment id</param>
        private void SegmentValueChanged(int selectedSegmentIndex)
        {
            if (!IsInitialized)
                return;

            ListType = selectedSegmentIndex == 0 ? ListTypes.ScoreRanking : ListTypes.ScoreRankingFollowing;
            LoadRankEmptyMessage();
            RefleshCommand.Execute(null);
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

        public ICommand SegmentValueChangedCommand => new Command<int>((selectedSegmentIndex) => SegmentValueChanged(selectedSegmentIndex));
        private ICommand _gotoProfilePageCommand;

        public ICommand GotoProfilePageCommand =>
            _gotoProfilePageCommand ?? (_gotoProfilePageCommand = new Command<string>(async (userName) => await ExecuteGotoProfilePageCommand(userName)));

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("SubCategoryId")) _subCategoryId = parameters.GetValue<int>("SubCategoryId");
            if (parameters.ContainsKey("SubCategoryName")) Title = parameters.GetValue<string>("SubCategoryName");
            if (parameters.ContainsKey("ListType")) ListType = parameters.GetValue<ListTypes>("ListType");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}