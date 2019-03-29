using ContestPark.Entities.Models;
using ContestPark.Mobile.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatDetailsPage : ContentPage
    {
        #region Constructors

        public ChatDetailsPage()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region ViewModel

        private ChatDetailsPageViewModel ViewModel
        {
            get
            {
                return BindingContext as ChatDetailsPageViewModel;
            }
        }

        #endregion ViewModel

        #region Methods

        /// <summary>
        /// Sayfa yüklendikten sonra txtChatbox'a focuslanır böylece klavye açık gelir
        /// </summary>
        protected override void OnAppearing()
        {
            if (ViewModel.IsInitialized)
                return;
            ViewModel.LoadItemsCommand.Execute(null);
            ViewModel.ListViewScrollToBottomCommand = new Command<int>((index) =>
            {
                if (ViewModel.Items.Count > 0 && index >= 0 && index < ViewModel.Items.Count)
                    lstMessages.ScrollTo(ViewModel.Items[index], ScrollToPosition.Center, true);
            });
            ViewModel.IsInitialized = true;

            txtChatbox.Focus();
        }

        #endregion Methods

        #region Events

        /// <summary>
        /// Entry dolu ise gönder buttonu aktif boş ise pasif olsun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtChatbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtChatbox.lineHeight != 0)
            {
                if (txtChatbox.Text == null) return;

                double newHeight;
                int count = txtChatbox.Text.Split(new string[] { "\n" }, StringSplitOptions.None).Count();
                if (count == 0) newHeight = txtChatbox.lineHeight;
                else newHeight = ((txtChatbox.lineHeight) * (count + 1));
                if (txtChatbox.Height != newHeight) txtChatbox.HeightRequest = newHeight;
            }
            if (txtChatbox.Text.Trim().Length <= 0)
            {
                btnBtnSendMessage.Opacity = 0.20;
                txtChatbox.HeightRequest = 35;
            }
            else btnBtnSendMessage.Opacity = 1;
        }

        /// <summary>
        /// Enrty'e tıklanıncca scroll'u aşaya çeker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtChatbox_Focused(object sender, FocusEventArgs e)
        {
            ViewModel.ListViewScrollToBottomCommand.Execute(null);
        }

        /// <summary>
        /// Mesaj gönder
        /// </summary>
        /// <param name="sender">Entry objesi</param>
        /// <param name="e">EventArgs</param>
        private void txtChatbox_Completed(object sender, EventArgs e)
        {
            txtChatbox.Focus();
            ViewModel.SendMessageCommand.Execute(null);
        }

        private void OnItemAppearing(object Sender, ItemVisibilityEventArgs e)
        {
            var currentItem = (ChatHistoryModel)e.Item;
            if (!ViewModel.IsInitialized || ViewModel.ServiceModel.IsLastPage || !(currentItem is ChatHistoryModel)) return;
            if (ViewModel.Items.FirstOrDefault().Equals(currentItem)) ViewModel.LoadItemsCommand.Execute(null);
        }

        #endregion Events
    }
}