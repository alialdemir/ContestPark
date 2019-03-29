using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        #region Private variables

        private readonly IAccountService _accountService;

        #endregion Private variables

        #region Constructors

        public SignUpPageViewModel(INavigationService navigationService,
                                   IPageDialogService dialogService,
                                   IAccountService accountService) : base(navigationService, dialogService)
        {
            _accountService = accountService;
            Title = ContestParkResources.SignUp;
        }

        #endregion Constructors

        #region Properties

        private RegisterBindingModel _registerUserModel = new RegisterBindingModel();

        public RegisterBindingModel RegisterUserModel
        {
            get { return _registerUserModel; }
            set { SetProperty(ref _registerUserModel, value); }
        }

        #endregion Properties

        #region Methods

        private async Task ExecuteSignUpCommandAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            #region Validation

            string validationMessage = String.Empty;
            if (string.IsNullOrEmpty(RegisterUserModel.UserName)
                     || string.IsNullOrEmpty(RegisterUserModel.FullName)
                     || string.IsNullOrEmpty(RegisterUserModel.Email)
                     || string.IsNullOrEmpty(RegisterUserModel.Password)) validationMessage = ContestParkResources.RequiredFields;// Tüm alanlar boş ise
            else if (String.IsNullOrEmpty(RegisterUserModel.UserName)) validationMessage = ContestParkResources.ServerMessage_userNameRequiredFields;// Kullanıcı adı boş ise
            else if (String.IsNullOrEmpty(RegisterUserModel.FullName)) validationMessage = ContestParkResources.ServerMessage_fullNameRequiredFields;// Ad soyad boş ise
            else if (String.IsNullOrEmpty(RegisterUserModel.Email)) validationMessage = ContestParkResources.ServerMessage_emailRequiredFields;// Eposta boş ise
            else if (String.IsNullOrEmpty(RegisterUserModel.Password)) validationMessage = ContestParkResources.ServerMessage_passwordRequiredFields;// Şifre adı boş ise
            else if (RegisterUserModel.UserName.Length < 3) validationMessage = ContestParkResources.ServerMessage_userNameMinLength;// Kullanocı adı 3 karakterden küçük olamaz
            else if (RegisterUserModel.UserName.Length > 255) validationMessage = ContestParkResources.ServerMessage_userNameMaxLength;// Kullanocı adı 255 karakterden büyük olamaz
            else if (RegisterUserModel.FullName.Length < 3) validationMessage = ContestParkResources.ServerMessage_fullNameMinLength;// Ad soyad 3 karakterden küçük olamaz
            else if (RegisterUserModel.FullName.Length > 255) validationMessage = ContestParkResources.ServerMessage_fullNameMaxLength;// Ad soyad 255 karakterden büyük olamaz
            else if (RegisterUserModel.Email.Length > 255) validationMessage = ContestParkResources.ServerMessage_emailMaxLength;// Eposta adresi 255 karakterden büyük olamaz
            // TODO: Eposta adresi doğru formatta mı kontrol edilcek
            else if (RegisterUserModel.Password.Length < 8) validationMessage = ContestParkResources.ServerMessage_passwordMinLength;// Kullanocı adı 8 karakterden küçük olamaz
            else if (RegisterUserModel.Password.Length > 32) validationMessage = ContestParkResources.ServerMessage_passwordMaxLength; // Kullanocı adı 32 karakterden büyük olamaz
            if (!String.IsNullOrWhiteSpace(validationMessage))
            {
                await DisplayAlertAsync(ContestParkResources.Error,
                                        validationMessage,
                                        ContestParkResources.Okay);
                IsBusy = false;
                return;
            }

            #endregion Validation

            RegisterUserModel.ConfirmPassword = RegisterUserModel.Password;
            bool isSuccess = await _accountService.RegisterAsync(RegisterUserModel);
            if (isSuccess)
            {
                await DisplayAlertAsync(ContestParkResources.Success,
                                        ContestParkResources.WelcomeAboard,
                                        ContestParkResources.Okay);
                bool isOK = await _accountService.LoginProgcessAsync(new LoginModel
                {
                    UserName = RegisterUserModel.UserName,
                    Password = RegisterUserModel.Password
                });
                if (isOK) await PushNavigationPageAsync($"app:///{nameof(MainPage)}?appModuleRefresh=OnInitialized");
            }
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        public ICommand SignUpCommand => new Command(async () => await ExecuteSignUpCommandAsync());

        #endregion Commands
    }
}