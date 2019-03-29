using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class CategoryManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICategoryRepository> _repository;
        private ICategoryService _CategoryService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ICategoryRepository>();
        }

        private void CreateManager()
        {
            _CategoryService = new CategoryService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICategoryRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICategoryRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ICategoryService repository = new CategoryService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ICategoryService repository = new CategoryService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region CategoryList method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CategoryList_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CategoryService.CategoryList(nullUserId, new PagingModel());
        }

        #endregion CategoryList method tests

        #region SearchCategory method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchCategory_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CategoryService.SearchCategory(nullUserId, 1, new PagingModel());
        }

        #endregion SearchCategory method tests
    }
}