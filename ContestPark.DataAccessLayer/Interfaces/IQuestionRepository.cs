using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        List<RandomQuestioInfoModel> RandomQuestion(string userId, string competitorUserId, int subCategoryId);

        RandomQuestionModel UnansweredQuestions(int questionId, Stylish? trueAnswerIndex);

        List<DuelQuestionModel> DuelQuestions(int duelId);

        TrueAnswerControlModel TrueAnswerControl(int questionId, Stylish stylish, string userId, int duelInfoId, byte scorePoint);

        decimal SolvedQuestions(string userId, int subCategoryId);
    }
}