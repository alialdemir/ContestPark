using ContestPark.Entities.Helpers;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region Constructors

        public SettingsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = ContestParkResources.Settings;
        }

        #endregion Constructors

        #region Property

        private ObservableRangeCollection<MenuItemList> items;

        /// <summary>
        /// Listview içerisine bind edilecek liste
        /// </summary>
        public ObservableRangeCollection<MenuItemList> Items
        {
            get { return items ?? (items = new ObservableRangeCollection<MenuItemList>()); }
        }

        #endregion Property

        #region Methods

        protected override Task LoadItemsAsync()
        {
            Items.AddRange(new List<MenuItemList>()
            {
                new MenuItemList(ContestParkResources.GlobalSettings)
                                {
                                    new Models.MenuItem { PageName = nameof(LanguagesPage), Icon = "ic_public_black_24dp.png", Title = ContestParkResources.Languages },
                                    new Models.MenuItem { PageName = nameof(AccountSettingsPage), Icon = "ic_person_black_24dp.png", Title = ContestParkResources.AccountSettings },
                                    new Models.MenuItem { PageName = nameof(ChatSettingsPage), Icon = "ic_forum_black_24dp.png", Title = ContestParkResources.ChatSettings },
                                }
            });
            return base.LoadItemsAsync();
        }

        #endregion Methods

        #region Commands

        private Command<string> goToPageCommand;
        public Command<string> GoToPageCommand { get { return goToPageCommand ?? (goToPageCommand = new Command<string>(async (parameter) => await PushNavigationPageAsync(parameter))); } }

        #endregion Commands
    }
}