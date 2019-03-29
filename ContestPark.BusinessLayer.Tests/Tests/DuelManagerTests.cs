using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class DuelManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IDuelRepository> _repository;
        private IDuelService _duelService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IDuelRepository>();
        }

        private void CreateManager()
        {
            _duelService = new DuelService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IDuelRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IDuelRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IDuelService repository = new DuelService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IDuelService repository = new DuelService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region DuelUserInfo method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuelUserInfo_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelService.DuelUserInfo(nullUserId, 1);
        }

        /// <summary>
        /// Eğer duelId null gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DuelUserInfo_DuelId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullDuelId = 0;
            // Act
            _duelService.DuelUserInfo("deneme-user-id", nullDuelId);
        }

        #endregion DuelUserInfo method tests

        #region GameCount method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GameCount_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelService.GameCount(nullUserId, 1);
        }

        #endregion GameCount method tests

        #region IsFounder method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsFounder_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelService.IsFounder(nullUserId, 1);
        }

        /// <summary>
        /// Eğer duelId null gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsFounder_DuelId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullDuelId = 0;
            // Act
            _duelService.IsFounder("deneme-user-id", nullDuelId);
        }

        #endregion IsFounder method tests
    }
}