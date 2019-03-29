using ContestPark.Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuelEnterScreenPage : PopupPage
    {
        #region Constructors

        public DuelEnterScreenPage()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Constructors

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var ViewModel = ((DuelEnterScreenPageViewModel)BindingContext);
            if (ViewModel.IsInitialized)
                return;

            ViewModel.LoadItemsCommand.Execute(null);
            ViewModel.AnimationCommand = new Command(() => Animate());
            ViewModel.IsInitialized = true;
        }

        protected override bool OnBackButtonPressed()
        {
            ((DuelEnterScreenPageViewModel)BindingContext).DuelCloseCommand.Execute(null);
            return true;
        }

        #endregion Constructors

        #region Methods

        private void Animate()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var height = Application.Current.MainPage.Height;

                var founderAnimation = new Animation();
                var topToEnd = new Animation(callback: d => gridFounder.TranslationY = d,
                                               start: -height,
                                               end: 0,
                                               easing: Easing.BounceOut);
                founderAnimation.Add(0, 1, topToEnd);
                founderAnimation.Commit(gridFounder, "Loop", length: 2000);

                var copetitorAnimation = new Animation();
                var endToTop = new Animation(callback: d => gridCompetitor.TranslationY = d,
                                                start: height,
                                                  end: 0,
                                               easing: Easing.BounceOut);
                copetitorAnimation.Add(0, 1, endToTop);
                copetitorAnimation.Commit(gridCompetitor, "Loop", length: 2000);
            });
        }

        #endregion Methods
    }
}