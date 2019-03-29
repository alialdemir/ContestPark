using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class FollowCategoryManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IFollowCategoryRepository> _repository;
        private Mock<IOpenSubCategoryService> _openSubCategoryService;
        private IFollowCategoryService _followCategoryService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IFollowCategoryRepository>();
            _openSubCategoryService = new Mock<IOpenSubCategoryService>();
        }

        private void CreateManager()
        {
            _followCategoryService = new FollowCategoryService(_repository.Object, _openSubCategoryService.Object, _unitOfWork.Object);
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
            IFollowCategoryService repository = new FollowCategoryService(null, _openSubCategoryService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IOpenSubCategoryService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IOpenSubCategoryService_Null_Expect_ArgumentNullException()
        {
            // Act
            IFollowCategoryService repository = new FollowCategoryService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IFollowCategoryService repository = new FollowCategoryService(_repository.Object, _openSubCategoryService.Object, null);
        }

        #endregion Constructors methods tests

        #region Delete method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _followCategoryService.Delete(nullUserId, 1);
        }

        /// <summary>
        /// Eğer subCategoryId O gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Delete_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroSubCategoryId = 0;
            // Act
            _followCategoryService.Delete("deneme-user-id", zeroSubCategoryId);
        }

        /// <summary>
        /// Eğer followCategoryId O gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Delete_FollowCategoryId_0_Expect_Exception()
        {
            // Arrange
            _unitOfWork
                    .Setup(x => x.Repository<FollowCategory>()
                    .Find(It.IsAny<Expression<Func<FollowCategory, bool>>>()))
                    .Returns(new List<FollowCategory>() { new FollowCategory() }.AsQueryable());

            _repository
                    .Setup(x => x.Find(It.IsAny<Expression<Func<FollowCategory, bool>>>()))
                    .Returns(new List<FollowCategory>() { new FollowCategory() }.AsQueryable());

            CreateManager();
            // Act
            _followCategoryService.Delete("deneme-user-id", 1);
        }

        #endregion Delete method tests

        #region FollowersCount method tests

        /// <summary>
        /// Eğer SubCategoryId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FollowersCount_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroSubCategoryId = 0;
            // Act
            _followCategoryService.FollowersCount(zeroSubCategoryId);
        }

        #endregion FollowersCount method tests

        #region FollowingSubCategoryList method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FollowingSubCategoryList_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _followCategoryService.FollowingSubCategoryList(nullUserId, new PagingModel());
        }

        #endregion FollowingSubCategoryList method tests

        #region FollowingSubCategorySearch method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FollowingSubCategorySearch_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _followCategoryService.FollowingSubCategorySearch(nullUserId, new PagingModel());
        }

        #endregion FollowingSubCategorySearch method tests

        #region IsFollowUpStatus method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsFollowUpStatus_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _followCategoryService.IsFollowUpStatus(nullUserId, 1);
        }

        /// <summary>
        /// Eğer subCategoryId O gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsFollowUpStatus_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroSubCategoryId = 0;
            // Act
            _followCategoryService.IsFollowUpStatus("deneme-user-id", zeroSubCategoryId);
        }

        #endregion IsFollowUpStatus method tests
    }
}