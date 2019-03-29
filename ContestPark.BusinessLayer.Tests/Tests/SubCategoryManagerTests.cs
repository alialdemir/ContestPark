using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class SubCategoryManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISubCategoryRepository> _repository;
        private ISubCategoryService _subCategoryService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ISubCategoryRepository>();
        }

        private void CreateManager()
        {
            _subCategoryService = new SubCategoryService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ISubCategoryRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ISubCategoryRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ISubCategoryService repository = new SubCategoryService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ISubCategoryService repository = new SubCategoryService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region SubCategoryPicture method tests

        /// <summary>
        /// Eğer subCategoryId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DuelResultRanking_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _subCategoryService.SubCategoryPicture(nullsubCategoryId);
        }

        #endregion SubCategoryPicture method tests

        #region SubCategoryPrice method tests

        /// <summary>
        /// Eğer subCategoryId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubCategoryPrice_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _subCategoryService.SubCategoryPrice(nullsubCategoryId);
        }

        #endregion SubCategoryPrice method tests
    }
}