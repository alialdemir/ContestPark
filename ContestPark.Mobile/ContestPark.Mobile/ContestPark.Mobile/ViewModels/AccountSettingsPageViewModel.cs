using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class AccountSettingsPageViewModel : ViewModelBase
    {
        #region Private variable

        private readonly IAccountService _accountService;
        private readonly IUserDataModule _userDataModule;

        #endregion Private variable

        #region Constructors

        public AccountSettingsPageViewModel(IPageDialogService pageDialogService,
                                            IAccountService accountService,
                                            IUserDataModule userDataModule) : base(dialogService: pageDialogService)
        {
            Title = ContestParkResources.AccountSettings;
            _accountService = accountService;
            _userDataModule = userDataModule;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Kullanıcı bilgisi
        /// </summary>
        private UpdateUserInfoModel _userInfo;

        public UpdateUserInfoModel UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value, nameof(UserInfo)); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            UserInfo = await _accountService.GetUpdateUserInfoAsync();
        }

        /// <summary>
        /// Kullanıcı bilgileri güncelle
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveCommandAsync()
        {
            if (IsBusy || UserInfo == null)
                return;
            IsBusy = true;
            bool isSuccess = await _accountService.SetUpdateUserInfoAsync(UserInfo);
            if (isSuccess)
            {
                _userDataModule.UserModel.UserName = UserInfo.UserName;
                if (!String.IsNullOrEmpty(UserInfo.NewPassword) && !String.IsNullOrEmpty(UserInfo.OldPassword))
                {
                    _userDataModule.UserModel.Password = UserInfo.NewPassword;
                    UserInfo.OldPassword = "";
                    UserInfo.NewPassword = "";
                }
                await DisplayAlertAsync(ContestParkResources.Success,
                     ContestParkResources.AccountInfoUpdate,
                     ContestParkResources.Okay);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Profil resim değiştir
        /// </summary>
        /// <returns></returns>
        private void ExecuteChangeProfilePictureCommand()
        {
        }

        /// <summary>
        /// Kapak resim değiştir
        /// </summary>
        /// <returns></returns>
        private void ExecuteChangeCoverPictureCommand()
        {
        }

        #endregion Methods

        #region Commands

        public ICommand SaveCommand => new Command(async () => await ExecuteSaveCommandAsync());
        public ICommand ChangeProfilePictureCommand => new Command(() => ExecuteChangeProfilePictureCommand());
        public ICommand ChangeCoverPictureCommand => new Command(() => ExecuteChangeCoverPictureCommand());

        #endregion Commands
    }
}