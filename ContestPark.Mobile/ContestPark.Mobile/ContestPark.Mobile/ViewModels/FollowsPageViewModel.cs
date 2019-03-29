using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class FollowsPageViewModel : ViewModelBase<ListOfFollowerModel>
    {
        #region Enum

        public enum ListTypes
        {
            Followers = 1,
            Following = 2,
        }

        #endregion Enum

        #region Private variables

        private string _followedUserId;
        private ListTypes _listType;
        private readonly IFollowsService _followsService;

        #endregion Private variables

        #region Constructors

        public FollowsPageViewModel(IFollowsService followsService)
        {
            _followsService = followsService;
        }

        #endregion Constructors

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (ListTypes.Followers.HasFlag(_listType)) ServiceModel = await _followsService.FollowersAsync(_followedUserId, ServiceModel);
            else ServiceModel = await _followsService.FollowingAsync(_followedUserId, ServiceModel);
            await base.LoadItemsAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Takip et takibi bırak
        /// </summary>
        /// <param name="userId">Takip edilecek kullanıcı id</param>
        private async Task ExecuteFollowOrUnFollowCommand(string userId)
        {
            bool isFollowUpStatus = await _followsService.IsFollowUpStatusAsync(userId);

            bool isSuccess = false;
            if (isFollowUpStatus) isSuccess = await _followsService.UnFollowAsync(userId);
            else isSuccess = await _followsService.FollowUpAsync(userId);

            if (isSuccess)
            {
                var follow = Items.FirstOrDefault(p => p.FollowUpUserId == userId);
                follow.IsFollowUpStatus = !follow.IsFollowUpStatus;
                Items.Replace(follow);
            }
        }

        #endregion Methods

        #region Commands

        private ICommand _followOrUnFollowCommand;

        public ICommand FollowOrUnFollowCommand
        {
            get { return _followOrUnFollowCommand ?? (_followOrUnFollowCommand = new Command<string>(async (userId) => await ExecuteFollowOrUnFollowCommand(userId))); }
        }

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("ListType")) _listType = parameters.GetValue<ListTypes>("ListType");
            if (parameters.ContainsKey("FollowedUserId")) _followedUserId = parameters.GetValue<string>("FollowedUserId");
            Title = ListTypes.Followers.HasFlag(_listType) ? ContestParkResources.Followers : ContestParkResources.Following;
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}