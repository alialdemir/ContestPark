using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class FollowsService : ServiceBase, IFollowsService
    {
        #region Constructors

        public FollowsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Login olan kullanıcının takip ettiği kullanıcı listesi
        /// </summary>
        /// <param name="pageModel">Sayfalama</param>
        /// <returns>kullanıcı listesi</returns>
        public async Task<ServiceModel<ChatPeopleModel>> FollowingChatListAsync(string search, PagingModel pageModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ChatPeopleModel>>($"Follows{pageModel.ToString()}&{search}");
            return result.Data;
        }

        /// <summary>
        /// kullanıcının takip ettiği kişiler
        /// </summary>
        /// <param name="followedUserId">Takip edip eden kullanıcı Id</param>
        /// <param name="pageModel">Sayfalama</param>
        /// <returns>Takipçi listesi</returns>
        public async Task<ServiceModel<ListOfFollowerModel>> FollowingAsync(string followedUserId, PagingModel pageModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ListOfFollowerModel>>($"Follows/{followedUserId}/Following{pageModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// kullanıcının Takip edenler(Takipçileri)
        /// </summary>
        /// <param name="followedUserId">Takip edip eden kullanıcı Id</param>
        /// <param name="pageModel">Sayfalama</param>
        /// <returns>Takipçi listesi</returns>
        public async Task<ServiceModel<ListOfFollowerModel>> FollowersAsync(string followedUserId, PagingModel pageModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ListOfFollowerModel>>($"Follows/{followedUserId}/Followers{pageModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takipçi sayısı
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        /// <returns>takipçi sayısı</returns>
        public async Task<int> FollowersCountAsync(string followedUserId)
        {
            var result = await RequestProvider.PostDataAsync<int>($"Follows/FollowersCount", followedUserId);
            return result.Data;
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takip ettiklerinin sayısı
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        /// <returns>takipçi sayısı</returns>
        public async Task<int> FollowUpCountAsync(string followUpUserId)
        {
            var result = await RequestProvider.PostDataAsync<int>($"Follows/FollowUpCount", followUpUserId);
            return result.Data;
        }

        /// <summary>
        /// Login olan kullanıcının parametredene gelen kullanıcıyı takip etme durumu
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        /// <returns>Takip ediyorsa true etmiyor ise false</returns>
        public async Task<bool> IsFollowUpStatusAsync(string followedUserId)
        {
            var result = await RequestProvider.PostDataAsync<bool>($"Follows/IsFollowUpStatus", followedUserId);
            return result.Data;
        }

        /// <summary>
        /// Takibi BIRAK
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        public async Task<bool> UnFollowAsync(string followedUserId)
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"Follows", followedUserId);
            return result.IsSuccess;
        }

        /// <summary>
        /// Takip et
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        public async Task<bool> FollowUpAsync(string followedUserId)
        {
            var result = await RequestProvider.PostDataAsync<bool>($"Follows", followedUserId);
            return result.Data;
        }

        #endregion Methods
    }

    public interface IFollowsService
    {
        Task<ServiceModel<ChatPeopleModel>> FollowingChatListAsync(string search, PagingModel pageModel);

        Task<ServiceModel<ListOfFollowerModel>> FollowingAsync(string followedUserId, PagingModel pageModel);

        Task<ServiceModel<ListOfFollowerModel>> FollowersAsync(string followedUserId, PagingModel pageModel);

        Task<int> FollowersCountAsync(string followedUserId);

        Task<int> FollowUpCountAsync(string followUpUserId);

        Task<bool> IsFollowUpStatusAsync(string followedUserId);

        Task<bool> UnFollowAsync(string followedUserId);

        Task<bool> FollowUpAsync(string followedUserId);
    }
}