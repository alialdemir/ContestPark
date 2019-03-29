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
    public class CpManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICpRepository> _repository;
        private Mock<ICpInfoService> _CpInfoService;
        private ICpService _CpService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ICpRepository>();
            _CpInfoService = new Mock<ICpInfoService>();
        }

        private void CreateManager()
        {
            _CpService = new CpService(_repository.Object, _CpInfoService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICoverPictureRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICoverPictureRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ICpService repository = new CpService(null, _CpInfoService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer ICpInfoService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICpInfoService_Null_Expect_ArgumentNullException()
        {
            // Act
            ICpService repository = new CpService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ICpService repository = new CpService(_repository.Object, _CpInfoService.Object, null);
        }

        #endregion Constructors methods tests

        #region AddChip method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddChip_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpService.AddChip(nullUserId, 0, ChipProcessNames.Boost);
        }

        #endregion AddChip method tests

        #region AddRandomChip method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRandomChip_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpService.AddRandomChip(nullUserId);
        }

        #endregion AddRandomChip method tests

        #region BuyChip method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuyChip_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpService.BuyChip(nullUserId, "deneme-product-id");
        }

        /// <summary>
        /// Eğer productId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuyChip_ProductId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullProductId = String.Empty;
            // Act
            _CpService.BuyChip("deneme-user-id", nullProductId);
        }

        #endregion BuyChip method tests

        #region ChipDistribution method tests

        /// <summary>
        /// Eğer duelId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChipDistribution_DuelId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullDuelId = 0;
            // Act
            _CpService.ChipDistribution(nullDuelId);
        }

        /// <summary>
        /// Eğer duelId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChipDistribution_Override_DuelId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullDuelId = 0;
            // Act
            _CpService.ChipDistribution(nullDuelId, 1, 1);
        }

        #endregion ChipDistribution method tests

        #region RemoveChip method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveChip_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpService.RemoveChip(nullUserId, 0, ChipProcessNames.Buy);
        }

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void RemoveChip_RemoveChip_Return_1_Expect_Exception()
        {
            // Arrange
            _repository.Setup(x => x.RemoveChip(It.IsAny<string>(), 0, ChipProcessNames.Buy)).Returns(1);
            CreateManager();
            // Act
            _CpService.RemoveChip("deneme-user-id", 0, ChipProcessNames.Buy);
        }

        #endregion RemoveChip method tests

        #region UserChipEquals method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserChipEquals_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpService.UserChipEquals(nullUserId, 1);
        }

        #endregion UserChipEquals method tests

        #region UserTotalCp method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserTotalCp_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpService.UserTotalCp(nullUserId);
        }

        #endregion UserTotalCp method tests
    }
}