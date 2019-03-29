using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportPage : ContentPage
    {
        #region Constructors

        public SupportPage()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// TxtMessage dolu ise boyutunu büyültür
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMessage.lineHeight == 0 || txtMessage.Text == null) return;

            double newHeight;
            int count = txtMessage.Text.Split(new string[] { "\n" }, StringSplitOptions.None).Count();
            if (count == 0) newHeight = txtMessage.lineHeight;
            else newHeight = ((txtMessage.lineHeight) * (count + 1));
            if (txtMessage.Height != newHeight) txtMessage.HeightRequest = newHeight;
        }

        #endregion Events
    }
}