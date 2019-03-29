using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        #region Constructors

        public SignUpPage()
        {
            InitializeComponent();
            txtUserName.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
            txtFullName.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
            txtEmail.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
            txtPassword.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
        }

        #endregion Constructors
    }
}