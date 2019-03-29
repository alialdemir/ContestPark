using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class CpService : ServiceBase<Cp>, ICpService
    {
        #region Private Variables

        private ICpRepository _CpRepository;
        private ICpInfoService _CpInfoService;

        #endregion Private Variables

        #region Constructors

        public CpService(ICpRepository CpRepository, ICpInfoService CpInfoService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _CpRepository = CpRepository ?? throw new ArgumentNullException(nameof(CpRepository));
            _CpInfoService = CpInfoService ?? throw new ArgumentNullException(nameof(CpInfoService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Altın ekleme
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="addedChips">Eklenecek altın miktarı</param>
        /// <param name="chipProcessName">Altın ekleme işlemi nereden gerçekleşiyor</param>
        public void AddChip(string userId, int addedChips, ChipProcessNames chipProcessName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.AddChip\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            var CpId = _CpRepository.AddChip(userId, addedChips, chipProcessName);
            _CpInfoService.Insert(new CpInfo//Chip işleminin bilgisi eklendi
            {
                ChipProcessName = chipProcessName,
                CpSpent = addedChips,
                CpId = CpId
            });
        }

        /// <summary>
        /// 10000-20000 arası rasgele chip ekler
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Eklenen altın miktarı</returns>
        public int AddRandomChip(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.AddRandomChip\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            DateTime lastDailyChipDateTime = _CpInfoService.LastDailyChipDateTime(userId);
            return _CpRepository.AddRandomChip(userId, lastDailyChipDateTime);
        }

        /// <summary>
        /// Store'dan altın satın alınınca alınan altını ekliyor
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="productId">Product Id</param>
        public void BuyChip(string userId, string productId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.BuyChip\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(productId, nameof(productId));

            _CpRepository.BuyChip(userId, productId);
        }

        /// <summary>
        /// Duello Id'sine göre duellodaki bahis miktarı kadar kullanıcılardan bahisi düşer
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        public void ChipDistribution(int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.ChipDistribution\"");
            Check.IsLessThanZero(duelId, nameof(duelId));

            _CpRepository.ChipDistribution(duelId);
        }

        /// <summary>
        /// Bu method bot ile oynanan duellolarda düello sonunda kurucu kazanırsa bahisini alması için
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        /// <param name="FounderScore">Düello kurucusu puanı</param>
        /// <param name="CompetitorScore">Rakip oyuncu puanı</param>
        public void ChipDistribution(int duelId, byte FounderScore, byte CompetitorScore)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.ChipDistribution\"");
            Check.IsLessThanZero(duelId, nameof(duelId));

            _CpRepository.ChipDistribution(duelId, FounderScore, CompetitorScore);
        }

        /// <summary>
        /// Kullanıcının altınını azaltma
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="diminishingChips">Azaltılcak altın miktarı</param>
        /// <param name="chipProcessName">Nereden azaltıldığı bilgisi</param>
        public void RemoveChip(string userId, int diminishingChips, ChipProcessNames chipProcessName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.RemoveChip\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            int CpId = _CpRepository.RemoveChip(userId, diminishingChips, chipProcessName);
            if (CpId > 0)
            {
                _CpInfoService.Insert(new CpInfo//Chip işleminin bilgisi eklendi
                {
                    ChipProcessName = chipProcessName,
                    CpSpent = diminishingChips,
                    CpId = CpId
                });
            }
        }

        /// <summary>
        /// kullanıcının chip istenilen chipden fazla mı? fazla ise true değilse false
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="amountRequired">Karşılaştırılacak altın miktarı</param>
        /// <returns>kullanıcının chip istenilen chipden fazla mı? fazla ise true değilse false</returns>
        public bool UserChipEquals(string userId, int amountRequired)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.UserChipEquals\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _CpRepository.UserChipEquals(userId, amountRequired);
        }

        /// <summary>
        /// Kullanıcı Id'sine göre toplam altın miktarını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Toplam altın miktarı</returns>
        public int UserTotalCp(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CpManager.UserTotalCp\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _CpRepository.UserTotalCp(userId);
        }

        #endregion Methods
    }
}