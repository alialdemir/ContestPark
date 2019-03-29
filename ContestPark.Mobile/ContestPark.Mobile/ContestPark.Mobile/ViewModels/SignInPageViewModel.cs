using ContestPark.Entities.Models;
using ContestPark.Mobile.Components;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        #region Private variables

        private readonly IAccountService _accountService;

        #endregion Private variables

        #region Constructors

        public SignInPageViewModel(INavigationService navigationService,
                                   IAccountService accountService) : base(navigationService)
        {
            _accountService = accountService;
            LoginModel.UserName = "witcherfearless";
            LoginModel.Password = "19931993";
        }

        #endregion Constructors

        #region Properties

        private LoginModel _loginModel = new LoginModel();

        public LoginModel LoginModel
        {
            get { return _loginModel; }
            set { SetProperty(ref _loginModel, value, nameof(LoginModel)); }
        }

        #endregion Properties

        #region Methods

        private async Task ExecuteSignUpCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            await PushModalAsync($"{nameof(BaseNavigationPage)}/{nameof(SignUpPage)}");
            IsBusy = false;
        }

        private async Task ExecuteForgetYourPasswordCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            await PushModalAsync($"{nameof(BaseNavigationPage)}/{nameof(ForgetYourPasswordPage)}");
            IsBusy = false;
        }

        private async Task ExecuteLoginCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            bool isOK = await _accountService.LoginProgcessAsync(LoginModel);
            if (isOK) await PushNavigationPageAsync($"app:///{nameof(MainPage)}?appModuleRefresh=OnInitialized");
            IsBusy = false;
        }

        private void ExecuteFacebookWithLoginCommand()
        {
            ContestParkApp.Current.MainPage.Navigation.PushModalAsync(new SignInSocialNetworkPage(SignInSocialNetworkPage.SocialNetworkTypes.Facebook)
            {
                CompletedCommand = new Command<string>((accessToken) =>
                {
                }),
                ErrorCommand = new Command<string>((errror) =>
                {
                })
            });
        }

        #endregion Methods

        #region Commands

        public ICommand SignUpCommand => new Command(async () => await ExecuteSignUpCommand());
        public ICommand LoginCommand => new Command(async () => await ExecuteLoginCommandAsync());
        public ICommand FacebookWithLoginCommand => new Command(() => ExecuteFacebookWithLoginCommand());
        public ICommand ForgetYourPasswordCommand => new Command(async () => await ExecuteForgetYourPasswordCommand());

        #endregion Commands
    }
}