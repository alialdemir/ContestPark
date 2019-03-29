using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class OpenSubCategoryManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IOpenSubCategoryRepository> _repository;
        private Mock<ICpService> _CpService;
        private Mock<ISubCategoryService> _subCategoryService;
        private IOpenSubCategoryService _openSubCategoryService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IOpenSubCategoryRepository>();
            _subCategoryService = new Mock<ISubCategoryService>();
            _CpService = new Mock<ICpService>();
        }

        private void CreateManager()
        {
            _openSubCategoryService = new OpenSubCategoryService(_repository.Object, _subCategoryService.Object, _CpService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IOpenSubCategoryRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IOpenSubCategoryRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            _openSubCategoryService = new OpenSubCategoryService(null, _subCategoryService.Object, _CpService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer ISubCategoryService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ISubCategoryService_Null_Expect_ArgumentNullException()
        {
            // Act
            _openSubCategoryService = new OpenSubCategoryService(_repository.Object, null, _CpService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer ICpService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICpService_Null_Expect_ArgumentNullException()
        {
            // Act
            _openSubCategoryService = new OpenSubCategoryService(_repository.Object, _subCategoryService.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            _openSubCategoryService = new OpenSubCategoryService(_repository.Object, _subCategoryService.Object, _CpService.Object, null);
        }

        #endregion Constructors methods tests

        #region OpenCategory method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OpenCategory_userId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _openSubCategoryService.OpenCategory(nullUserId, 1);
        }

        /// <summary>
        /// Eğer subCategoryId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenCategory_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullSubCategoryId = 0;
            // Act
            _openSubCategoryService.OpenCategory("sample-user-id", nullSubCategoryId);
        }

        #endregion OpenCategory method tests
    }
}