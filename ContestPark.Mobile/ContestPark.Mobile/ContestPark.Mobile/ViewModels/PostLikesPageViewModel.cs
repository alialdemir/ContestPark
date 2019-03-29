using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Navigation;
using System.Threading.Tasks;

namespace ContestPark.Mobile.ViewModels
{
    public class PostLikesPageViewModel : ViewModelBase<LikesModel>
    {
        #region Private variable

        private int _postId;
        private readonly ILikesService _likesService;

        #endregion Private variable

        #region Constructors

        public PostLikesPageViewModel(ILikesService likesService)
        {
            Title = ContestParkResources.ThisLikes;
            _likesService = likesService;
        }

        #endregion Constructors

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ServiceModel = await _likesService.Likes(_postId, ServiceModel);
            await base.LoadItemsAsync();
            IsBusy = false;
        }

        #endregion Methods

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("PostId")) _postId = parameters.GetValue<int>("PostId");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}