using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class ScoreManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IScoreRepository> _repository;
        private IScoreService _scoreService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IScoreRepository>();
        }

        private void CreateManager()
        {
            _scoreService = new ScoreService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IScoreRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IScoreRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IScoreService repository = new ScoreService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IScoreService repository = new ScoreService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region DuelResultRanking method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuelResultRanking_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _scoreService.DuelResultRanking(nullUserId, 1);
        }

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
            _scoreService.DuelResultRanking("sample-user-id", nullsubCategoryId);
        }

        #endregion DuelResultRanking method tests

        #region FacebookFriendRanking method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FacebookFriendRanking_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _scoreService.FacebookFriendRanking(nullUserId, 1, null);
        }

        /// <summary>
        /// Eğer subCategoryId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FacebookFriendRanking_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _scoreService.FacebookFriendRanking("sample-user-id", nullsubCategoryId, null);
        }

        /// <summary>
        /// Eğer FacebookFriendRanking null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FacebookFriendRanking_FacebookFriendRanking_null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            List<FacebookFriendRankingModel> nullFacebookFriendRanking = (List<FacebookFriendRankingModel>)null;
            // Act
            _scoreService.FacebookFriendRanking("sample-user-id", 1, nullFacebookFriendRanking);
        }

        #endregion FacebookFriendRanking method tests

        #region ScoreRanking method tests

        /// <summary>
        /// Eğer subCategoryId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScoreRanking_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _scoreService.ScoreRanking(nullsubCategoryId, new PagingModel { });
        }

        #endregion ScoreRanking method tests

        #region ScoreRankingFollowing method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreRankingFollowing_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _scoreService.ScoreRankingFollowing(nullUserId, 1, new PagingModel());
        }

        /// <summary>
        /// Eğer subCategoryId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScoreRankingFollowing_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _scoreService.ScoreRankingFollowing("sample-user-id", nullsubCategoryId, new PagingModel());
        }

        #endregion ScoreRankingFollowing method tests

        #region UserTotalScore method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserTotalScore_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _scoreService.UserTotalScore(nullUserId, 1);
        }

        /// <summary>
        /// Eğer subCategoryId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UserTotalScore_SubCategoryId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullsubCategoryId = 0;
            // Act
            _scoreService.UserTotalScore("sample-user-id", nullsubCategoryId);
        }

        #endregion UserTotalScore method tests
    }
}