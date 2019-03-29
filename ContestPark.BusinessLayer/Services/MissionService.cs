using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Missions;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class MissionService : ServiceBase<Mission>, IMissionService
    {
        #region Private Variables

        private IMissionRepository _missionRepository;
        private IMissionCreator _missionCreator;
        private ICompletedMissionService _completedMissionService;
        private ICpService _CpService;

        #endregion Private Variables

        #region Constructors

        public MissionService(IMissionRepository missionRepository, IMissionCreator missionCreator, ICompletedMissionService completedMissionService, ICpService CpService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _missionRepository = missionRepository ?? throw new ArgumentNullException(nameof(missionRepository));
            _missionCreator = missionCreator ?? throw new ArgumentNullException(nameof(missionCreator));
            _completedMissionService = completedMissionService ?? throw new ArgumentNullException(nameof(completedMissionService));
            _CpService = CpService ?? throw new ArgumentNullException(nameof(CpService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Görev tamamla ekleme işlemi
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="mission">Görev id</param>
        private void InsertCompletedMission(string userId, Entities.Enums.Missions mission)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.MissionManager.InsertCompletedMission\"");
            _completedMissionService
                .Insert(new CompletedMission
                {
                    UserId = userId,
                    MissionId = (int)mission,
                    MissionComplate = false// False ise görevin verdiği altın alınmamıştır demek oluyor.
                });
        }

        /// <summary>
        /// Bu method kullanıcının görevini tamamlamasına yarar ilgili görev burada yazılır ve kontrol edilmesi istenilen yerden çağırılır
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="missions">Görevlerin id'leri</param>
        public void MissionComplete(string userId, params Missions[] missions)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.MissionManager.MissionComplete\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            if (missions == null || missions.Count() <= 0)
                throw new ArgumentNullException(nameof(missions));

            //Task.Run(() =>
            //{
            IMission[] missionsList = _missionCreator.MissionFactory(missions);

            foreach (IMission mission in missionsList)
            {
                bool taskStatus = _completedMissionService.MissionStatus(userId, mission.Mission);
                if (!taskStatus)//Görev yapılmamışsa
                {
                    bool isComplete = mission.MissionComplete(userId);
                    if (isComplete)
                        InsertCompletedMission(userId, mission.Mission);
                }
            }
            //});
        }

        /// <summary>
        /// Parametreden gelen görevin verdiği altın miktirı
        /// </summary>
        /// <param name="mission">Görev id</param>
        /// <returns>Görevin altın miktarı</returns>
        public int MissionGold(Missions task)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.MissionManager.MissionGold\"");
            return _missionRepository.MissionGold(task);
        }

        /// <summary>
        /// Kullanıcının görevlerini listeler
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görev listesi</returns>
        public MissionListModel MissionList(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.MissionManager.MissionList\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _missionRepository.MissionList(userId, pagingModel);
        }

        /// <summary>
        /// Tamamladığı görevin altınını alma
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="mission">Görev id</param>
        public void TakesMissionGold(string userId, Missions mission)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.MissionManager.TakesMissionGold\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            if (_completedMissionService.MissionStatus(userId, mission))
            {
                CompletedMission taskComplete = _completedMissionService.UserMission(userId, mission);
                taskComplete.MissionComplate = true;
                _completedMissionService.Update(taskComplete);

                int taskGold = MissionGold(mission);
                _CpService.AddChip(userId, taskGold, ChipProcessNames.Task);
            }
        }

        #endregion Methods
    }
}