using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IDuelInfoService : IRepository<DuelInfo>
    {
        DuelEnterScreenModel DuelStartRandom(string founderUserId, int subCategoryId);

        DuelEnterScreenModel DuelEnterScreen(string founderUserId, string competitorUserId, int subCategoryId, bool notificationStatus, int Cp);

        DuelEnterScreenModel DuelEnterScreen(int duelId);

        void AcceptsDuelWithNotification(string competitorUserId, int notificationId);

        void SmotherDuel(string userId, int duelId);

        RandomQuestionModel NextDuelStart(int duelId, string userId);

        RandomQuestionModel DuelQuestionControl(int duelId, string userId);

        TrueAnswerControlModel TrueAnswerControl(string DuelInfoId, byte time, int questionId, Stylish stylish, string userId);

        DuelResultModel DuelResult(int duelId);

        StatisticsInfoModel SelectedContestStatistics(string userName, int subCateogryId);

        StatisticsInfoModel GlobalStatisticsInfo(string userName);

        ServiceModel<ContestMostPlayModel> ContestMostPlay(string userName, PagingModel pagingModel);

        decimal SolvedQuestions(string userId, int subCategoryId);

        string RandomCompetitorUserId(string founderUserId);

        AudienceAnswersModel AudienceAnswers(int questionId);
    }
}