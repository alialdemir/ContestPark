using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class ChatReplyManagerTests : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IChatReplyRepository> _repository;
        private Mock<IChatService> _chatService;
        private IChatReplyService _chatReplyService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IChatReplyRepository>();
            _chatService = new Mock<IChatService>();
        }

        private void CreateManager()
        {
            _chatReplyService = new ChatReplyManager(_repository.Object, _chatService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IChatReplyRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IChatReplyRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatReplyService repository = new ChatReplyManager(null, _chatService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IChatService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IChatService_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatReplyService repository = new ChatReplyManager(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IChatReplyService repository = new ChatReplyManager(_repository.Object, _chatService.Object, null);
        }

        #endregion Constructors methods tests

        #region UserChatVisibilityCount method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserChatVisibilityCount_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _chatReplyService.UserChatVisibilityCount(nullUserId);
        }

        #endregion UserChatVisibilityCount method tests

        #region ChatSeen method tests

        /// <summary>
        /// Eğer receiverId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChatSeen_ReceiverId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullReceiverId = String.Empty;
            // Act
            _chatReplyService.ChatSeen(nullReceiverId, 1);
        }

        /// <summary>
        /// Eğer chatId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChatSeen_ChatId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullchatId = 0;
            // Act
            _chatReplyService.ChatSeen("deneme", nullchatId);
        }

        #endregion ChatSeen method tests

        #region AllChatSeen method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AllChatSeen_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            await _chatReplyService.ChatSeen(nullUserId);
        }

        #endregion AllChatSeen method tests

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
            _chatReplyService.Delete(nullUserId, "deneme");
        }

        /// <summary>
        /// Eğer receiverUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_ReceiverUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullReceiverUserIdd = String.Empty;
            // Act
            _chatReplyService.Delete("deneme", nullReceiverUserIdd);
        }

        #endregion Delete method tests
    }
}