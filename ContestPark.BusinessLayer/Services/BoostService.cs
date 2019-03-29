using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Services
{
    public class BoostService : ServiceBase<Boost>, IBoostService
    {
        #region Private Variables

        private IBoostRepository _boostRepository;
        private ICpService _cpService;

        #endregion Private Variables

        #region Constructors

        public BoostService(
            IBoostRepository boostRepository,
            ICpService cpService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _boostRepository = boostRepository ?? throw new ArgumentNullException(nameof(boostRepository));
            _cpService = cpService ?? throw new ArgumentNullException(nameof(cpService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Joker listesi
        /// </summary>
        /// <returns>Joker listesi döndürür</returns>
        public List<BoostModel> BoostList()
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.BoostManager.BoostList\"");
            return _boostRepository.BoostList();
        }

        /// <summary>
        /// Joker satıl alma
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="boostId">Joker id</param>
        public void BuyBoost(string userId, int boostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.BoostManager.BuyBoost\"");

            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(boostId, nameof(boostId));

            if (!IsBoostIdControl(boostId))
                Check.BadStatus("ServerMessage_theJokerIsNotFound", $"{boostId} {nameof(boostId)} bulunamadı. ServerMessage_theJokerIsNotFound fırlatıldı");

            byte boostGold = FindBoostGold(boostId);
            _cpService.RemoveChip(userId, boostGold, ChipProcessNames.Boost);
        }

        /// <summary>
        /// Jokerin altın miktarı
        /// </summary>
        /// <param name="boostId">Joker Id</param>
        /// <returns>Altın miktarı</returns>
        public byte FindBoostGold(int boostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.BoostManager.FindBoostGold\"");
            Check.IsLessThanZero(boostId, nameof(boostId));
            if (!IsBoostIdControl(boostId))
                Check.BadStatus("ServerMessage_theJokerIsNotFound", $"FindBoostGold({boostId}) boostId bulunamadı. ServerMessage_theJokerIsNotFound fırlatıldı");
            return _boostRepository.FindBoostGold(boostId);
        }

        /// <summary>
        /// Parametreden gelen boost id var mı
        /// </summary>
        /// <param name="boostId">Joker id</param>
        /// <returns>Joker varsa true yoksa false</returns>
        public bool IsBoostIdControl(int boostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.BoostManager.IsBoostIdControl\"");
            return boostId <= 0 ? false : _boostRepository.IsBoostIdControl(boostId);
        }

        #endregion Methods
    }
}