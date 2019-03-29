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
    public class DuelBettingPopupPageViewModel : ViewModelBase
    {
        #region Private varaibles

        private readonly ICpService _CpService;

        #endregion Private varaibles

        #region Constructors

        public DuelBettingPopupPageViewModel(INavigationService navigationService,
                                             ICpService CpService) : base(navigationService)
        {
            _CpService = CpService;
        }

        #endregion Constructors

        #region Properties

        private int _userCp = 0;
        private int _subCategoryId;

        /// <summary>
        /// Kullanıcının altın miktarını tutar
        /// </summary>
        public int UserCp
        {
            get { return _userCp; }
            set { SetProperty(ref _userCp, value); }
        }

        /// <summary>
        /// Kullanıcının maksimum ne kadar arttırabileceği altın miktarını tutar
        /// </summary>
        private int MinCp { get; set; } = 0;

        private bool _increaseBetIsEnabled;

        public bool IncreaseBetIsEnabled
        {
            get { return _increaseBetIsEnabled; }
            set { SetProperty(ref _increaseBetIsEnabled, value); }
        }

        private bool _reduceBetIsVisible;

        public bool ReduceBetIsVisible
        {
            get { return _reduceBetIsVisible; }
            set { SetProperty(ref _reduceBetIsVisible, value); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            int userCp = await _CpService.UserTotalCp();
            if (userCp > 0) UserCp = userCp / 2;
            MinCp = userCp;
            if (UserCp <= 0) IncreaseBetIsEnabled = false;//Kullanıcının altını 0 ise azalt buttonu pasif olsun
            IsBusy = false;
        }

        private void ExecuteIncreaseBetCommand()
        {
            decimal total = Math.Round((decimal)((this.MinCp / 100) * 10));
            total = this.UserCp + total;
            if (total >= this.MinCp)
            {
                this.IncreaseBetIsEnabled = false;
                this.ReduceBetIsVisible = true;
                this.UserCp = this.MinCp;
            }
            else
            {
                this.UserCp = (int)total;
                this.IncreaseBetIsEnabled = true;
                this.ReduceBetIsVisible = true;
            }
        }

        private void ExecuteReduceBetCommand()
        {
            decimal total = Math.Round((decimal)((this.MinCp / 100) * 10));
            if (total > 0 && (this.UserCp - total) > total)
            {
                this.UserCp = this.UserCp - (int)total;
                IncreaseBetIsEnabled = true;
            }
            if (!(total > 0 && (this.UserCp - total) > total)) ReduceBetIsVisible = false;
        }

        private async Task ExecuteDuelStartCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            //CloseAllPopup();
            var parameters = new NavigationParameters
            {
                { "SubCategoryId", _subCategoryId },
                { "UserCp", UserCp },
                { "StandbyMode", DuelEnterScreenPageViewModel.StandbyModes.On },
            };
            await PushPopupPageAsync(nameof(DuelEnterScreenPage), parameters);
            //Navigation.PushPopupAsync(new DuelEnterScreenPage() { BindingContext = new DuelEnterScreenPageViewModel(_subCategoryId, UserCp, DuelEnterScreenPageViewModel.StandbyModes.On) { Navigation = Navigation } });
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        public ICommand IncreaseBetCommand
        {
            get
            {
                return new Command(() => ExecuteIncreaseBetCommand());
            }
        }

        public ICommand ReduceBetCommand
        {
            get
            {
                return new Command(() => ExecuteReduceBetCommand());
            }
        }

        public ICommand DuelStartCommand
        {
            get
            {
                return new Command(async () => await ExecuteDuelStartCommand());
            }
        }

        #endregion Commands

        #region Navigations

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("SubCategoryId")) _subCategoryId = parameters.GetValue<int>("SubCategoryId");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigations
    }
}