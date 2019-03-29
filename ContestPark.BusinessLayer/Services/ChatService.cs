using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class ChatService : ServiceBase<Chat>, IChatService
    {
        #region Private Variables

        private IChatRepository _chatRepository;
        private INotificationService _notificationService;
        private IUnitOfWork _unitOfWork;

        #endregion Private Variables

        #region Constructors

        public ChatService(IChatRepository chatRepository,
            INotificationService notificationService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcılar arasındaki mesaj geçmişi
        /// </summary>
        /// <param name="receiverId">Göndere Kullanıcı Id</param>
        /// <param name="senderId">Alıcı Kullanıcı Id</param>
        /// <returns>Mesaj geçmişi listesi</returns>
        public ServiceModel<ChatHistoryModel> ChatHistory(string receiverId, string senderId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatManager.ChatHistory\"");
            Check.IsNullOrEmpty(receiverId, nameof(receiverId));
            Check.IsNullOrEmpty(senderId, nameof(senderId));

            return _chatRepository.ChatHistory(receiverId, senderId, pagingModel);
        }

        /// <summary>
        /// Kullanıcının mesaj listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kullanıcıya ait mesajlar</returns>
        public ServiceModel<ChatListModel> UserChatList(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatManager.UserChatList\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            return _chatRepository.UserChatList(userId, pagingModel);
        }

        /// <summary>
        /// Chat block ekleme
        /// </summary>
        /// <param name="entity">Chat entity</param>
        /// <param name="message">Mesajı</param>
        /// <param name="senderFullName">Gönderen adı</param>
        public void Insert(Chat entity, string message, string senderFullName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatManager.Insert\"");
            Check.IsNull(entity, nameof(entity));
            Check.IsNullOrEmpty(message, nameof(message));
            Check.IsNullOrEmpty(senderFullName, nameof(senderFullName));

            if (entity.SenderId == entity.ReceiverId)
                Check.BadStatus("serverMessages_youCanNotMessageToYourself", $"ChatManager.Insert() entity.SenderId ile entity.ReceiverId eşit geldi. ServerMessage_theJokerIsNotFound fırlatıldı");

            int conversations = _chatRepository.Conversations(entity.ReceiverId, entity.SenderId);
            if (conversations <= 0) base.Insert(entity);
            else entity.ChatId = conversations;

            _unitOfWork
                .Repository<ChatReply>()
                .Insert(new ChatReply
                {
                    ChatId = entity.ChatId,
                    UserId = entity.SenderId,
                    Message = message,
                    VisibilityStatus = true
                });
            SendPushNotificationClients(entity.ReceiverId, senderFullName, message);
        }

        /// <summary>
        /// Mesajın alıcısına push notifcation gönderir
        /// </summary>
        /// <param name="receiverId">Alıcı user id</param>
        /// <param name="senderFullName">Gönderen full name</param>
        /// <param name="message">Gönderilen mesaj1</param>
        private void SendPushNotificationClients(string receiverId, string senderFullName, string message)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatManager.SendPushNotificationClients\"");
            string receiverUserName = _unitOfWork
                                                .Repository<User>()
                                                .Find(p => p.Id == receiverId)
                                                .Select(p => p.UserName)
                                                .FirstOrDefault();

            if (!String.IsNullOrEmpty(receiverUserName))
                _notificationService.PushNotificationClients(senderFullName + ": " + message, receiverUserName);//Alıcıya push notification gönderdik
        }

        /// <summary>
        /// Online olma sırasına göre kullanıcı listesi login olan kullanıcı hariç
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Kullanıcı listesi</returns>
        public ServiceModel<ChatPeopleModel> ChatPeople(string userId, string search, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatManager.ChatPeople\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            if (search == null) search = "";
            return _chatRepository.ChatPeople(userId, search.ToLower(), pagingModel);
        }

        /// <summary>
        /// İki kullanıcı arasında mesaj gönderilmiş mi kontrol eder
        /// </summary>
        /// <param name="receiverId">Gönderen kullanıcı Id</param>
        /// <param name="senderId">Alıcı kullanıcı adı</param>
        /// <returns>İki kullanıcı arasındaki mesaj Id</returns>
        public int Conversations(string receiverId, string senderId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatManager.Conversations\"");
            Check.IsNullOrEmpty(receiverId, nameof(receiverId));
            Check.IsNullOrEmpty(senderId, nameof(senderId));
            return _chatRepository.Conversations(receiverId, senderId);
        }

        #endregion Methods
    }
}