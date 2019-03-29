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
    public class LanguageManagerTests
    {
        #region Field

        private Mock<ILanguageRepository> _repository;
        private Mock<ISettingService> _settingService;
        private Mock<IUnitOfWork> _unitOfWork;
        private ILanguageService _languageService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ILanguageRepository>();
            _settingService = new Mock<ISettingService>();
        }

        private void CreateManager()
        {
            _languageService = new LanguageService(_repository.Object, _settingService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ILanguageRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ILanguageRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ILanguageService repository = new LanguageService(null, _settingService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer ISettingService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ISettingService_Null_Expect_ArgumentNullException()
        {
            // Act
            ILanguageService repository = new LanguageService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ILanguageService repository = new LanguageService(_repository.Object, _settingService.Object, null);
        }

        #endregion Constructors methods tests

        #region GetUserLangId method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUserLangId_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            // Act
            string nullUserId = String.Empty;
            _languageService.GetUserLangId(nullUserId);
        }

        /// <summary>
        /// Eğer veribanın da dil bilgisi kayıtlı ise türkçe dönüyor mu
        /// </summary>
        [TestMethod]
        public void GetUserLangId_Language_Control_Expect_Languages_Turkish()
        {
            // Arrange
            _repository.Setup(x => x.GetUserLangId(It.IsAny<string>())).Returns((byte)Languages.Turkish);
            CreateManager();
            // Act
            Languages turkishLanguage = _languageService.GetUserLangId("sample-user-id");
            // Assert
            Assert.AreEqual(Languages.Turkish, turkishLanguage);
        }

        #endregion GetUserLangId method tests
    }
}