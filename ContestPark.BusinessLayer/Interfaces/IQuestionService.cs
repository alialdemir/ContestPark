using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IQuestionService : IRepository<Question>
    {
        List<RandomQuestioInfoModel> RandomQuestion(string userId, string competitorUserId, int subCategoryId);

        RandomQuestionModel UnansweredQuestions(string userId, int questionId, Stylish? trueAnswerIndex);

        List<DuelQuestionModel> DuelQuestions(string userId, int duelId);

        TrueAnswerControlModel TrueAnswerControl(int questionId, Stylish stylish, string userId, int duelInfoId, byte scorePoint, int subCategoryId);

        decimal SolvedQuestions(string userId, int subCategoryId);
    }
}