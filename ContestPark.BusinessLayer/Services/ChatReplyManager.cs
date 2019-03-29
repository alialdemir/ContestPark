using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Services
{
    public class ChatReplyManager : ServiceBase<ChatReply>, IChatReplyService
    {
        #region Private Variables

        private IChatReplyRepository _chatReplyRepository;
        private IChatService _chatService;

        #endregion Private Variables

        #region Constructors

        public ChatReplyManager(IChatReplyRepository chatReplyRepository, IChatService chatService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _chatReplyRepository = chatReplyRepository ?? throw new ArgumentNullException(nameof(chatReplyRepository));
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının okunmamış mesaj sayısı
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Okunmamış mesaj sayısı</returns>
        public int UserChatVisibilityCount(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatReplyManager.UserChatVisibilityCount\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            return _chatReplyRepository.UserChatVisibilityCount(userId);
        }

        /// <summary>
        /// Mesajı alan mesajı görmüş ise ilgili mesajı görme durumu false yapar
        /// </summary>
        /// <param name="receiverId">Mesajı alan kullanıcı Id</param>
        /// <param name="chatId">sohbet Id</param>
        /// <returns>İşlem durumu</returns>
        public void ChatSeen(string receiverId, int chatId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatReplyManager.ChatSeen\"");
            Check.IsNullOrEmpty(receiverId, nameof(receiverId));
            Check.IsLessThanZero(chatId, nameof(chatId));

            _chatReplyRepository.ChatSeen(receiverId, chatId);
        }

        /// <summary>
        /// Kullanıcının görmediği tüm mesajlarını görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>İşlem durumu</returns>
        public async Task<bool> ChatSeen(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatReplyManager.AllChatSeen\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return await _chatReplyRepository.ChatSeen(userId);
        }

        /// <summary>
        /// Kullanıcı mesajını görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="chatId">Sohbet Id</param>
        /// <returns>İşlem durumu</returns>
        public bool Delete(string userId, string receiverUserId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatReplyManager.Delete\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(receiverUserId, nameof(receiverUserId));

            int charId = _chatService.Conversations(userId, receiverUserId);
            return _chatReplyRepository.Delete(userId, charId);
        }

        #endregion Methods
    }
}