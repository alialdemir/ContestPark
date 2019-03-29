using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IScoreService : IRepository<Score>
    {
        ServiceModel<ScoreRankingModel> ScoreRanking(int subCategoryId, PagingModel pagingModel);

        ServiceModel<ScoreRankingModel> ScoreRankingFollowing(string userId, int subCategoryId, PagingModel pagingModel);

        ScoreRankingModel UserTotalScore(string userId, int subCategoryId);

        List<ScoreRankingModel> FacebookFriendRanking(string userId, int subCategoryId, List<FacebookFriendRankingModel> facebookFriendRanking);

        DuelResultRankingModel DuelResultRanking(string userId, int subCategoryId);
    }
}