using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class SettingManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISettingRepository> _repository;
        private ISettingService _settingService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ISettingRepository>();
        }

        private void CreateManager()
        {
            _settingService = new SettingService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ISettingRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ISettingRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ISettingService repository = new SettingService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ISettingService repository = new SettingService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region Update method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserChatList_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _settingService.Update(nullUserId, "sample-value", 1);
        }

        /// <summary>
        /// Eğer Value null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserChatList_Value_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullValue = String.Empty;
            // Act
            _settingService.Update("sample-user-id", nullValue, 1);
        }

        /// <summary>
        /// Eğer settingTypeId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UserChatList_SettingTypeId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            byte zeroValue = 0;
            // Act
            _settingService.Update("sample-user-id", "sample-value", zeroValue);
        }

        #endregion Update method tests

        #region GetSettingValue method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSettingValue_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _settingService.GetSettingValue(nullUserId, 1);
        }

        /// <summary>
        /// Eğer settingTypeId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetSettingValue_SettingTypeId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            byte nullsubCategoryId = 0;
            // Act
            _settingService.GetSettingValue("sample-user-id", nullsubCategoryId);
        }

        #endregion GetSettingValue method tests
    }
}