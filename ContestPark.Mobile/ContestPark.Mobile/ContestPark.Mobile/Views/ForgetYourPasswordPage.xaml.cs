using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetYourPasswordPage : ContentPage
    {
        #region Constructors

        public ForgetYourPasswordPage()
        {
            InitializeComponent();
            txtUserNameOrEmail.BackgroundColor = Color.FromRgba(0, 0, 0, 125);
        }

        #endregion Constructors
    }
}