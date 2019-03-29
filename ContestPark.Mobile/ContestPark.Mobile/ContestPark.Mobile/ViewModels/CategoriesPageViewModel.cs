using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Events;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using ContestPark.Mobile.Views;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class CategoriesPageViewModel : ViewModelBase<CategoryModel>
    {
        #region Private varaibles

        private readonly ICategoryServices _CategoryServices;
        private readonly ICpService _CpService;
        private readonly IFollowCategoryService _followCategoryService;
        private readonly IOpenSubCategoryService _openSubCategoryService;
        private readonly IDuelModule _duelModule;

        #endregion Private varaibles

        #region Constructors

        public CategoriesPageViewModel(ICategoryServices CategoryServices,
                                       ICpService CpService,
                                       IFollowCategoryService followCategoryService,
                                       IOpenSubCategoryService openSubCategoryService,
                                       INavigationService navigationService,
                                       IPageDialogService pageDialogService,
                                       IEventAggregator eventAggregator,
                                       IDuelModule duelModule) : base(navigationService, pageDialogService)
        {
            Title = ContestParkResources.Categories;
            _CategoryServices = CategoryServices;
            _CpService = CpService;
            _followCategoryService = followCategoryService;
            _openSubCategoryService = openSubCategoryService;
            _duelModule = duelModule;
            _duelModule.NavigationService = navigationService;
            EventSubscribe(eventAggregator);
            ServiceModel.PageSize = 9999;// Şimdilik 9999 verdim kategorilerde safyalama yok
        }

        #endregion Constructors

        #region Properties

        public int SeeAllSubCateogryId { get; set; } = 0;

        /// <summary>
        /// Kullanıcı altın miktarı
        /// </summary>
        private string _userCoins = "0";

        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string UserCoins
        {
            get { return _userCoins; }
            set { SetProperty(ref _userCoins, value, nameof(UserCoins)); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            SetUserGoldCommand.Execute(null);

            //TODO: Kategorileri sayfala
            var followingSubCategories = await _followCategoryService.FollowingSubCategoryListAsync(ServiceModel);
            var subCategories = await _CategoryServices.CategoryListAsync(ServiceModel);
            if (followingSubCategories != null && followingSubCategories.Items != null && followingSubCategories.Items.Count() > 0)
            {
                Items.AddRange(new List<CategoryModel>()
                            {
                                            new CategoryModel()
                                            {
                                                CategoryId = -1,//-1 Çünkü CategorySearchPage sayfasında -1 gelirse takip ettiğim kategorileri listeler
                                                CategoryName = ContestParkResources.FollowingCategories,
                                                SubCategories = followingSubCategories.Items.ToList()
                                            }
                        });
            }
            if (subCategories != null && subCategories.Items != null) Items.AddRange(subCategories.Items);
            IsBusy = false;
            await base.LoadItemsAsync();
        }

        private async Task ExecutGoToCategorySearchPageCommand(int CategoryId = 0)
        {
            await PushNavigationPageAsync($"{nameof(CategorySearchPage)}?CategoryId={CategoryId}");
        }

        /// <summary>
        /// Kullanıcı altın miktarı
        /// </summary>
        /// <returns></returns>
        private async Task SetUserGoldAsync()
        {
            int userGold = await _CpService.UserTotalCp();
            if (userGold > 0) UserCoins = userGold.ToString("##,#").Replace(",", ".");
            else UserCoins = userGold.ToString();
        }

        /// <summary>
        /// Event dinleme
        /// </summary>
        private void EventSubscribe(IEventAggregator eventAggregator)
        {
            eventAggregator
                          .GetEvent<SubCategoryRefleshEvent>()
                          .Subscribe(() => RefleshCommand.Execute(null));
        }

        #endregion Methods

        #region Commands

        public ICommand SetUserGoldCommand => new Command(async () => await SetUserGoldAsync());
        private ICommand _goToCategorySearchPageCommand;
        public ICommand GoToCategorySearchPageCommand => _goToCategorySearchPageCommand ?? (_goToCategorySearchPageCommand = new Command<int>(async (CategoryId) => await ExecutGoToCategorySearchPageCommand(CategoryId)));
        private ICommand _pushEnterPageAsyncCommand;
        public ICommand PushEnterPageAsyncCommand => _pushEnterPageAsyncCommand ?? (_pushEnterPageAsyncCommand = new Command<SubCategoryModel>(async (subCategoryModel) => await _duelModule.PushEnterPageAsync(subCategoryModel)));
        private ICommand _subCategoriesDisplayActionSheetCommand;
        public ICommand SubCategoriesDisplayActionSheetCommand => _subCategoriesDisplayActionSheetCommand ?? (_subCategoriesDisplayActionSheetCommand = new Command<SubCategoryModel>(async (subCategoryModel) => await _duelModule.SubCategoriesDisplayActionSheetAsync(subCategoryModel)));

        #endregion Commands
    }
}