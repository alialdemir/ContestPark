using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Services
{
    public class ScoreService : ServiceBase<Score>, IScoreService
    {
        #region Private Variables

        private IScoreRepository _scoreRepository;

        #endregion Private Variables

        #region Constructors

        public ScoreService(IScoreRepository scoreRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _scoreRepository = scoreRepository ?? throw new ArgumentNullException(nameof(scoreRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Düello sonucu ekranında kendi sırasının 2 fazla ve 2  eksiğini gösterir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kullanıcının 2 alt ve 2 üst sıralaması</returns>
        public DuelResultRankingModel DuelResultRanking(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ScoreManager.DuelResultRanking\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _scoreRepository.DuelResultRanking(userId, subCategoryId);
        }

        /// <summary>
        /// Kullanıcının facebook arkadaş listesindeki kullanıcı bizde ekli ise o kategorisindeki puan ve sırasını döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="facebookFriendRanking">Facebook Id'leri</param>
        /// <returns>Facebook arkadaşları puan sıralaması</returns>
        public List<ScoreRankingModel> FacebookFriendRanking(string userId, int subCategoryId, List<FacebookFriendRankingModel> facebookFriendRanking)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ScoreManager.FacebookFriendRanking\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));
            Check.IsNull(facebookFriendRanking, nameof(facebookFriendRanking));

            return _scoreRepository.FacebookFriendRanking(userId, subCategoryId, facebookFriendRanking);
        }

        /// <summary>
        /// Alt kategoriye göre sıralma listesi getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="paging">Sayfalama</param>
        /// <param name="take">Arasındaki data</param>
        /// <returns>Sıralama listesi</returns>
        public ServiceModel<ScoreRankingModel> ScoreRanking(int subCategoryId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ScoreManager.ScoreRanking\"");
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _scoreRepository.ScoreRanking(subCategoryId, pagingModel);
        }

        /// <summary>
        /// Kullanıcının takip ettiği arkadaşlarının sıralamadaki durumunu verir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="paing">Sayfalama</param>
        /// <returns>Sıralama listesi</returns>
        public ServiceModel<ScoreRankingModel> ScoreRankingFollowing(string userId, int subCategoryId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ScoreManager.ScoreRankingFollowing\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _scoreRepository.ScoreRankingFollowing(userId, subCategoryId, pagingModel);
        }

        /// <summary>
        /// Kullanıcının belirtilen yarışmadaki sıralam durumunu verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kullanıcının sıralama durumu</returns>
        public ScoreRankingModel UserTotalScore(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ScoreManager.UserTotalScore\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _scoreRepository.UserTotalScore(userId, subCategoryId);
        }

        #endregion Methods
    }
}