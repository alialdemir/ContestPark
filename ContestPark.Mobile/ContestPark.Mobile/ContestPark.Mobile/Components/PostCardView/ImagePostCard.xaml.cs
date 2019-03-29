using ContestPark.Entities.Models;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePostCard : ContentView
    {
        #region Private

        private bool IsBusy;
        private readonly INavigationService _navigationService;

        #endregion Private

        #region Constructors

        public ImagePostCard(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
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

        private Command gotoPhotoModalCommand;

        public Command GotoPhotoModalCommand
        {
            get
            {
                return gotoPhotoModalCommand ?? (gotoPhotoModalCommand = new Command(() =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;
                    PostListModel model = (PostListModel)BindingContext;
                    if (model == null)
                        return;

                    _navigationService?.NavigateAsync(nameof(PhotoModalView), new NavigationParameters
                    {
                         { "userPictureList",  new UserPictureListModel { PicturePath=model.AlternativePicturePath } }
                    }, useModalNavigation: true);
                    IsBusy = false;
                }));
            }
        }

        #endregion Commands
    }
}