using ContestPark.Entities.Helpers;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Events;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class MasterPageViewModel : ViewModelBase
    {
        #region Private variables

        private readonly IUserDataModule _userDataModule;
        private readonly IEventAggregator _eventAggregator;

        #endregion Private variables

        #region Constructors

        public MasterPageViewModel(INavigationService navigationService,
                                   IEventAggregator eventAggregator,
                                   IUserDataModule userDataModule) : base(navigationService)
        {
            _eventAggregator = eventAggregator;
            _userDataModule = userDataModule;
            FullName = userDataModule.UserModel.FullName;
            CoverPicture = userDataModule.UserModel.UserCoverPicturePath;
            LoadItemsAsync();
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

        private string _coverPicture;

        public string CoverPicture
        {
            get { return _coverPicture; }
            set { SetProperty(ref _coverPicture, value); }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        #endregion Property

        #region Methods

        protected override Task LoadItemsAsync()
        {
            Items.AddRange(new List<MenuItemList>()
            {
                new MenuItemList("Menu")
                            {
                                new Models.MenuItem { PageName = nameof(ContestStorePage), Icon = "ic_shopping_cart_black_24dp.png", Title = ContestParkResources.ContestStore },
                                new Models.MenuItem { PageName = nameof(MissionsPage),Icon = "ic_filter_list_black_24dp.png", Title = ContestParkResources.Missions },
                                new Models.MenuItem { PageName = nameof(SupportPage), Icon = "ic_child_care_black_24dp.png",Title = ContestParkResources.RequestsAndComplaints },
                                new Models.MenuItem { PageName = nameof(SettingsPage), Icon = "ic_settings_applications_black_24dp.png",Title = ContestParkResources.Settings },
                                new Models.MenuItem { PageName = "Exit", Icon = "ic_input_black_24dp.png",Title = ContestParkResources.Exit }
                            },
                new MenuItemList(ContestParkResources.FollowUsOnSocialNetworks)
                            {
                                 new Models.MenuItem { PageName = "FacebookAddress",  Icon = "facebook_36x.png", Title = ContestParkResources.FacebookAddress },
                                 new Models.MenuItem { PageName = "TwitterAddress", Icon = "twitter_36x.png", Title = ContestParkResources.TwitterAddress },
                                 new Models.MenuItem { PageName = "InstagramAddress", Icon = "instagram_36x.png", Title = ContestParkResources.InstagramAddress }
                            }
            });
            return base.LoadItemsAsync();
        }

        private void ExecutePushPageCommand(string name)
        {
            if (name == "Exit")
            {
                _userDataModule.Unauthorized();
                PushNavigationPageAsync("app:///SignInPage?appModuleRefresh=OnInitialized");
            }
            else if (!String.IsNullOrEmpty(name))
            {
                _eventAggregator
                .GetEvent<MasterDetailPageIsPresentedEvent>()
                .Publish(false);
                _eventAggregator
                        .GetEvent<TabPageNavigationEvent>()
                        .Publish(name);
            }
        }

        #endregion Methods

        #region Commands

        private ICommand closePresented;

        public ICommand ClosePresented
        {
            set { closePresented = value; }
            get { return closePresented; }
        }

        private ICommand _pushPageCommand;

        public ICommand PushPageCommand
        {
            get { return _pushPageCommand ?? (_pushPageCommand = new Command<string>((pageName) => ExecutePushPageCommand(pageName))); }
        }

        #endregion Commands
    }
}