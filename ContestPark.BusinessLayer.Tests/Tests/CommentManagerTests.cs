using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class CommentManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICommentRepository> _repository;
        private Mock<INotificationService> _notificationService;
        private Mock<IPostService> _PostService;
        private ICommentService _commentService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ICommentRepository>();
            _notificationService = new Mock<INotificationService>();
            _PostService = new Mock<IPostService>();
        }

        private void CreateManager()
        {
            _commentService = new CommentService(_repository.Object, _notificationService.Object, _PostService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICommentRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICommentRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ICommentService repository = new CommentService(null, _notificationService.Object, _PostService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer INotificationService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_INotificationService_Null_Expect_ArgumentNullException()
        {
            // Act
            ICommentService repository = new CommentService(_repository.Object, null, _PostService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IPostService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IPostService_Null_Expect_ArgumentNullException()
        {
            // Act
            ICommentService repository = new CommentService(_repository.Object, _notificationService.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ICommentService repository = new CommentService(_repository.Object, _notificationService.Object, _PostService.Object, null);
        }

        #endregion Constructors methods tests

        #region CommentCount method tests

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CommentCount_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroPostId = 0;
            // Act
            _commentService.CommentCount(zeroPostId);
        }

        #endregion CommentCount method tests

        #region CommentList method tests

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CommentList_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nulPostId = 0;
            // Act
            _commentService.CommentList(nulPostId, new Entities.Models.PagingModel());
        }

        #endregion CommentList method tests

        #region Insert method tests

        /// <summary>
        /// Eğer entity null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_entity_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            Comment nulEntity = null;
            // Act
            _commentService.Insert(nulEntity);
        }

        /// <summary>
        /// Eğer en son bilgidirim gönderme tarihi 28 dakkikadan az ise method başarılı bir şekilde sonuçlanmalı
        /// </summary>
        [TestMethod]
        public void Given_Minutes_When_28_Then_Success()
        {
            // Arrange
            _unitOfWork.Setup(x => x.Repository<Comment>().Insert(It.IsAny<Comment>()));
            _repository.Setup(x => x.LastCommentTime(It.IsAny<string>(), It.IsAny<int>())).Returns(DateTime.Now.AddMinutes(10));
            CreateManager();
            // Act
            _commentService.Insert(new Comment { });
        }

        #endregion Insert method tests

        #region LastCommentTime method tests

        /// <summary>
        /// Eğer userId null gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LastCommentTime_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _commentService.LastCommentTime(nullUserId, 12);
        }

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LastCommentTime_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroPostId = 0;
            // Act
            _commentService.LastCommentTime("Deneme-user-id", zeroPostId);
        }

        #endregion LastCommentTime method tests
    }
}