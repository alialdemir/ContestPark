using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.ExceptionHandling;
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
    public class NotificationManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<INotificationRepository> _repository;
        private INotificationService _notificationService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<INotificationRepository>();
        }

        private void CreateManager()
        {
            _notificationService = new NotificationService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer INotificationRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_INotificationRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            INotificationService repository = new NotificationService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            INotificationService repository = new NotificationService(_repository.Object, null);
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
            Notification nullNotification = null;
            // Act
            _notificationService.Insert(nullNotification);
        }

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void Insert_WhoId_Equals_WhonId_Expect_Success()
        {
            // Arrange
            _repository.Setup(mr => mr.Insert(It.IsAny<Notification>()))
                   .Verifiable();
            CreateManager();
            Notification notification = new Notification { WhoId = "sample-user-id", WhonId = "sample-user-id" };
            // Act
            _notificationService.Insert(notification);
        }

        #endregion Insert method tests

        #region Delete method tests

        /// <summary>
        /// Eğer Id 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Delete_Id_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int id = 0;
            // Act
            _notificationService.Delete(id, "sample-user-id");
        }

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
            _notificationService.Delete(1, nullUserId);
        }

        /// <summary>
        /// Eğer gönderdiği bildirim id o kullanıcı id'sine ait değilse 'This id is not yours' mesajı içeren NotificationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"This id is not yours\"}")]
        public void Delete_Not_Equals_NotificationId_And_UserId_Expect_Exception()
        {
            // Arrange
            List<Notification> notificationList = new List<Notification>();
            _unitOfWork
                    .Setup(x => x.Repository<Notification>()
                    .Find(It.IsAny<Expression<Func<Notification, bool>>>()))
                    .Returns(notificationList.AsQueryable());
            CreateManager();
            // Act
            _notificationService.Delete(1, "sample-user-id");
        }

        #endregion Delete method tests

        #region UserNotificationAll method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserNotificationAll_userId_Equals_WhonId_Expect_Success()
        {
            // Arrange
            CreateManager();
            string userId = String.Empty;
            // Act
            _notificationService.UserNotificationAll(userId, new PagingModel());
        }

        #endregion UserNotificationAll method tests

        #region UserNotificationVisibilityCount method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserNotificationVisibilityCount_userId_Equals_WhonId_Expect_Success()
        {
            // Arrange
            CreateManager();
            string userId = String.Empty;
            // Act
            _notificationService.UserNotificationVisibilityCount(userId);
        }

        #endregion UserNotificationVisibilityCount method tests

        #region NotificationSeed method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotificationSeed_userId_Equals_WhonId_Expect_Success()
        {
            // Arrange
            CreateManager();
            string userId = String.Empty;
            // Act
            _notificationService.NotificationSeen(userId);
        }

        #endregion NotificationSeed method tests

        #region PushNotificationInfo method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PushNotificationInfo_userId_Equals_WhonId_Expect_Success()
        {
            // Arrange
            CreateManager();
            string userId = String.Empty;
            // Act
            _notificationService.PushNotificationInfo(NotificationTypes.Comment, userId);
        }

        #endregion PushNotificationInfo method tests

        #region LastLikeNotificationTime method tests

        /// <summary>
        /// Eğer who null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LastLikeNotificationTime_Who_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullwho = String.Empty;
            // Act
            _notificationService.LastLikeNotificationTime(nullwho, "sample-user-id", 1);
        }

        /// <summary>
        /// Eğer whon null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LastLikeNotificationTime_Whon_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullwhon = String.Empty;
            // Act
            _notificationService.LastLikeNotificationTime(nullwhon, "sample-user-id", 1);
        }

        /// <summary>
        /// Eğer PostId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LastLikeNotificationTime_PostId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullPostId = 0;
            // Act
            _notificationService.LastLikeNotificationTime("sample-user-id", "sample-user-id", nullPostId);
        }

        #endregion LastLikeNotificationTime method tests
    }
}