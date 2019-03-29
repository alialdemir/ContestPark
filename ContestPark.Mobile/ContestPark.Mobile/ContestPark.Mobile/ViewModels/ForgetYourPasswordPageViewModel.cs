using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class ForgetYourPasswordPageViewModel : ViewModelBase
    {
        #region Private variables

        private readonly IAccountService _accountService;

        #endregion Private variables

        #region Constructors

        public ForgetYourPasswordPageViewModel(INavigationService navigationService,
                                               IPageDialogService dialogService,
                                               IAccountService accountService) : base(navigationService, dialogService)
        {
            _accountService = accountService;
            Title = ContestParkResources.ForgotYourPassword;
        }

        #endregion Constructors

        #region Properties

        private string _uerNameOrEmail = String.Empty;

        public string UserNameOrEmail
        {
            get { return _uerNameOrEmail; }
            set { SetProperty(ref _uerNameOrEmail, value); }
        }

        #endregion Properties

        #region Methods

        private async Task ExecuteForgetYourPasswordCommandAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (string.IsNullOrEmpty(UserNameOrEmail))
            {
                await DisplayAlertAsync(ContestParkResources.Error,
                                        ContestParkResources.ForgetYourPasswordLabel1,
                                        ContestParkResources.Okay);
                IsBusy = false;
                return;
            }
            var result = await _accountService.ForgotYourPasswordAsync(UserNameOrEmail);
            if (result)
            {
                UserNameOrEmail = String.Empty;
                await DisplayAlertAsync(ContestParkResources.Success,
                                        ContestParkResources.ServerMessage_userInfoSendMailAdress,
                                        ContestParkResources.Okay);
            }
            IsBusy = false;
        }

        #endregion Methods

        #region Command

        public ICommand ForgetYourPasswordCommand => new Command(async () => await ExecuteForgetYourPasswordCommandAsync());

        #endregion Command
    }
}