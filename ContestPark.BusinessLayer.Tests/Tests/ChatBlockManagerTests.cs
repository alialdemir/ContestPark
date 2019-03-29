using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.ExceptionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class ChatBlockManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IUserService> _userService;
        private Mock<IChatBlockRepository> _repository;
        private Mock<DataAccessLayer.Interfaces.IRepository<ChatBlock>> _entityRepository;
        private Mock<IDbFactory> _dDbFactory;
        private IChatBlockService _chatBlockService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userService = new Mock<IUserService>();
            _repository = new Mock<IChatBlockRepository>();
            _entityRepository = new Mock<DataAccessLayer.Interfaces.IRepository<ChatBlock>>();
            _dDbFactory = new Mock<IDbFactory>();
        }

        private void CreateManager()
        {
            _chatBlockService = new ChatBlockService(_repository.Object, _userService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IChatBlockRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IChatBlockRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatBlockService _boost = new ChatBlockService(null, _userService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUserService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUserService_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatBlockService _boost = new ChatBlockService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatBlockService _boost = new ChatBlockService(_repository.Object, _userService.Object, null);
        }

        #endregion Constructors methods tests

        #region ChatBlockRemove method tests

        /// <summary>
        /// ChatBlockId değeri sıfır giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChatBlockRemove_Wrong_ChatBlockId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            int chatBlockId = 0;
            // Act
            _chatBlockService.ChatBlockRemove(chatBlockId, "deneme-123", "deneme-user-id");
        }

        /// <summary>
        /// WhonId değeri null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChatBlockRemove_Wrong_WhonId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string whonId = String.Empty;
            // Act
            _chatBlockService.ChatBlockRemove(1, whonId, "deneme-user-id");
        }

        /// <summary>
        /// UserId değeri null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChatBlockRemove_Wrong_UserId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string userId = String.Empty;

            // Act
            _chatBlockService.ChatBlockRemove(1, "deneme-user-id", userId);
        }

        /// <summary>
        /// WhonId ve UserId aynı giderse giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"serverMessages_youCanNotRemoveTheBlockYourself\"}")]
        public void ChatBlockRemove_Equals_WhonId_And_UserId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string userIdAndWhonId = "deneme";

            // Act
            _chatBlockService.ChatBlockRemove(1, userIdAndWhonId, userIdAndWhonId);
        }

        /// <summary>
        /// Kullanıcılar arası engelleme varsa exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"serverMessages_userLiftedTheBlock\"}")]
        public void ChatBlockRemove_ChatBlockStatus_Control_Expect_Exception()
        {
            //Arrange
            _userService.Setup(x => x.IsUserIdControl(It.IsAny<string>())).Returns(true);
            _repository.Setup(p => p.Delete(It.IsAny<int>()));
            _repository.Setup(p => p.BlockingStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            CreateManager();
            // Act
            _chatBlockService.ChatBlockRemove(1, "deneme-deneme-whonId", "deneme-deneme-userId");
        }

        #endregion ChatBlockRemove method tests

        #region UserBlocking method tests

        /// <summary>
        /// User id null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserBlocking_UserId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullUserId = String.Empty;

            // Act
            _chatBlockService.UserBlocking(nullUserId, "deneme-deneme");
        }

        /// <summary>
        /// WhonId id null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserBlocking_WhonId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhonId = String.Empty;

            // Act
            _chatBlockService.UserBlocking("deneme-deneme", nullWhonId);
        }

        /// <summary>
        /// WhonId ve UserId aynı giderse giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"serverMessages_doNotBlockToYourself\"}")]
        public void UserBlocking_Equals_WhonId_And_UserId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string userIdAndWhonId = "deneme";

            // Act
            _chatBlockService.UserBlocking(userIdAndWhonId, userIdAndWhonId);
        }

        /// <summary>
        /// Kullanıcılar arası engelleme yoksa zaten engellediniz diye exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"serverMessages_thisUserIsAlreadyBlocked\"}")]
        public void UserBlocking_ChatBlockStatus_Control_Expect_Exception()
        {
            //Arrange
            _userService.Setup(x => x.IsUserIdControl(It.IsAny<string>())).Returns(true);
            _repository.Setup(p => p.UserBlockingStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            CreateManager();
            // Act
            _chatBlockService.UserBlocking("deneme-deneme-whonId", "deneme-deneme-userId");
        }

        /// <summary>
        /// Kullanıcılar arası engelleme yoksa zaten engellediniz diye exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(InvalidOperationException), "whoId Id not registry.")]
        public void UserBlocking_IsUserIdControl_Control_Expect_Exception()
        {
            //Arrange
            _userService.Setup(x => x.IsUserIdControl(It.IsAny<string>())).Returns(false);
            CreateManager();
            // Act
            _chatBlockService.UserBlocking("deneme-deneme-whonId", "deneme-deneme-userId");
        }

        #endregion UserBlocking method tests

        #region Delete method tests

        /// <summary>
        /// ChatBlock entity null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Entity_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            // Act
            _chatBlockService.Update(null);
        }

        /// <summary>
        /// Methoda kullanılan GetById null döndürürse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_GetById_Return_Null_Expect_Exception()
        {
            //Arrange
            _entityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<ChatBlock, bool>>>())).Returns(new List<ChatBlock>().AsQueryable());
            _unitOfWork.Setup(p => p.Repository<ChatBlock>()).Returns(() =>
            {
                return _entityRepository.Object;
            });
            CreateManager();
            ChatBlock chatBlock = new ChatBlock();
            // Act
            _chatBlockService.Update(chatBlock);
        }

        #endregion Delete method tests

        #region BlockingStatus method tests

        /// <summary>
        /// WhoId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BlockingStatus_WhoId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhoId = String.Empty;

            // Act
            _chatBlockService.BlockingStatus(nullWhoId, "deneme-deneme");
        }

        /// <summary>
        /// WhonId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BlockingStatus_WhonId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhonId = String.Empty;

            // Act
            _chatBlockService.BlockingStatus("deneme-deneme", nullWhonId);
        }

        /// <summary>
        /// WhonId ve WhoId aynı giderse giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(InvalidOperationException), "The two id's should not be equal.")]
        public void BlockingStatus_Equals_WhonId_And_WhoId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string whoIdAndWhonId = "deneme";

            // Act
            _chatBlockService.BlockingStatus(whoIdAndWhonId, whoIdAndWhonId);
        }

        #endregion BlockingStatus method tests

        #region UserBlockingStatus method tests

        /// <summary>
        /// WhoId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserBlockingStatus_WhoId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhoId = String.Empty;

            // Act
            _chatBlockService.UserBlockingStatus(nullWhoId, "deneme-deneme");
        }

        /// <summary>
        /// WhonId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserBlockingStatus_WhonId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhonId = String.Empty;

            // Act
            _chatBlockService.UserBlockingStatus("deneme-deneme", nullWhonId);
        }

        /// <summary>
        /// WhonId ve WhoId aynı giderse giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(InvalidOperationException), "The two id's should not be equal.")]
        public void UserBlockingStatus_Equals_WhonId_And_WhoId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string whoIdAndWhonId = "deneme";

            // Act
            _chatBlockService.UserBlockingStatus(whoIdAndWhonId, whoIdAndWhonId);
        }

        #endregion UserBlockingStatus method tests

        #region Delete method tests

        /// <summary>
        /// WhoId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_WhoId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhoId = String.Empty;

            // Act
            _chatBlockService.Delete(nullWhoId, "deneme-deneme");
        }

        /// <summary>
        /// WhonId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_WhonId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhonId = String.Empty;

            // Act
            _chatBlockService.Delete("deneme-deneme", nullWhonId);
        }

        /// <summary>
        /// WhonId ve WhoId aynı giderse giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(InvalidOperationException), "The two id's should not be equal.")]
        public void Delete_Equals_WhonId_And_WhoId_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string whoIdAndWhonId = "deneme";

            // Act
            _chatBlockService.Delete(whoIdAndWhonId, whoIdAndWhonId);
        }

        #endregion Delete method tests

        #region UserBlockList methods tests

        /// <summary>
        /// UserId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserBlockList_UserId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullUserId = String.Empty;

            // Act
            _chatBlockService.UserBlockList(nullUserId, new Entities.Models.PagingModel());
        }

        #endregion UserBlockList methods tests

        #region GetChatBlockIdByWhonIdAndWhoId methods tests

        /// <summary>
        /// whoId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChatBlockIdByWhonIdAndWhoId_WhoId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhoId = String.Empty;

            // Act
            _chatBlockService.GetChatBlockIdByWhonIdAndWhoId(nullWhoId, "sample-whon-Id");
        }

        /// <summary>
        /// whonId null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChatBlockIdByWhonIdAndWhoId_WhonId_Null_Expect_Exception()
        {
            //Arrange
            CreateManager();
            string nullWhonId = String.Empty;

            // Act
            _chatBlockService.GetChatBlockIdByWhonIdAndWhoId("sample-who-Id", nullWhonId);
        }

        #endregion GetChatBlockIdByWhonIdAndWhoId methods tests
    }
}