using ContestPark.Mobile.ViewModels;
using Prism.Navigation;
using Xamarin.Forms;

namespace ContestPark.Mobile.Views
{
    public partial class TabPage : TabbedPage
    {
        #region Constructors

        public TabPage()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Override

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            this.Title = this.CurrentPage.Title;

            NavigationParameters parameters = new NavigationParameters();
            if (this.CurrentPage is ProfilePage)
            {
                TabPageViewModel viewModel = (TabPageViewModel)BindingContext;
                if (viewModel != null)
                {
                    parameters.Add("UserName", viewModel.UserDataModule.UserModel.UserName);
                    parameters.Add("IsVisibleBackArrow", false);
                }
            }

                (this.CurrentPage as INavigatedAware)?.OnNavigatedTo(parameters);
            (this.CurrentPage?.BindingContext as INavigatedAware)?.OnNavigatedTo(parameters);
        }

        #endregion Override
    }
}