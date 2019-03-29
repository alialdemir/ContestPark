using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.ExceptionHandling;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class ChatManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IChatRepository> _repository;
        private Mock<INotificationService> _notificationService;
        private Mock<DataAccessLayer.Interfaces.IRepository<Chat>> _entityRepository;
        private Mock<IDbFactory> _dDbFactory;
        private PagingModel _pagingModel;
        private IChatService _chatService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IChatRepository>();
            _notificationService = new Mock<INotificationService>();
            _entityRepository = new Mock<DataAccessLayer.Interfaces.IRepository<Chat>>();
            _dDbFactory = new Mock<IDbFactory>();
            _pagingModel = new PagingModel();
        }

        private void CreateManager()
        {
            _chatService = new ChatService(_repository.Object, _notificationService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IChatRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IChatBlockRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatService repository = new ChatService(null, _notificationService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer INotificationService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_INotificationService_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatService repository = new ChatService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatService repository = new ChatService(_repository.Object, _notificationService.Object, null);
        }

        #endregion Constructors methods tests

        #region ChatHistory method tests

        /// <summary>
        /// Eğer ReceiverId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChatHistory_ReceiverId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullReceiverId = String.Empty;
            // Act
            _chatService.ChatHistory(nullReceiverId, "deneme", new PagingModel());
        }

        /// <summary>
        /// Eğer SenderId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChatHistory_SenderId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullSenderId = String.Empty;
            // Act
            _chatService.ChatHistory("deneme", nullSenderId, new PagingModel());
        }

        /// <summary>
        /// Data katmanından ChatHistory items null gelirse iş katmanıda null döndürmeli
        /// </summary>
        [TestMethod]
        public void ChatHistory_When_Data_Layer_Return_Null_Expect_Return_Null()
        {
            // Arrange
            _repository.Setup(p => p.ChatHistory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagingModel>())).Returns(() => { return new ServiceModel<ChatHistoryModel>(); });
            CreateManager();
            // Act
            var chatHistories = _chatService.ChatHistory("deneme-receiverId", "deneme-senderId", new PagingModel());

            Assert.IsNull(chatHistories.Items);
        }

        /// <summary>
        /// Eğer PicturePath data katmanından gelen listedeki SenderId eşit değilse
        /// parametreden gelen receiverId'ye ve aralarında engelleme durumu VARSA !!!
        /// default profil picture resim yolu eklenmeli
        /// </summary>
        [TestMethod]
        public void ChatHistory_When_PicturePath_Null_Expect_Set_Default_PicturePath()
        {
            // Arrange
            _repository.Setup(p => p.ChatHistory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagingModel>())).Returns(() =>
            {
                return new ServiceModel<ChatHistoryModel>
                {
                    Items = new List<ChatHistoryModel>
                   {
                         new ChatHistoryModel{ PicturePath=String.Empty }
                   }
                };
            });
            CreateManager();
            // Act
            var chatHistories = _chatService.ChatHistory("deneme-receiverId", "deneme-senderId", new PagingModel());

            Assert.AreEqual(chatHistories.Items.First().PicturePath, DefaultImages.DefaultProfilePicture);
        }

        /// <summary>
        /// Eğer PicturePath data katmanından gelen listedeki SenderId eşit değilse
        /// parametreden gelen receiverId'ye ve aralarında engelleme durumu Yoksa
        /// Kullanıcıya ait profil reesmi dönmeli
        /// </summary>
        [TestMethod]
        public void ChatHistory_When_PicturePath_Not_Null_Expect_User_PicturePath()
        {
            // Arrange
            _repository.Setup(p => p.ChatHistory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagingModel>())).Returns(() =>
            {
                return new ServiceModel<ChatHistoryModel>
                {
                    Items = new List<ChatHistoryModel>
                   {
                         new ChatHistoryModel{ PicturePath="ProfilePicturePath" }
                   }
                };
            });
            CreateManager();
            // Act
            var chatHistories = _chatService.ChatHistory("deneme-receiverId", "deneme-senderId", new PagingModel());

            Assert.AreEqual(chatHistories.Items.First().PicturePath, "ProfilePicturePath");
        }

        #endregion ChatHistory method tests

        #region UserChatList method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserChatList_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _chatService.UserChatList(nullUserId, _pagingModel);
        }

        /*
         *
         *
         * Engelleme durumu data katmanında sorgu içinde yapılmıştır bu yüzden bu test iptal edildi
         *
         *
         *
         */
        /// <summary>
        /// Eğer PicturePath data katmanından gelen kullanıcı ile
        /// parametreden gelen userId aralarında engelleme durumu VARSA !!!
        /// default profil picture resim yolu eklenmeli
        /// </summary>
        //[TestMethod]
        //public void UserChatList_When_PicturePath_Null_Expect_Set_Default_PicturePath()
        //{
        //    // Arrange
        //    _chatBlockService.Setup(p => p.BlockingStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        //    _repository.Setup(p => p.UserChatList(It.IsAny<string>(), It.IsAny<Languages>(), It.IsAny<PagingModel>())).Returns(() =>
        //     {
        //         return new ServiceModel<ChatListModel>()
        //         {
        //             Items = new List<ChatListModel>
        //          {
        //             new ChatListModel { UserProfilePicturePath="ProfilePicturePath" }
        //          }
        //         };
        //     });
        //    CreateManager();
        //    // Act
        //    var chatHistories = _chatService.UserChatList("deneme-receiverId", Languages.English, _pagingModel);

        //    Assert.AreEqual(chatHistories.Items.First().UserProfilePicturePath, DefaultImages.DefaultProfilePicture);
        //}
        /// <summary>
        /// Eğer PicturePath data katmanından gelen kullanıcı ile
        /// parametreden gelen userId aralarında engelleme durumu YOKSA !!!
        /// default profil picture resim yolu eklenmeli
        /// </summary>
        [TestMethod]
        public void UserChatList_When_PicturePath_Not_Null_Expect_User_PicturePath()
        {
            // Arrange
            _repository.Setup(p => p.UserChatList(It.IsAny<string>(), It.IsAny<PagingModel>())).Returns(() =>
            {
                return new ServiceModel<ChatListModel>()
                {
                    Items = new List<ChatListModel>
                  {
                     new ChatListModel { UserProfilePicturePath="ProfilePicturePath" }
                  }
                };
            });
            CreateManager();
            // Act
            var chatHistories = _chatService.UserChatList("deneme-receiverId", _pagingModel);

            Assert.AreEqual(chatHistories.Items.First().UserProfilePicturePath, "ProfilePicturePath");
        }

        /// <summary>
        /// Data katmanından UserChatList null gelirse iş katmanıda null döndürmeli
        /// </summary>
        [TestMethod]
        public void UserChatList_When_Data_Layer_Return_Null_Expect_Return_Null()
        {
            // Arrange
            _repository.Setup(p => p.UserChatList(It.IsAny<string>(), It.IsAny<PagingModel>())).Returns(() => { return null; });
            CreateManager();
            // Act
            var userChatList = _chatService.UserChatList("deneme-receiverId", _pagingModel);

            Assert.IsNull(userChatList);
        }

        #endregion UserChatList method tests

        #region UserChatList method tests

        /// <summary>
        /// Eğer entity null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_Entity_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            Chat nullChat = null;
            // Act
            _chatService.Insert(nullChat, "deneme", "deneme");
        }

        /// <summary>
        /// Eğer message null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_message_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullmessage = String.Empty;
            // Act
            _chatService.Insert(new Chat { }, nullmessage, "deneme");
        }

        /// <summary>
        /// Eğer senderFullName gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_senderFullName_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullSenderFullName = String.Empty;
            // Act
            _chatService.Insert(new Chat { }, "deneme", nullSenderFullName);
        }

        /// <summary>
        /// Eğer entity.SenderId ve entity.ReceiverId aynı gelirse serverMessages_youCanNotMessageToYourself fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"serverMessages_youCanNotMessageToYourself\"}")]
        public void Insert_SenderId_Equals_ReceiverId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            var equalsReceiverIdAndSenderFullName = new Chat { SenderId = "deneme", ReceiverId = "deneme" };
            // Act
            _chatService.Insert(equalsReceiverIdAndSenderFullName, "deneme-message", "deneme");
        }

        #endregion UserChatList method tests

        #region ChatPeople method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChatPeople_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _chatService.ChatPeople(nullUserId, "", _pagingModel);
        }

        #endregion ChatPeople method tests

        #region Conversations method tests

        /// <summary>
        /// Eğer receiverId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Conversations_ReceiverId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullReceiverId = String.Empty;
            // Act
            _chatService.Conversations(nullReceiverId, "deneme");
        }

        /// <summary>
        /// Eğer senderId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Conversations_SenderId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullSenderId = String.Empty;
            // Act
            _chatService.Conversations("deneme", nullSenderId);
        }

        #endregion Conversations method tests
    }
}