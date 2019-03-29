using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class QuestionsService : ServiceBase, IQuestionsService
    {
        #region Constructors

        public QuestionsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Duellodaki soruları getirir
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Düelloda sorulan soruların listesi</returns>
        public async Task<IEnumerable<DuelQuestionModel>> DuelQuestionsAsync(int duelId, int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<IEnumerable<DuelQuestionModel>>($"Questions/{duelId}/{subCategoryId}");
            return result.Data;
        }

        #endregion Methods
    }

    public interface IQuestionsService
    {
        Task<IEnumerable<DuelQuestionModel>> DuelQuestionsAsync(int duelId, int subCategoryId);
    }
}