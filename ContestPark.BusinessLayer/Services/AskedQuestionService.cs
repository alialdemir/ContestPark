using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Services
{
    public class AskedQuestionService : ServiceBase<AskedQuestion>, IAskedQuestionService
    {
        #region Private Variables

        private IAskedQuestionRepository _askedQuestion;

        #endregion Private Variables

        #region Constructors

        public AskedQuestionService(
            IAskedQuestionRepository askedQuestion,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _askedQuestion = askedQuestion ?? throw new ArgumentNullException(nameof(askedQuestion));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Sorulan soruları silme
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        public async Task DeleteAsync(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.AskedQuestionManager.DeleteAsync\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));
            await _askedQuestion.DeleteAsync(userId, subCategoryId);
        }

        #endregion Methods
    }
}