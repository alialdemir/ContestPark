using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using Xamarin.Forms;

namespace ContestPark.Mobile.Components
{
    public partial class PhotoModalView : ContentPage
    {
        #region Constructors

        public PhotoModalView(int selectedIndex = 0, params UserPictureListModel[] userPictureList)
        {
            InitializeComponent();
            ObservableRangeCollection<UserPictureListModel> photoCollection = new ObservableRangeCollection<UserPictureListModel>();
            photoCollection.AddRange(userPictureList);

            cvCphotos.ItemsSource = photoCollection;
            cvCphotos.Position = selectedIndex;
            fabButton.Clicked += (sender, e) => Navigation.PopModalAsync();
            fabSettings.Clicked += async (sender, e) =>
            {
                //await ContestParkApp.DisplayAlert(
                //         ContestParkResources.SelectImageProcessing,
                //         ContestParkResources.Cancel,
                //         "test", "tltlt");
            };
        }

        #endregion Constructors
    }
}