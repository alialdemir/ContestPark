using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.Entities.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class CompletedMissionTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICompletedMissionRepository> _repository;
        private ICompletedMissionService _completedMissionService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ICompletedMissionRepository>();
        }

        private void CreateManager()
        {
            _completedMissionService = new CompletedMissionService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICompletedMissionRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICompletedMissionRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ICompletedMissionService repository = new CompletedMissionService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ICompletedMissionService repository = new CompletedMissionService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region MissionStatus method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissionStatus_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _completedMissionService.MissionStatus(nullUserId, Missions.Mission1);
        }

        #endregion MissionStatus method tests

        #region UserMission method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserMission_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _completedMissionService.UserMission(nullUserId, Missions.Mission1);
        }

        #endregion UserMission method tests
    }
}