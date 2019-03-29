using ContestPark.Entities.Enums;
using ContestPark.Extensions;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class LanguagesPageViewModel : ViewModelBase
    {
        #region Private variables

        private readonly ISettingsService _settingsService;
        private readonly ILanguageService _languageService;

        #endregion Private variables

        #region Constructors

        public LanguagesPageViewModel(IPageDialogService pageDialogService,
                                       INavigationService navigationService,
                                      ISettingsService settingsService,
                                      ILanguageService languageService) : base(navigationService, pageDialogService)
        {
            Title = ContestParkResources.Languages;
            _settingsService = settingsService;
            _languageService = languageService;

            if (_languageService.Language.HasFlag(Languages.Turkish))
            {
                IsToggledTurkish = true;
                IsToggledEnglish = false;
            }
            else if (_languageService.Language.HasFlag(Languages.English))
            {
                IsToggledTurkish = false;
                IsToggledEnglish = true;
            }
        }

        #endregion Constructors

        #region Properties

        private bool _isToggledTurkish;

        public bool IsToggledTurkish
        {
            get { return _isToggledTurkish; }
            set { SetProperty(ref _isToggledTurkish, value); }
        }

        private bool _isToggledEnglish;

        public bool IsToggledEnglish
        {
            get { return _isToggledEnglish; }
            set { SetProperty(ref _isToggledEnglish, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Dil değiştirme metotdu
        /// </summary>
        private async Task SetLanguageAsync(Languages language)
        {
            if (IsBusy) return;

            IsBusy = true;
            bool isOk = await DisplayAlertAsync(ContestParkResources.ChangeLangTitle,
                                                ContestParkResources.ChangeLang,
                                                ContestParkResources.ChangeOkay,
                                                ContestParkResources.Cancel);
            if (!isOk)
            {
                IsBusy = false;
                return;
            }

            string languageCode = language.ToLanguageCode();

            bool isSuccess = await _settingsService.UpdateSettingsAsync(languageCode, SettingTypes.Language);
            if (!isSuccess)
            {
                await DisplayAlertAsync(ContestParkResources.ChangeLangTitle,
                                        ContestParkResources.ChangeLangError,
                                        ContestParkResources.Okay);
                IsBusy = false;
                return;
            }

            _languageService.SetUserLanguage(languageCode);
            await PushModalAsync($"{nameof(Views.MasterDetailPage)}/{nameof(BaseNavigationPage)}/{nameof(TabPage)}");
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        public ICommand SetTrCommand => new Command(async () => await SetLanguageAsync(Languages.Turkish));
        public ICommand SetEnCommand => new Command(async () => await SetLanguageAsync(Languages.English));

        #endregion Commands
    }
}