using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        #region Constructors

        public SignInPage()
        {
            InitializeComponent();
            txtUserName.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
            txtPassword.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
        }

        #endregion Constructors
    }
}