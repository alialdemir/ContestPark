using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class ScoresService : ServiceBase, IScoresService
    {
        #region Constructors

        public ScoresService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Alt kategori Id'ye göre sýralama getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="pagingModel">Sayfalama</param>
        /// <returns>Sýralama listesi</returns>
        public async Task<ServiceModel<ScoreRankingModel>> ScoreRankingAsync(int subCategoryId, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ScoreRankingModel>>($"Scores/{subCategoryId}{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Alt kategori Id'ye göre login olan kullanicinin 2 alt ve 2 üst sýrasýndaki kullanicilarýn sýralama olarak döndürür
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Sýralama listesi</returns>
        public async Task<DuelResultRankingModel> DuelResultRankingAsync(int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<DuelResultRankingModel>($"Scores/{subCategoryId}/DuelResultRanking");
            return result.Data;
        }

        /// <summary>
        /// kullanicinin facebook arkadaþ listesindeki kullanici bizde ekli ise o kategorisindeki puan ve sýrasýný döndürür
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="facebookFriendRanking">Facebook arkadaþ listesi</param>
        /// <returns>Sýralama listesi</returns>
        public async Task<IEnumerable<ScoreRankingModel>> FacebookFriendRankingAsync(int subCategoryId, string facebookFriendRanking)
        {
            var result = await RequestProvider.PostDataAsync<IEnumerable<ScoreRankingModel>>($"Scores/{subCategoryId}/FacebookFriendRanking", facebookFriendRanking);
            return result.Data;
        }

        /// <summary>
        /// Login olan kullanicinin o kategorideki puanýný döndürür
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Puan</returns>
        public async Task<ScoreRankingModel> UserTotalScoreAsync(int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<ScoreRankingModel>($"Scores/{subCategoryId}/UserTotalScore");
            return result.Data;
        }

        /// <summary>
        /// kullanicinin takip ettigi arkadaþlarýnýn sýralamadaki durumunu verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="pagingModel">Sayfalama</param>
        /// <returns>Takip ettiklerinin sýralama listesi</returns>
        public async Task<ServiceModel<ScoreRankingModel>> ScoreRankingFollowingAsync(int subCategoryId, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<ScoreRankingModel>>($"Scores/{subCategoryId}/ScoreRankingFollowing{pagingModel.ToString()}");
            return result.Data;
        }

        #endregion Methods
    }

    public interface IScoresService
    {
        Task<ServiceModel<ScoreRankingModel>> ScoreRankingAsync(int subCategoryId, PagingModel pagingModel);

        Task<DuelResultRankingModel> DuelResultRankingAsync(int subCategoryId);

        Task<IEnumerable<ScoreRankingModel>> FacebookFriendRankingAsync(int subCategoryId, string facebookFriendRanking);

        Task<ScoreRankingModel> UserTotalScoreAsync(int subCategoryId);

        Task<ServiceModel<ScoreRankingModel>> ScoreRankingFollowingAsync(int subCategoryId, PagingModel pagingModel);
    }
}