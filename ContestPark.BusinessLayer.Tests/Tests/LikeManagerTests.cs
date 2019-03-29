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
    public class LikeManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ILikeRepository> _repository;
        private Mock<INotificationService> _notificationService;
        private Mock<IPostService> _PostService;
        private ILikeService _likeService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ILikeRepository>();
            _notificationService = new Mock<INotificationService>();
            _PostService = new Mock<IPostService>();
        }

        private void CreateManager()
        {
            _likeService = new LikeService(_repository.Object, _notificationService.Object, _PostService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ILikeRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ILikeRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ILikeService repository = new LikeService(null, _notificationService.Object, _PostService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer INotificationService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_INotificationService_Null_Expect_ArgumentNullException()
        {
            // Act
            ILikeService repository = new LikeService(_repository.Object, null, _PostService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IPostService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IPostService_Null_Expect_ArgumentNullException()
        {
            // Act
            ILikeService repository = new LikeService(_repository.Object, _notificationService.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ILikeService repository = new LikeService(_repository.Object, _notificationService.Object, _PostService.Object, null); ;
        }

        #endregion Constructors methods tests

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
            Like entity = null;
            // Act
            _likeService.Insert(entity);
        }

        /// <summary>
        /// Eğer entity null gelirse NotificationException fırlatması ve mesajının serverMessages_inThisPostYouHaveAlreadyLiked olması lazım
        /// </summary>
        [TestMethod]
        public void Insert_Is_Like_Expect_Exception()
        {
            // Arrange
            _repository.Setup(x => x.IsLike(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            CreateManager();
            // Act
            Like sampleEntity = new Like()
            {
                UserId = "sample-user-id",
                PostId = 1
            };
            // Assert
            NotificationException notificationException = Assert.ThrowsException<NotificationException>(() => _likeService.Insert(sampleEntity));
            Assert.AreEqual("{\"Message\":\"serverMessages_inThisPostYouHaveAlreadyLiked\"}", notificationException.Message);
        }

        /// <summary>
        /// Eğer methodun içindeki PostInfo null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_If_the_PostInfo_is_Null_Expect_Exception()
        {
            // Arrange
            _repository.Setup(x => x.IsLike(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _unitOfWork.Setup(x => x.Repository<Like>().Insert(It.IsAny<Like>()));
            _PostService.Setup(x => x.GetUserId(It.IsAny<int>())).Returns((PostInfoModel)null);
            CreateManager();
            Like sampleEntity = new Like() { UserId = "sample-user-id", PostId = 1 };
            // Act
            _likeService.Insert(sampleEntity);
        }

        #endregion Insert method tests

        #region IsLike method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsLike_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _likeService.IsLike(nullUserId, 1);
        }

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsLike_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullPostId = 0;
            // Act
            _likeService.IsLike("sample-user-id", nullPostId);
        }

        #endregion IsLike method tests

        #region LikeCount method tests

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LikeCount_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullPostId = 0;
            // Act
            _likeService.LikeCount(nullPostId);
        }

        #endregion LikeCount method tests

        #region DisLike method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DisLike_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _likeService.DisLike(nullUserId, 1);
        }

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DisLike_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullPostId = 0;
            // Act
            _likeService.DisLike("sample-user-id", nullPostId);
        }

        #endregion DisLike method tests

        #region Likes method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Likes_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _likeService.Likes(nullUserId, 1, new PagingModel());
        }

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Likes_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullPostId = 0;
            // Act
            _likeService.Likes("sample-user-id", nullPostId, new PagingModel());
        }

        #endregion Likes method tests
    }
}