using ContestPark.Entities.Models;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContestPostCard : ContentView
    {
        #region Private

        private bool IsBusy;
        private readonly INavigationService _navigationService;

        #endregion Private

        #region Constructors

        public ContestPostCard(INavigationService navigationService)
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
                    _navigationService?.NavigateAsync($"{nameof(BaseNavigationPage)}/{nameof(ProfilePage)}", new NavigationParameters
                    {
                         { "UserName", userName }
                    });
                    IsBusy = false;
                }));
            }
        }

        private Command<PostListModel> gotoDuelResultPageCommand;

        /// <summary>
        /// Go to DuelResultPage load command
        /// </summary>
        public Command<PostListModel> GotoDuelResultPageCommand
        {
            get
            {
                return gotoDuelResultPageCommand ?? (gotoDuelResultPageCommand = new Command<PostListModel>((model) =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;
                    int duelId = Convert.ToInt32(model.AlternativeId);
                    _navigationService?.NavigateAsync(nameof(DuelResultPage), new NavigationParameters
                    {
                        { "DuelId", duelId },
                        { "SubCategoryId", model.SubCategoryId },
                        { "IsNavBarShow", false },
                    });
                    IsBusy = false;
                }));
            }
        }

        #endregion Commands
    }
}