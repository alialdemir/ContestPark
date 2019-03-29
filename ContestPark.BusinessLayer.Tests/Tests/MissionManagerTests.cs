using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Missions;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class MissionManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMissionRepository> _repository;
        private Mock<IMissionCreator> _missionCreator;
        private Mock<ICompletedMissionService> _completedMissionService;
        private Mock<ICpService> _CpService;
        private IMissionService _missionService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IMissionRepository>();
            _CpService = new Mock<ICpService>();
            _missionCreator = new Mock<IMissionCreator>();
            _completedMissionService = new Mock<ICompletedMissionService>();
        }

        private void CreateManager()
        {
            _missionService = new MissionService(_repository.Object, _missionCreator.Object, _completedMissionService.Object, _CpService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IMissionRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IMissionRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IMissionService repository = new MissionService(null, _missionCreator.Object, _completedMissionService.Object, _CpService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IMissionCreator null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IMissionCreator_Null_Expect_ArgumentNullException()
        {
            // Act
            IMissionService repository = new MissionService(_repository.Object, null, _completedMissionService.Object, _CpService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer ICompletedMissionService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICompletedMissionService_Null_Expect_ArgumentNullException()
        {
            // Act
            IMissionService repository = new MissionService(_repository.Object, _missionCreator.Object, null, _CpService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer ICpService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICpService_Null_Expect_ArgumentNullException()
        {
            // Act
            IMissionService repository = new MissionService(_repository.Object, _missionCreator.Object, _completedMissionService.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IMissionService repository = new MissionService(_repository.Object, _missionCreator.Object, _completedMissionService.Object, _CpService.Object, null);
        }

        #endregion Constructors methods tests

        #region MissionComplete method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissionComplete_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _missionService.MissionComplete(nullUserId, Missions.Mission1);
        }

        /// <summary>
        /// Eğer missions null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissionComplete_missions_Count_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            // Act
            _missionService.MissionComplete("sample-user-id");
        }

        #endregion MissionComplete method tests

        #region MissionGold method tests

        /// <summary>
        /// Görevlerin altın miktarları test edildi Missions enumundaki tüm görevleri 100 altın ile kontrol ettim
        /// </summary>
        [DataTestMethod]
        [DataRow(Missions.Mission1, 100)]
        [DataRow(Missions.Mission2, 100)]
        [DataRow(Missions.Mission3, 100)]
        [DataRow(Missions.Mission4, 100)]
        [DataRow(Missions.Mission5, 100)]
        [DataRow(Missions.Mission6, 100)]
        [DataRow(Missions.Mission7, 100)]
        [DataRow(Missions.Mission8, 100)]
        [DataRow(Missions.Mission9, 100)]
        [DataRow(Missions.Mission10, 100)]
        [DataRow(Missions.Mission11, 100)]
        [DataRow(Missions.Mission12, 100)]
        [DataRow(Missions.Mission13, 100)]
        [DataRow(Missions.Mission14, 100)]
        [DataRow(Missions.Mission15, 100)]
        [DataRow(Missions.Mission16, 100)]
        [DataRow(Missions.Mission17, 100)]
        [DataRow(Missions.Mission18, 100)]
        [DataRow(Missions.Mission19, 100)]
        [DataRow(Missions.Mission20, 100)]
        [DataRow(Missions.Mission21, 100)]
        [DataRow(Missions.Mission22, 100)]
        [DataRow(Missions.Mission23, 100)]
        [DataRow(Missions.Mission24, 100)]
        [DataRow(Missions.Mission25, 100)]
        [DataRow(Missions.Mission26, 100)]
        [DataRow(Missions.Mission27, 100)]
        [DataRow(Missions.Mission28, 100)]
        [DataRow(Missions.Mission29, 100)]
        [DataRow(Missions.Mission30, 100)]
        [DataRow(Missions.Mission31, 100)]
        [DataRow(Missions.Mission32, 100)]
        [DataRow(Missions.Mission33, 100)]
        [DataRow(Missions.Mission34, 100)]
        [DataRow(Missions.Mission35, 100)]
        [DataRow(Missions.Mission36, 100)]
        public void MissionGold_MissionId_1_Expect_Exception(Missions mission, int gold)
        {
            // Arrange
            _repository.Setup(x => x.MissionGold(It.IsAny<Missions>())).Returns(gold);
            CreateManager();
            // Act
            int missionGold = _missionService.MissionGold(mission);
            // Assert
            Assert.AreEqual(gold, missionGold);
        }

        #endregion MissionGold method tests

        #region MissionList method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissionList_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _missionService.MissionList(nullUserId, new PagingModel());
        }

        #endregion MissionList method tests

        #region TakesMissionGold method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakesMissionGold_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _missionService.TakesMissionGold(nullUserId, Missions.Mission1);
        }

        #endregion TakesMissionGold method tests
    }
}