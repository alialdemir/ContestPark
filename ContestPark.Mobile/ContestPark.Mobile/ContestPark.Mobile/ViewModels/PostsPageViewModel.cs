using ContestPark.Mobile.ViewModels.Base;
using Prism.Navigation;

namespace ContestPark.Mobile.ViewModels
{
    public class PostsPageViewModel : ViewModelBase
    {
        #region Private variable

        private int _postId;

        #endregion Private variable

        #region Constructors

        public PostsPageViewModel()
        {
        }

        #endregion Constructors

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("PostId")) _postId = parameters.GetValue<int>("PostId");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}