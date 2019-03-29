using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class DuelEnterScreenPageViewModel : ViewModelBase
    {
        #region Enums

        public enum StandbyModes
        {
            On,
            Off
        }

        #endregion Enums

        #region Private Variables

        private readonly IDuelSignalRService _duelSignalRService;
        private readonly IDuelInfosService _duelInfosService;
        private readonly IPicturesService _picturesService;
        private int _subCategoryId;
        private int _bet;
        private StandbyModes StandbyMode;

        #endregion Private Variables

        #region Constructors

        public DuelEnterScreenPageViewModel(INavigationService navigationService,
                                            IUserDataModule userDataModule,
                                            IDuelSignalRService duelSignalRService,
                                            IDuelInfosService duelInfosService,
                                            IPicturesService picturesService) : base(navigationService)
        {
            _duelSignalRService = duelSignalRService;
            _duelInfosService = duelInfosService;
            _picturesService = picturesService;
            DuelScreen = new DuelEnterScreenModel()
            {
                FounderFullName = userDataModule.UserModel.FullName,
                FounderCoverPicturePath = userDataModule.UserModel.UserCoverPicturePath,
                FounderProfilePicturePath = userDataModule.UserModel.UserProfilePicturePath
            };
            StartHub();// SignalR event load
        }

        #endregion Constructors

        #region Properties

        public DuelEnterScreenModel DuelScreen { get; private set; }
        public bool RandomPicturStatus { get; set; } = true;

        #endregion Properties

        #region SignalR

        private void StartHub()
        {
            _duelSignalRService.OnDataReceived += (sender, e) => SetDuelScreen(e);//  Signalr Tarafından gelen data
            _duelSignalRService.GoToDuelScreenProxy();
        }

        #endregion SignalR

        #region Methods

        protected override Task LoadItemsAsync()
        {
            if (!StandbyModes.Off.HasFlag(StandbyMode))
            {
            }
            else
            {
                RandomUserProfilePicturesCommand.Execute(null);
                DuelOpenCommand.Execute(null);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Rakip araken bekleme ekranı için gerekli işlemler
        /// </summary>
        private async Task RandomUserProfilePicturesAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            DuelScreen.CompetitorFullName = ContestParkResources.AwaitingOpponent;
            await RandomPictures();
            FindOpponent();
            IsBusy = false;
        }

        /// <summary>
        /// Rakip arar
        /// </summary>
        private void FindOpponent()
        {
            Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 0, 4, 0), () =>
            {
                if (!RandomPicturStatus) return false;

                Task.Run(async () =>
                {
                    DuelEnterScreenModel result = await _duelInfosService.DuelPlayAsync();
                    SetDuelScreen(result);

                    Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 0, 5, 0), () =>
                    {
                        if (!RandomPicturStatus) return false;
                        RandomPicturStatus = false;
                        PopupGoBackAsync();
                        PushPopupPageAsync($"{nameof(QuizPage)}?DuelId={DuelScreen.DuelId}");

                        return false;
                    });
                });
                return false;
            });
        }

        /// <summary>
        /// Bekleme modundayken rakip profil resmini değiştir
        /// </summary>
        private async Task RandomPictures()
        {
            ServiceModel<string> serviceModel = await _picturesService.RandomUserProfilePicturesAsync(new PagingModel() { PageSize = 70 });
            string[] pictures = serviceModel.Items.ToArray();
            int pictureIndex = pictures.Length - 1;
            Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 0, 0, 500), () =>
            {
                if (pictureIndex > 0)
                {
                    DuelScreen.CompetitorProfilePicturePath = pictures[pictureIndex];
                    pictureIndex--;
                    if (pictureIndex <= 0) pictureIndex = pictures.Length - 1;
                }
                return RandomPicturStatus;
            });
        }

        /// <summary>
        /// Rakip bekliyor moduna aldık
        /// </summary>
        private async Task DuelOpenRandom()
        {
            await _duelInfosService.OpenDuel(_subCategoryId, _bet);// success kontrol et hata oluşursa mesaj çıksın
        }

        /// <summary>
        /// Ekrandaki bilgileri yeniler
        /// </summary>
        /// <param name="duelEnterScreenModel"></param>
        private void SetDuelScreen(DuelEnterScreenModel duelEnterScreenModel)
        {
            if (duelEnterScreenModel != null)
            {
                RandomPicturStatus = false;
                AnimationCommand.Execute(null);
                DuelScreen = duelEnterScreenModel;
            }
        }

        /// <summary>
        /// Kullanıcı bekleme modundan çıkmak isterse alert ile sorma ve sunucudan bekleme modundan çıkarma
        /// </summary>
        /// <param name="popupPage">Page tarafından çağrılcağı için kapatılacak popup sayfası</param>
        private async Task DuelCloseRandom()
        {
            bool isOk = await DisplayAlertAsync(String.Empty,
                                                ContestParkResources.DoYouWantToToQuit,
                                                ContestParkResources.Exit,
                                                ContestParkResources.Cancel);
            if (isOk)
            {
                RandomPicturStatus = false;
                await _duelInfosService.CloseDuelAsync();
                await PopupGoBackAsync();
            }
        }

        #endregion Methods

        #region Commands

        private ICommand RandomUserProfilePicturesCommand => new Command(async () => await RandomUserProfilePicturesAsync());
        private ICommand DuelOpenCommand => new Command(async () => await DuelOpenRandom());
        public ICommand DuelCloseCommand => new Command(async () => await DuelCloseRandom());
        public ICommand AnimationCommand;

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("SubCategoryId")) _subCategoryId = parameters.GetValue<int>("SubCategoryId");
            if (parameters.ContainsKey("Bet")) _bet = parameters.GetValue<int>("Bet");
            if (parameters.ContainsKey("StandbyMode")) StandbyMode = parameters.GetValue<StandbyModes>("StandbyMode");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}