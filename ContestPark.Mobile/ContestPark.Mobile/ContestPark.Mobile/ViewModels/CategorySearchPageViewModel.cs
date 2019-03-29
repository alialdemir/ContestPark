using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Events;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Events;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class CategorySearchPageViewModel : ViewModelBase<SubCategorySearchModel>
    {
        #region Private variable

        private int _CategoryId = 0;
        private readonly IFollowCategoryService _followCategoryService;
        private readonly ICategoryServices _CategoryServices;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDuelModule _duelModule;

        #endregion Private variable

        #region Constructors

        public CategorySearchPageViewModel(IFollowCategoryService followCategoryService,
                                           ICategoryServices CategoryServices,
                                           IEventAggregator eventAggregator,
                                           IDuelModule duelModule)
        {
            Title = ContestParkResources.CategorySearch;
            _followCategoryService = followCategoryService;
            _CategoryServices = CategoryServices;
            _eventAggregator = eventAggregator;
            _duelModule = duelModule;
            EventSubscribe();
        }

        #endregion Constructors

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (_CategoryId == -1) ServiceModel = await _followCategoryService.FollowingSubCategorySearchAsync(ServiceModel);// _CategoryId  -1 gelirse takip ettiğim kategoriler
            else ServiceModel = await _CategoryServices.SearchAsync(_CategoryId, ServiceModel);//0 gelirse tüm kategoriler demek 0 dan büyük ise ilgili kategoriyi getirir
            await base.LoadItemsAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Kategori giriş sayfasına git
        /// </summary>
        /// <param name="subCategoryId">Alt kategori ıd</param>
        /// <returns></returns>
        private void ExecutPushEnterPageCommandAsync(int subCategoryId)
        {
            SubCategorySearchModel selectedModel = Items.Where(p => p.SubCategoryId == subCategoryId).First();
            _duelModule.PushEnterPageAsync(new SubCategoryModel()
            {
                PicturePath = selectedModel.PicturePath,
                SubCategoryId = selectedModel.SubCategoryId,
                SubCategoryName = selectedModel.SubCategoryName,
                DisplayPrice = selectedModel.DisplayPrice
            });
        }

        /// <summary>
        /// Event dinleme
        /// </summary>
        private void EventSubscribe()
        {
            _eventAggregator
                          .GetEvent<SubCategoryRefleshEvent>()
                          .Subscribe(() => RefleshCommand.Execute(null));
        }

        #endregion Methods

        #region Commands

        private ICommand pushEnterPageCommand;

        /// <summary>
        /// Yarışma kategorileri command
        /// </summary>
        public ICommand PushEnterPageCommand
        {
            get { return pushEnterPageCommand ?? (pushEnterPageCommand = new Command<int>((subCategoryId) => ExecutPushEnterPageCommandAsync(subCategoryId))); }
        }

        #endregion Commands

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("CategoryId")) _CategoryId = parameters.GetValue<int>("CategoryId");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}