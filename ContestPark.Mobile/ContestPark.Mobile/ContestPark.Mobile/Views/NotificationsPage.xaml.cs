using ContestPark.Mobile.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        #region Constructors

        public NotificationsPage()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Property

        public NotificationsPageViewModel ViewModel
        {
            get
            {
                return BindingContext as NotificationsPageViewModel;
            }
        }

        #endregion Property

        #region Methods

        /// <summary>
        /// Item seçildiği zaman yapılacak işlem
        /// </summary>
        /// <param name="sender">ListView objesi</param>
        /// <param name="e">e.SelectedItem içerisinde seçilen item modeli</param>
        private async Task ExecuteSelectedItemCommandAsync(int notificationId)
        {// prisme göre uyarlancak ve viewmodele taşıncak
            //////////UserNotificationListModel selectedModel = ViewModel.Items.FirstOrDefault(p => p.NotificationId == notificationId);

            //////////NotificationTypes notificationType = (NotificationTypes)selectedModel.NotificationTypeId;
            //////////if (selectedModel.IsContest && !NotificationTypes.Contest.HasFlag(notificationType))
            //////////    await Navigation.PushModalAsync(new DuelResultPage() { BindingContext = new DuelResultPageViewModel(int.Parse(selectedModel.Link), selectedModel.SubCategoryId, false) { Navigation = Navigation } });
            //////////else if (NotificationTypes.LinkLike == notificationType || NotificationTypes.Comment == notificationType)
            //////////    await Navigation.PushAsync(new PostsPage(int.Parse(selectedModel.Link)));
        }

        #endregion Methods

        #region Command

        private ICommand acceptsDuelCommand;

        /// <summary>
        /// Düelloyu kabul et command
        /// </summary>
        public ICommand AcceptsDuelCommand
        {
            get { return acceptsDuelCommand ?? (acceptsDuelCommand = new Command<int>((notificationId) => ViewModel.ExecuteAcceptsDuelCommandAsync(notificationId))); }
        }

        private ICommand smotherDuelCommand;

        /// <summary>
        /// Düello yenil command
        /// </summary>
        public ICommand SmotherDuelCommand
        {
            get { return smotherDuelCommand ?? (smotherDuelCommand = new Command<int>((notificationId) => ViewModel.ExecuteSmotherDuelCommandAsync(notificationId))); }
        }

        private ICommand deleteItemCommand;

        public ICommand DeleteItemCommand
        {
            get { return deleteItemCommand ?? (deleteItemCommand = new Command<int>((notificationId) => ViewModel.ExecuteDeleteItemCommandAsync(notificationId))); }
        }

        private ICommand selectedItemCommand;

        public ICommand SelectedItemCommand
        {
            get { return selectedItemCommand ?? (selectedItemCommand = new Command<int>(async (notificationId) => await ExecuteSelectedItemCommandAsync(notificationId))); }
        }

        #endregion Command
    }
}