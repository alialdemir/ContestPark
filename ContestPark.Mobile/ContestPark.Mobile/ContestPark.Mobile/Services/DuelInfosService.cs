using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class DuelInfosService : ServiceBase, IDuelInfosService
    {
        #region Constructors

        public DuelInfosService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        public async Task<DuelEnterScreenModel> DuelPlayAsync()//Rasgele rakip bulma
        {
            var result = await RequestProvider.PostDataAsync<DuelEnterScreenModel>($"DuelInfos/DuelPlay");
            return result.Data;
        }

        public async Task<bool> CloseDuelAsync()//Rasgele rakip bulma
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"DuelInfos/CloseDuel");
            return result.IsSuccess;
        }

        public async Task<bool> OpenDuel(int subCategoryId, int Cp)//Rasgele rakip bulma
        {
            var result = await RequestProvider.PostDataAsync<string>($"DuelInfos/OpenDuel/{subCategoryId}?Cp={Cp}");
            return result.IsSuccess;
        }

        public async Task<AudienceAnswersModel> AudienceAnswersAsync(int questionId)
        {
            var result = await RequestProvider.PostDataAsync<AudienceAnswersModel>($"DuelInfos/AudienceAnswers/{questionId}");
            return result.Data;
        }

        public async Task<int> GameCountAsync(string userId, int subCategoryId = 0)
        {
            var result = await RequestProvider.GetDataAsync<int>($"DuelInfos/GameCount/{userId}/{subCategoryId}");
            return result.Data;
        }

        //public async Task<DuelEnterScreenModel> DuelEnterScreenGetByDuelIdAsync(int duelId, int subCategoryId)//Düello başlıyor ekranı
        //{
        //    var result = await RequestProvider.PostDataAsync<DuelEnterScreenModel>($"DuelInfos/DuelEnterScreenGetByDuelId/{duelId}/{subCategoryId}");
        //    return result.Data;
        //}
        //public async Task<DuelEnterScreenModel> DuelEnterScreenAsync(string competitorUserId, int subCategoryId)//Düello başlıyor ekranı
        //{
        //    var result = await RequestProvider.PostDataAsync<DuelEnterScreenModel>($"DuelInfos/DuelEnterScreen/{subCategoryId}", competitorUserId);
        //    return result.Data;
        //}
        public async Task<bool> AcceptsDuelWithNotificationAsync(int notificationId, int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"DuelInfos/AcceptsDuelWithNotification/{notificationId}/{subCategoryId}");
            return result.IsSuccess;
        }

        public async Task<bool> SmotherDuelAsync(int duelId, int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"DuelInfos/SmotherDuel/{duelId}/{subCategoryId}");
            return result.IsSuccess;
        }

        public async Task<decimal> SolvedQuestionsAsync(int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<decimal>($"DuelInfos/SolvedQuestions/{subCategoryId}");
            return result.Data;
        }

        public async Task<DuelUserInfoModel> DuelUserInfoAsync(int duelId, int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<DuelUserInfoModel>($"DuelInfos/DuelUserInfo/{duelId}/{subCategoryId}");
            return result.Data;
        }

        public async Task<DuelResultModel> DuelResultAsync(int duelId, int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<DuelResultModel>($"DuelInfos/DuelResult/{duelId}/{subCategoryId}");
            return result.Data;
        }

        public async Task<StatisticsInfoModel> SelectedContestStatisticsAsync(string userName, int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<StatisticsInfoModel>($"DuelInfos/SelectedContestStatistics/{subCategoryId}");
            return result.Data;
        }

        public async Task<IEnumerable<ContestMostPlayModel>> ContestMostPlayAsync(string userName, PagingModel pagingModel)
        {
            var result = await RequestProvider.PostDataAsync<IEnumerable<ContestMostPlayModel>>($"DuelInfos/ContestMostPlay{pagingModel.ToString()}", userName);
            return result.Data;
        }

        public async Task<StatisticsInfoModel> GlobalStatisticsInfoAsync(string userName)
        {
            var result = await RequestProvider.PostDataAsync<StatisticsInfoModel>($"DuelInfos/GlobalStatisticsInfo", userName);
            return result.Data;
        }

        #endregion Methods
    }

    public interface IDuelInfosService
    {
        Task<DuelEnterScreenModel> DuelPlayAsync();

        Task<bool> CloseDuelAsync();

        Task<bool> OpenDuel(int subCategoryId, int Cp);

        Task<AudienceAnswersModel> AudienceAnswersAsync(int questionId);

        Task<int> GameCountAsync(string userId, int subCategoryId = 0);

        Task<bool> AcceptsDuelWithNotificationAsync(int notificationId, int subCategoryId);

        Task<bool> SmotherDuelAsync(int duelId, int subCategoryId);

        Task<decimal> SolvedQuestionsAsync(int subCategoryId);

        Task<DuelUserInfoModel> DuelUserInfoAsync(int duelId, int subCategoryId);

        Task<DuelResultModel> DuelResultAsync(int duelId, int subCategoryId);

        Task<StatisticsInfoModel> SelectedContestStatisticsAsync(string userName, int subCategoryId);

        Task<IEnumerable<ContestMostPlayModel>> ContestMostPlayAsync(string userName, PagingModel pagingModel);

        Task<StatisticsInfoModel> GlobalStatisticsInfoAsync(string userName);
    }
}