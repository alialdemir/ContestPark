using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class CpInfoService : ServiceBase<CpInfo>, ICpInfoService
    {
        #region Private Variables

        private ICpInfoRepository _CpInfoRepository;

        #endregion Private Variables

        #region Constructors

        public CpInfoService(ICpInfoRepository CpInfoRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _CpInfoRepository = CpInfoRepository ?? throw new ArgumentNullException(nameof(CpInfoRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// En son günlük aldığı altının tarihini verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>En son aldığı günlük altın tarihi</returns>
        public DateTime LastDailyChipDateTime(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpInfoManager.LastDailyChipDateTime\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _CpInfoRepository.LastDailyChipDateTime(userId);
        }

        #endregion Methods
    }
}