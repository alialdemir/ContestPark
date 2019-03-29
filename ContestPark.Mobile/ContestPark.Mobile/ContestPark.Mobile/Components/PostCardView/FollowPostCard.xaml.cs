﻿using ContestPark.Mobile.Views;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowPostCard : ContentView
    {
        #region Private

        private bool IsBusy;
        private readonly INavigationService _navigationService;

        #endregion Private

        #region Constructors

        public FollowPostCard(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();
        }

        #endregion Constructors

        #region Commands

        private Command<string> gotoProfilePageCommand;

        /// <summary>
        /// Go to ProfilePage load command
        /// </summary>
        public Command<string> GotoProfilePageCommand
        {
            get
            {
                return gotoProfilePageCommand ?? (gotoProfilePageCommand = new Command<string>((userName) =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;
                    _navigationService?.NavigateAsync(nameof(ProfilePage), new NavigationParameters
                    {
                         { "UserName", userName }
                    });
                    IsBusy = false;
                }));
            }
        }

        #endregion Commands
    }
}