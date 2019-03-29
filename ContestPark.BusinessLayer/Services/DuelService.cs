using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class DuelService : ServiceBase<Duel>, IDuelService
    {
        #region Private Variables

        private IDuelRepository _duelRepository;

        #endregion Private Variables

        #region Constructors

        public DuelService(IDuelRepository duelRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _duelRepository = duelRepository ?? throw new ArgumentNullException(nameof(duelRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının düello bilgisini verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>Düello bilgisi</returns>
        public DuelUserInfoModel DuelUserInfo(string userId, int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelManager.DuelUserInfo\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(duelId, nameof(duelId));

            return _duelRepository.DuelUserInfo(userId, duelId);
        }

        /// <summary>
        /// Kullanıcının ilgili kategorideki tamamlanmış düello sayısı eğer _subCategoryId 0 ise tüm kategorilerdeki oyun sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Tamamlanmış toplam düello sayısı</returns>
        public int GameCount(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelManager.GameCount\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _duelRepository.GameCount(userId, subCategoryId);
        }

        /// <summary>
        /// Duellonun kurucusu mu yoksa rakip mi kurucu ise true rakip ise false döner
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>kurucusu mu yoksa rakip mi kurucu ise true rakip ise false döner</returns>
        public bool IsFounder(string userId, int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelManager.IsFounder\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(duelId, nameof(duelId));

            return _duelRepository.IsFounder(userId, duelId);
        }

        #endregion Methods
    }
}