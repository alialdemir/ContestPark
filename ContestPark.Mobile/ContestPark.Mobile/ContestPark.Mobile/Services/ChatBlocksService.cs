using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services.Base;
using Prism.Services;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class ChatBlocksService : ServiceBase, IChatBlocksService
    {
        #region Private varaibles

        private readonly IPageDialogService _pageDialogService;

        #endregion Private varaibles

        #region Constructors

        public ChatBlocksService(IRequestProvider requestProvider,
                                 IPageDialogService pageDialogService) : base(requestProvider)
        {
            _pageDialogService = pageDialogService;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının engellediği kullanıcıların lsitesini döndürür
        /// </summary>
        /// <returns>Engellenenler listesi</returns>
        public async Task<ServiceModel<UserBlockListModel>> UserBlockListAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<UserBlockListModel>>($"ChatBlocks{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// İki kullanıcı arasında engelleme var mı kontrol eder
        /// </summary>
        public async Task<bool> BlockingStatusAsync(string senderUserId)
        {
            var result = await RequestProvider.GetDataAsync<bool>($"ChatBlocks/{senderUserId}/BlockingStatus");
            return result.Data;
        }

        /// <summary>
        /// Parametreden gelen kullanıcı engelle
        /// </summary>
        /// <param name="fullName">kullanıcı adı</param>
        /// <param name="senderUserId">kullanıcı Id</param>
        /// <returns></returns>
        private async Task<bool> UserBlockingAsync(string fullName, string senderUserId)
        {
            bool isOk = await _pageDialogService.DisplayAlertAsync(
                   ContestParkResources.ConfirmBlock,
                   fullName + ContestParkResources.ConfirmBlockTitle,
                   ContestParkResources.Block,
                   ContestParkResources.Cancel);
            if (!isOk) return isOk;
            var response = await RequestProvider.PostDataAsync<string>($"ChatBlocks/{senderUserId}");
            return response.IsSuccess;
        }

        /// <summary>
        /// İki kullanıcı arasında engeleme varsa engeli kaldır çalışır engelleme yoksa engelle çalışır
        /// </summary>
        public async Task<bool> BlockingProgressAsync(string fullName, string senderUserId, bool isBlocking)
        {
            if (string.IsNullOrEmpty(senderUserId)) return false;
            else if (isBlocking) return await UnBlockAsync(senderUserId);
            else return await UserBlockingAsync(fullName, senderUserId);
        }

        /// <summary>
        /// Parametreden gelen kullanıcı id'nin engellini kaldır
        /// </summary>
        /// <param name="whonId">kullanıcı Id</param>
        /// <returns></returns>
        private async Task<bool> UnBlockAsync(string senderUserId)
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"ChatBlocks/{senderUserId}");
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface IChatBlocksService
    {
        Task<ServiceModel<UserBlockListModel>> UserBlockListAsync(PagingModel pagingModel);

        Task<bool> BlockingStatusAsync(string senderUserId);

        Task<bool> BlockingProgressAsync(string fullName, string senderUserId, bool isBlocking);
    }
}