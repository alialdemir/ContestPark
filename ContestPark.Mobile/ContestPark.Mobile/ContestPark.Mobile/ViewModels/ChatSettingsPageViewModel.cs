using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class ChatSettingsPageViewModel : ViewModelBase<UserBlockListModel>
    {
        #region Private variables

        private readonly IChatBlocksService _chatBlocksService;

        #endregion Private variables

        #region Constructors

        public ChatSettingsPageViewModel(IChatBlocksService chatBlocksService)
        {
            Title = ContestParkResources.ChatSettings;
            _chatBlocksService = chatBlocksService;
        }

        #endregion Constructors

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ServiceModel = await _chatBlocksService.UserBlockListAsync(ServiceModel);
            await base.LoadItemsAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Kullanıcı engelini kaldırma
        /// </summary>
        /// <param name="selectedModel"></param>
        /// <returns></returns>
        public async Task ExecuteBlockingProgressCommandAsync(string userId)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            UserBlockListModel selectedModel = Items.First(p => p.UserId == userId);
            if (selectedModel != null)
            {
                bool isSuccess = await _chatBlocksService.BlockingProgressAsync(selectedModel.FullName, selectedModel.UserId, true);
                if (isSuccess) Items.Remove(selectedModel);
            }
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        private ICommand blockingProgressCommand;

        /// <summary>
        /// Sohbet engel kaldır command
        /// </summary>
        public ICommand BlockingProgressCommand
        {
            get { return blockingProgressCommand ?? (blockingProgressCommand = new Command<string>(async (userId) => await ExecuteBlockingProgressCommandAsync(userId))); }
        }

        #endregion Commands
    }
}