using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class DuelResultPageViewModel : ViewModelBase<DuelQuestionModel>
    {
        #region Enum

        public enum ListTypes
        {
            DuelResult = 1,
            Questions = 2
        }

        #endregion Enum

        #region Private variables

        private int _duelId = 0, _subCategoryId = 0;
        private byte _competitorScore;
        private string WinnerColor = "#017d46";// Kazananın border-color renği #yeşil
        private string LostColor = "#993232";// Kaybedenin border-color renği #kırmızı '#008000'; kırmızı '#f53d3d';

        //private string DrawColor = "#ffc900";
        private readonly IDuelInfosService _duelInfosService;

        private readonly IUserDataModule _userDataModule;
        private readonly IQuestionsService _questionsService;
        private readonly IScoresService _scoresService;

        #endregion Private variables

        #region Constroctor

        public DuelResultPageViewModel(INavigationService navigationService,
                                       IDuelInfosService duelInfosService,
                                       IUserDataModule userDataModule,
                                       IQuestionsService questionsService,
                                       IScoresService scoresService) : base(navigationService)
        {
            _duelInfosService = duelInfosService;
            _userDataModule = userDataModule;
            _questionsService = questionsService;
            _scoresService = scoresService;
        }

        #endregion Constroctor

        #region Properties

        public ListTypes ListType { get; set; } = ListTypes.DuelResult;
        private string _winnerFullName = string.Empty;

        public string WinnerFullName
        {
            get { return _winnerFullName; }
            set { SetProperty(ref _winnerFullName, value, nameof(WinnerFullName)); }
        }

        private string _founderColor = "#ffc900";

        public string FounderColor
        {
            get { return _founderColor; }
            set { SetProperty(ref _founderColor, value, nameof(FounderColor)); }
        }

        private string _competitorColor = "#ffc900";

        public string CompetitorColor
        {
            get { return _competitorColor; }
            set { SetProperty(ref _competitorColor, value, nameof(CompetitorColor)); }
        }

        public bool IsNavBarShow { get; set; }
        private DuelResultModel _duelResultModel;

        /// <summary>
        /// Listview içerisine bind edilecek liste
        /// </summary>
        public DuelResultModel DuelResultModel
        {
            get { return _duelResultModel ?? (_duelResultModel = new DuelResultModel()); }
            set { SetProperty(ref _duelResultModel, value, nameof(DuelResultModel)); }
        }

        private DuelResultRankingModel _duelResultRankingModel;

        public DuelResultRankingModel DuelResultRankingModel
        {
            get { return _duelResultRankingModel; }
            set { SetProperty(ref _duelResultRankingModel, value, nameof(DuelResultRankingModel)); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            DuelResultModel = await _duelInfosService.DuelResultAsync(_duelId, _subCategoryId);
            if (_competitorScore != 0) DuelResultModel.CompetitorTrueAnswerCount = _competitorScore;

            WriteWinnerFullName();//Kazananı yaz
            QuestionsCommand.Execute(null);
            await SetDuelResultRankingModelAsync();//Düello sonucunu propertiese ekler
            IsBusy = false;
        }

        /// <summary>
        /// Düello sonucuna göre kazanan kişinin adını yazar
        /// Kendi kazandıysa kazandın
        /// yada kaybettiyle kaybettin yazar
        /// </summary>
        private void WriteWinnerFullName()
        {
            string userName = _userDataModule.UserModel.UserName;
            // Kazanan kişiyi yazmak için
            if (DuelResultModel.FounderEscapeStatus || DuelResultModel.CompetitorEscapeStatus)
            {
                if (userName == DuelResultModel.CompetitorUserName && DuelResultModel.CompetitorEscapeStatus == true) WinnerFullName = ContestParkResources.YouWin;
                else WinnerFullName = ContestParkResources.YouLose;
                if (userName == DuelResultModel.FounderUserName && DuelResultModel.FounderEscapeStatus == true) WinnerFullName = ContestParkResources.YouLose;
                else WinnerFullName = ContestParkResources.YouWin;
            }
            else if (DuelResultModel.FounderScorePoint == 0 && (DuelResultModel.FounderNullAnswerCount || DuelResultModel.CompetitorNullAnswerCount))
                WinnerFullName = ContestParkResources.CompetitorsIsExpectedResistDuel;
            else if (DuelResultModel.FounderScorePoint > DuelResultModel.CompetitorTrueAnswerCount)
            {
                if (userName == DuelResultModel.FounderUserName) WinnerFullName = ContestParkResources.YouWin;
                else if (userName == DuelResultModel.CompetitorUserName) WinnerFullName = ContestParkResources.YouLose;
                else WinnerFullName = ContestParkResources.Winning + " " + DuelResultModel.FounderFullName;

                FounderColor = WinnerColor;
                CompetitorColor = LostColor;
            }
            else if (DuelResultModel.FounderTrueAnswerCount < DuelResultModel.CompetitorTrueAnswerCount)
            {
                if (userName == DuelResultModel.FounderUserName) WinnerFullName = ContestParkResources.YouLose;
                else if (userName == DuelResultModel.CompetitorUserName) WinnerFullName = ContestParkResources.YouWin;
                else WinnerFullName = ContestParkResources.Winning + " " + DuelResultModel.CompetitorFullName;

                FounderColor = LostColor;
                CompetitorColor = WinnerColor;
            }
            else WinnerFullName = ContestParkResources.Draw;
        }

        /// <summary>
        /// Düello sonucunu propertiese ekler
        /// </summary>
        /// <returns></returns>
        private async Task SetDuelResultRankingModelAsync()
        {
            DuelResultRankingModel = await _scoresService.DuelResultRankingAsync(_subCategoryId);
        }

        private async Task ExecuteQuestionsCommandAsync()
        {
            var items = await _questionsService.DuelQuestionsAsync(_duelId, _subCategoryId);
            if (items != null) Items.AddRange(items);
        }

        /// <summary>
        /// Rövanş oyna
        /// </summary>
        /// <returns></returns>
        private void ExecuteRevengeCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            string userId = _userDataModule.UserModel.UserName == DuelResultModel.FounderUserName ? DuelResultModel.CompetitorUserId : DuelResultModel.FounderUserId;
            //  MessagingCenter.Send(new MessagingCenterPush(new DuelEnterScreenPage(userId, 0, _subCategoryId, false)), "MessagingCenterPush");
            IsBusy = false;
        }

        /// <summary>
        /// Sohbete git
        /// </summary>
        /// <returns></returns>
        private void ExecuteChatCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            string userName = _userDataModule.UserModel.UserName;
            if (userName == DuelResultModel.FounderUserName)
            {
                PushNavigationPageAsync(nameof(ChatDetailsPage), new NavigationParameters
                {
                    {"UserName", DuelResultModel.CompetitorUserName },
                    {"UserFullName", DuelResultModel.CompetitorFullName },
                    {"SenderUserId", DuelResultModel.CompetitorUserId }
                });
            }
            else if (userName == DuelResultModel.CompetitorUserName)
            {
                PushNavigationPageAsync(nameof(ChatDetailsPage), new NavigationParameters
                {
                    {"UserName", DuelResultModel.FounderUserName },
                    {"UserFullName", DuelResultModel.FounderFullName },
                    {"SenderUserId", DuelResultModel.FounderUserId }
                });
            }
            IsBusy = false;
        }

        public async Task ExecuteGotoBackPageCommandAsync()
        {
            await GoBackAsync();
        }

        /// <summary>
        /// Başka rakip bul
        /// </summary>
        /// <returns></returns>
        private void ExecuteOtherOpponenteCommand()
        {
            //MessagingCenter.Send(new MessagingCenterPush(new DuelEnterScreenPage("", 0, _subCategoryId, false)), "MessagingCenterPush");
        }

        private async Task ExecuteGotoProfilePageCommandAsync(string userName)
        {
            await PushNavigationPageAsync($"{nameof(ProfilePage)}?UserName={userName}");
        }

        #endregion Methods

        #region Commands

        private Command<string> gotoProfilePageCommand;

        /// <summary>
        /// Go to ProfilePage load command
        /// </summary>
        public Command<string> GotoProfilePageCommand
        {
            get { return gotoProfilePageCommand ?? (gotoProfilePageCommand = new Command<string>(async (userName) => await ExecuteGotoProfilePageCommandAsync(userName))); }
        }

        private ICommand revengeCommand;

        /// <summary>
        /// Rövanş command
        /// </summary>
        public ICommand RevengeCommand
        {
            get { return revengeCommand ?? (revengeCommand = new Command(() => ExecuteRevengeCommand())); }
        }

        private ICommand chatCommand;

        /// <summary>
        /// Sohbete git command
        /// </summary>
        public ICommand ChatCommand
        {
            get { return chatCommand ?? (chatCommand = new Command(() => ExecuteChatCommand())); }
        }

        private ICommand otherOpponenteCommand;

        /// <summary>
        /// Başka rakip bul command
        /// </summary>
        public ICommand OtherOpponenteCommand
        {
            get { return otherOpponenteCommand ?? (otherOpponenteCommand = new Command(() => ExecuteOtherOpponenteCommand())); }
        }

        private ICommand questionsCommand;

        /// <summary>
        /// Başka rakip bul command
        /// </summary>
        public ICommand QuestionsCommand
        {
            get { return questionsCommand ?? (questionsCommand = new Command(async () => await ExecuteQuestionsCommandAsync())); }
        }

        private ICommand gotoBackPageCommand;

        /// <summary>
        /// Başka rakip bul command
        /// </summary>
        public ICommand GotoBackPageCommand
        {
            get { return gotoBackPageCommand ?? (gotoBackPageCommand = new Command(async () => await ExecuteGotoBackPageCommandAsync())); }
        }

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("DuelId")) _duelId = parameters.GetValue<int>("DuelId");
            if (parameters.ContainsKey("SubCategoryId")) _subCategoryId = parameters.GetValue<int>("SubCategoryId");
            if (parameters.ContainsKey("IsNavBarShow")) IsNavBarShow = parameters.GetValue<bool>("IsNavBarShow");
            if (parameters.ContainsKey("CompetitorScore")) _competitorScore = parameters.GetValue<byte>("CompetitorScore");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}