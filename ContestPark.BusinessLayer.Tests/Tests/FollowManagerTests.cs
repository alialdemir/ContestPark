using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.ExceptionHandling;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class FollowManagerTests
    {
        #region Field

        private Mock<IFollowRepository> _repository;
        private Mock<INotificationService> _notificationService;
        private Mock<IPostService> _PostService;
        private Mock<IUnitOfWork> _unitOfWork;
        private PagingModel _pagingModel;
        private IFollowService _followService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _repository = new Mock<IFollowRepository>();
            _notificationService = new Mock<INotificationService>();
            _PostService = new Mock<IPostService>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _pagingModel = new PagingModel();
        }

        private void CreateManager()
        {
            _followService = new FollowService(_repository.Object, _notificationService.Object, _PostService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IFollowRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IFollowRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IFollowService _boost = new FollowService(null, _notificationService.Object, _PostService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer INotificationService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_INotificationService_Null_Expect_ArgumentNullException()
        {
            // Act
            IFollowService _boost = new FollowService(_repository.Object, null, _PostService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IPostService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IPostService_Null_Expect_ArgumentNullException()
        {
            // Act
            IFollowService _boost = new FollowService(_repository.Object, _notificationService.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IFollowService _boost = new FollowService(_repository.Object, _notificationService.Object, _PostService.Object, null);
        }

        #endregion Constructors methods tests

        #region IsFollowUpStatus method tests

        /// <summary>
        /// Eğer FollowUpUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsFollowUpStatus_FollowUpUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowUpUserId = String.Empty;
            // Act
            _followService.IsFollowUpStatus(nullFollowUpUserId, "deneme-user-id");
        }

        /// <summary>
        /// Eğer followedUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsFollowUpStatus_FollowedUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowedUserId = String.Empty;
            // Act
            _followService.IsFollowUpStatus("deneme-user-id", nullFollowedUserId);
        }

        /// <summary>
        /// Eğer FollowedUserId ve FollowUpUserId aynı gelirse false dönmeli lazım
        /// </summary>
        [TestMethod]
        public void IsFollowUpStatus_Equals_FollowedUserId_And_FollowUpUserId_Expect_False()
        {
            // Arrange
            CreateManager();
            string nullUserId = "deneme-user-id";
            // Act
            bool result = _followService.IsFollowUpStatus(nullUserId, nullUserId);
            // Assert
            Assert.IsFalse(result);
        }

        #endregion IsFollowUpStatus method tests

        #region Insert method tests

        /// <summary>
        /// Eğer entity null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_Entity_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            Follow nullEntity = null;
            // Act
            _followService.Insert(nullEntity);
        }

        /// <summary>
        /// Eğer entity.FollowedUserId ve entity.FollowUpUserId aynı gelirse serverMessages_youCannotFollowASelf fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"ServerMessages_youCannotFollowASelf\"}")]
        public void Insert_SenderId_Equals_ReceiverId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string sampleUserId = "sample-user-id";
            Follow entity = new Follow
            {
                FollowedUserId = sampleUserId,
                FollowUpUserId = sampleUserId
            };
            // Act
            _followService.Insert(entity);
        }

        /// <summary>
        /// Eğer daha önceden takip etmiş ise serverMessages_thisUserAreAlreadyFollowing fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"ServerMessages_thisUserAreAlreadyFollowing\"}")]
        public void Insert_If_Are_already_Following_Expect_Exception()
        {
            // Arrange
            _repository.Setup(x => x.IsFollowUpStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            CreateManager();
            Follow entity = new Follow
            {
                FollowedUserId = "sample-FollowedUserId",
                FollowUpUserId = "sample-FollowUpUserId"
            };
            // Act
            _followService.Insert(entity);
        }

        #endregion Insert method tests

        #region Followers method tests

        /// <summary>
        /// Eğer followedUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Followers_FollowedUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowedUserId = String.Empty;
            // Act
            _followService.Followers(nullFollowedUserId, "deneme-user-id", _pagingModel);
        }

        /// <summary>
        /// Eğer followUpUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Followers_FollowUpUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowUpUserId = String.Empty;
            // Act
            _followService.Followers(nullFollowUpUserId, "deneme-user-id", _pagingModel);
        }

        #endregion Followers method tests

        #region Following method tests

        /// <summary>
        /// Eğer followedUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Following_FollowedUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowedUserId = String.Empty;
            // Act
            _followService.Following(nullFollowedUserId, "deneme-user-id", _pagingModel);
        }

        /// <summary>
        /// Eğer followUpUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Following_FollowUpUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowUpUserId = String.Empty;
            // Act
            _followService.Following(nullFollowUpUserId, "deneme-user-id", _pagingModel);
        }

        #endregion Following method tests

        #region Delete method tests

        /// <summary>
        /// Eğer followedUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_FollowedUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowedUserId = String.Empty;
            // Act
            _followService.Delete(nullFollowedUserId, "deneme-user-id");
        }

        /// <summary>
        /// Eğer followUpUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_FollowUpUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowUpUserId = String.Empty;
            // Act
            _followService.Delete(nullFollowUpUserId, "deneme-user-id");
        }

        #endregion Delete method tests

        #region FollowersCount method tests

        /// <summary>
        /// Eğer followedUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FollowersCount_FollowedUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowedUserId = String.Empty;
            // Act
            _followService.FollowersCount(nullFollowedUserId);
        }

        #endregion FollowersCount method tests

        #region FollowUpCount method tests

        /// <summary>
        /// Eğer followUpUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FollowUpCount_FollowUpUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowUpUserId = String.Empty;
            // Act
            _followService.FollowUpCount(nullFollowUpUserId);
        }

        #endregion FollowUpCount method tests

        #region FollowingChatList method tests

        /// <summary>
        /// Eğer followedUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FollowingChatList_FollowedUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFollowedUserId = String.Empty;
            // Act
            _followService.FollowingChatList(nullFollowedUserId, "", _pagingModel);
        }

        #endregion FollowingChatList method tests
    }
}