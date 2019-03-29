using Autofac;
using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Configs;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.Views;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomPostCard : ContentView
    {
        #region Private

        private bool IsBusy;
        private readonly INavigationService _navigationService;

        #endregion Private

        #region Constructors

        public BottomPostCard(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        #endregion Constructors

        #region Override

        public string LikeText { get; set; }
        public string CommentText { get; set; }

        protected override void OnBindingContextChanged()
        {
            PostListModel model = (PostListModel)BindingContext;
            if (model != null)
            {
                LikeText = model.LikeCount + " " + ContestParkResources.Like;
                CommentText = model.CommentCount + " " + ContestParkResources.Comment;
            }
            base.OnBindingContextChanged();
        }

        #endregion Override

        #region Methods

        /// <summary>
        /// Postu beğenenleri listeleme sayfasına git
        /// </summary>
        private void ExecuteGoToPostLikesPageCommandAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PostListModel model = (PostListModel)BindingContext;
            _navigationService?.NavigateAsync(nameof(PostLikesPage), new NavigationParameters
            {
                { "PostId", model.PostId }
            });
            IsBusy = false;
        }

        /// <summary>
        /// Beğen beğenemkten vazgeç
        /// </summary>
        private async Task ImageButton_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            ILikesService likesService = RegisterTypesConfig.Container.Resolve<ILikesService>();
            PostListModel model = (PostListModel)BindingContext;
            if (model != null && likesService != null)
            {
                if (model.IsLike)
                {
                    //un like
                    model.IsLike = !model.IsLike;
                    var isOK = await likesService.DisLike(model.PostId);
                    if (!isOK) model.IsLike = !model.IsLike;
                }
                else
                {
                    // like
                    model.IsLike = !model.IsLike;
                    var isOK = await likesService.Like(model.PostId);
                    if (!isOK) model.IsLike = !model.IsLike;
                }
            }
            IsBusy = false;
        }

        /// <summary>
        /// Post deyay sayfasına git
        /// </summary>
        private void ExecuteGoToPosPostCommentPageCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PostListModel model = (PostListModel)BindingContext;
            _navigationService?.NavigateAsync(nameof(PostsPage), new NavigationParameters
            {
                { "PostId", model.PostId }
            });
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        private ICommand _goToPostLikesPageCommand;

        public ICommand GoToPostLikesPageCommand
        {
            get { return _goToPostLikesPageCommand ?? (_goToPostLikesPageCommand = new Command(() => ExecuteGoToPostLikesPageCommandAsync())); }
        }

        private ICommand _goToPosPostCommentPageCommand;

        public ICommand GoToPosPostCommentPageCommand
        {
            get { return _goToPosPostCommentPageCommand ?? (_goToPosPostCommentPageCommand = new Command(() => ExecuteGoToPosPostCommentPageCommand())); }
        }

        #endregion Commands
    }
}