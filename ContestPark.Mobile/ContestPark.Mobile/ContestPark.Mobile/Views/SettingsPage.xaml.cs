using ContestPark.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        #region Constructors

        public SettingsPage()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Item seçildiği zaman yapılacak işlem
        /// </summary>
        /// <param name="sender">ListView objesi</param>
        /// <param name="e">e.SelectedItem içerisinde seçilen item modeli</param>
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (IsBusy || e.SelectedItem == null)
                return;

            if (e.SelectedItem is Models.MenuItem currentItem)
                (BindingContext as SettingsPageViewModel).GoToPageCommand.Execute(currentItem.PageName);

            ((ListView)sender).SelectedItem = null;
        }

        #endregion Events
    }
}