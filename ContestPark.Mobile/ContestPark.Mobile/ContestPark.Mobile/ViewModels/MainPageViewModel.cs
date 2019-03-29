using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;

namespace ContestPark.Mobile
{
    public class MainPageViewModel : ViewModelBase, INavigationAware
    {
        #region Private variables

        private readonly IUserDataModule _userDataModule;
        private readonly ILanguageService _languageService;

        #endregion Private variables

        #region Constructors

        public MainPageViewModel(INavigationService navigationService,
                                 IUserDataModule userDataModule,
                                 ILanguageService languageService) : base(navigationService)
        {
            _userDataModule = userDataModule;
            _languageService = languageService;
        }

        #endregion Constructors

        #region Navigations

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (_userDataModule.IsLoaded) PushNavigationPageAsync($"{nameof(MasterDetailPage)}/{nameof(BaseNavigationPage)}/{nameof(TabPage)}", useModalNavigation: null);
            else
            {
                _languageService.SetDefaultLanguage();
                _userDataModule.Unauthorized();
                PushModalAsync(nameof(SignInPage));
            }
        }

        #endregion Navigations
    }
}