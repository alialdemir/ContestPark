using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class CompletedMissionService : ServiceBase<CompletedMission>, ICompletedMissionService
    {
        #region Private Variables

        private ICompletedMissionRepository _completedMissionRepository;

        #endregion Private Variables

        #region Constructors

        public CompletedMissionService(ICompletedMissionRepository completedMissionRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _completedMissionRepository = completedMissionRepository ?? throw new ArgumentNullException(nameof(completedMissionRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Görevin yapılma durumu
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="task">Görev tipi</param>
        /// <returns>Görevi yapmış ise true yapmamış ise false</returns>
        public bool MissionStatus(string userId, Missions mission)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CompletedMissionManager.MissionStatus\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _completedMissionRepository.MissionStatus(userId, mission);
        }

        /// <summary>
        /// Kullanıcının ilgili görevini geri döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="mission">Görev tipi</param>
        /// <returns>Tamamlanan görev entity</returns>
        public CompletedMission UserMission(string userId, Missions mission)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CompletedMissionManager.UserMission\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _completedMissionRepository.UserMission(userId, mission);
        }

        #endregion Methods
    }
}