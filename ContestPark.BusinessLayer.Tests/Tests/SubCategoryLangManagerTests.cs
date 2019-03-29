using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class SubCategoryLangManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISubCategoryLangRepository> _repository;
        private ISubCategoryLangService _subCategoryLangService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ISubCategoryLangRepository>();
        }

        private void CreateManager()
        {
            _subCategoryLangService = new SubCategoryLangService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ISubCategoryLangRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ISubCategoryLangRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ISubCategoryLangService repository = new SubCategoryLangService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ISubCategoryLangService repository = new SubCategoryLangService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region ISubCategoryLangRepository method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubCategoryNameByLanguage_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _subCategoryLangService.SubCategoryNameByLanguage(nullUserId, 1);
        }

        /// <summary>
        /// Eğer subCategoryId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubCategoryNameByLanguage_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _subCategoryLangService.SubCategoryNameByLanguage("sample-user-id", nullsubCategoryId);
        }

        #endregion ISubCategoryLangRepository method tests
    }
}