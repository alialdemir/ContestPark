using Autofac;
using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Configs;
using ContestPark.Mobile.Events;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.Views;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Modules
{
    public class DuelModule : IDuelModule
    {
        #region Private varaibles

        private readonly IPageDialogService _pageDialogService;
        private readonly IFollowCategoryService _followCategoryService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IOpenSubCategoryService _openSubCategoryService;

        #endregion Private varaibles

        #region Constructors

        public DuelModule(
                          IPageDialogService pageDialogService,
                          IFollowCategoryService followCategoryService,
                          IEventAggregator eventAggregator,
                          IOpenSubCategoryService openSubCategoryService)
        {
            _pageDialogService = pageDialogService;
            _followCategoryService = followCategoryService;
            _eventAggregator = eventAggregator;
            _openSubCategoryService = openSubCategoryService;
        }

        #endregion Constructors

        #region Property

        public INavigationService NavigationService { get; set; }

        #endregion Property

        #region Methods

        private void SubCategoryRefleshEvent()
        {
            _eventAggregator
                         .GetEvent<SubCategoryRefleshEvent>()
                         .Publish();
        }

        /// <summary>
        /// Alt kategori takip et veya takip bırakma işlemi yapar parametreden gelen isSubCategoryFollowUpStatus true ise takibi bırakır false ise
        /// </summary>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="isSubCategoryFollowUpStatus">O anki alt kategori takip etme durumu</param>
        public async Task SubCategoryFollowProgcess(int subCategoryId, bool isSubCategoryFollowUpStatus)
        {
            bool isOk = await _followCategoryService?.SubCategoryFollowProgcess(subCategoryId, isSubCategoryFollowUpStatus);
            if (isOk) SubCategoryRefleshEvent();
        }

        /// <summary>
        /// Kategori kilidi açık ise EnterPage gider değilse alert çıkarır kilidi aç der
        /// </summary>
        /// <param name="subCategoryModel"></param>
        /// <returns>Kilitli ise kilidi açsınmı isteğini sorar açsın derse true döndürür</returns>
        public async Task<bool> PushEnterPageAsync(SubCategoryModel subCategoryModel)
        {
            if (subCategoryModel.DisplayPrice == "0") GoToEnterPage(subCategoryModel.SubCategoryName, subCategoryModel.PicturePath, subCategoryModel.SubCategoryId);
            else
            {
                bool isUnLock = await OpenSubCategory(subCategoryModel.SubCategoryId);
                if (isUnLock)
                {
                    SubCategoryRefleshEvent();
                    return true;
                }
            }
            return false;
        }

        private void GoToEnterPage(string subCategoryName, string subCategoryPicturePath, int subCategoryId)
        {
            NavigationService?.NavigateAsync(nameof(EnterPage), new NavigationParameters
                                                {
                                                    { "SubCategoryName", subCategoryName },
                                                    { "SubCategoryPicturePath", subCategoryPicturePath },
                                                    { "SubCategoryId", subCategoryId }
                                                }, useModalNavigation: false);
        }

        /// <summary>
        /// Kilitli kategorinin kilidini açmak ister misiniz mesajı
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        private async Task<bool> OpenSubCategory(int subCategoryId)
        {
            bool isOk = await _pageDialogService?.DisplayAlertAsync(ContestParkResources.UnLock + "?",
                                                                   ContestParkResources.CategoryLocked,
                                                                   ContestParkResources.UnLock,
                                                                   ContestParkResources.Cancel);
            if (!isOk)
                return isOk;
            bool isUnLock = await _openSubCategoryService?.OpenCategoryAsync(subCategoryId);
            if (!isUnLock) await BuyDisplayAlertAsync();
            return isUnLock;
        }

        /// <summary>
        /// Altın satın almak istermisiniz sorusunu sorar onaylarsa Contest Store sayfasına gider
        /// </summary>
        private async Task BuyDisplayAlertAsync()
        {
            bool isOk = await _pageDialogService?.DisplayAlertAsync(ContestParkResources.NoGold,
                                                                   ContestParkResources.YouDoNotHaveASufficientAmountOfGoldToOpenThisCategory,
                                                                   ContestParkResources.Buy,
                                                                   ContestParkResources.Cancel);
            NavigationService = RegisterTypesConfig
                                    .Container
                                    .Resolve<INavigationService>();
            if (isOk) await NavigationService?.NavigateAsync(nameof(ContestStorePage), useModalNavigation: false);
        }

        /// <summary>
        /// Düello oluşturma paneli aç
        /// </summary>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="subCategoryName">Alt kategori adı</param>
        /// <returns></returns>
        //public void DuelOpenPanelAsync(int subCategoryId, string subCategoryName)
        //{
        //    MessagingCenter.Send(new MessagingCenterPush(new DuelBettingPopup()), "MessagingCenterPushPopupAsync");
        //}
        /// <summary>
        /// Alt kategori Long press Alert
        /// </summary>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="subCategoryName">Alt kategori adı</param>
        public async Task SubCategoriesDisplayActionSheetAsync(SubCategoryModel subCategoryModel)
        {
            if (subCategoryModel.DisplayPrice == "0")
            {
                bool isSubCategoryFollowUpStatus = await _followCategoryService?.IsFollowUpStatusAsync(subCategoryModel.SubCategoryId);
                string selected = await _pageDialogService?.DisplayActionSheetAsync(ContestParkResources.SelectProcess,
                                                                                   ContestParkResources.Cancel,
                                                                                   "",
                                                                                   //buttons
                                                                                   ContestParkResources.FindOpponent,
                                                                                   ContestParkResources.Ranking,
                                                                                   isSubCategoryFollowUpStatus ? ContestParkResources.UnFollow : ContestParkResources.Follow,
                                                                                   ContestParkResources.Share);
                /*  if (String.Equals(selected, ContestParkResources.FindOpponent)) DuelOpenPanelAsync(subCategoryId, subCategoryName);
                  else*/
                if (String.Equals(selected, ContestParkResources.Ranking)) GotoRankingPage(subCategoryModel.SubCategoryId, subCategoryModel.SubCategoryName);
                else if (String.Equals(selected, ContestParkResources.Follow) || String.Equals(selected, ContestParkResources.UnFollow)) await SubCategoryFollowProgcess(subCategoryModel.SubCategoryId, isSubCategoryFollowUpStatus);
                else if (String.Equals(selected, ContestParkResources.Share)) SubCategoryShare(subCategoryModel.SubCategoryName);
            }
        }

        /// <summary>
        /// İlgili kategorinin sıralama sayfasına gider
        /// </summary>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="subCategoryName">Alt kategori adı</param>
        private void GotoRankingPage(int subCategoryId, string subCategoryName)
        {
            NavigationService?.NavigateAsync($"{nameof(RankingPage)}?SubCategoryId={subCategoryId}&SubCategoryName={subCategoryName}", useModalNavigation: false);
        }

        /// <summary>
        /// Alt kategoriyi paylaş
        /// </summary>
        /// <param name="Title">Alt kategori adı</param>
        public void SubCategoryShare(string Title)
        {
            //string text = Title + " yarışması ContestPark'da";
            //string title = "ContestPark'a gel " + Title + " yarışmasında bilgimizi yarıştıralım";
            //string url = "https://play.google.com/store/apps/details?id=com.contestparkapp.app";
            //CrossShare.Current.Share(new ShareMessage
            //{
            //    Text = text,
            //    Title = title,
            //    Url = url
            //});
        }

        #endregion Methods
    }

    public interface IDuelModule
    {
        Task SubCategoryFollowProgcess(int subCategoryId, bool isSubCategoryFollowUpStatus);

        Task<bool> PushEnterPageAsync(SubCategoryModel subCategoryModel);

        Task SubCategoriesDisplayActionSheetAsync(SubCategoryModel subCategoryModel);

        void SubCategoryShare(string Title);

        INavigationService NavigationService { get; set; }
    }
}