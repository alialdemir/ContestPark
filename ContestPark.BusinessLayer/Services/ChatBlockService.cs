using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class ChatBlockService : ServiceBase<ChatBlock>, IChatBlockService
    {
        #region Private Variables

        private IChatBlockRepository _chatBlockRepository;
        private IUserService _userService;

        #endregion Private Variables

        #region Constructors

        public ChatBlockService(IChatBlockRepository chatBlockRepository,
            IUserService userService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _chatBlockRepository = chatBlockRepository ?? throw new ArgumentNullException(nameof(chatBlockRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Engellenmiş kullanıcının engelini kaldırır
        /// </summary>
        /// <param name="chatBlockId">Sohet engel Id</param>
        /// <param name="whonId">Engelleyen kullanıcı Id</param>
        /// <param name="userId">Engellenen kullanıcı Id</param>
        public void ChatBlockRemove(int chatBlockId, string whonId, string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.ChatBlockRemove\"");
            Check.IsLessThanZero(chatBlockId, nameof(whonId));
            Check.IsNullOrEmpty(whonId, nameof(whonId));
            Check.IsNullOrEmpty(userId, nameof(userId));
            if (whonId == userId)
                Check.BadStatus(
                    "serverMessages_youCanNotRemoveTheBlockYourself",
                    $"ChatBlockRemove({chatBlockId}, '{whonId}', '{userId}') whonId ve userId birbirine eşit geldi. serverMessages_youCanNotRemoveTheBlockYourself fırlatıldı");

            _chatBlockRepository.Delete(chatBlockId);

            // TODO: burada kendisi engellediyse hata oluşabilir test etmek lazım
            bool chatBlockStatus = BlockingStatus(whonId, userId);//Bu seferde karşı taraf bunu engellemiş mi kontrol ettik
            if (!chatBlockStatus)
                Check.BadStatus(
                    "serverMessages_userLiftedTheBlock",
                   $"ChatBlockRemove({chatBlockId}, '{whonId}', '{userId}') iki kullanıcı arasında engelleme var. serverMessages_userLiftedTheBlock fırlatıldı");
        }

        /// <summary>
        /// Kullanıcıyı engelle
        /// </summary>
        /// <param name="userId"></</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public void UserBlocking(string userId, string whonId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.UserBlocking\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(whonId, nameof(whonId));
            if (whonId == userId)
                Check.BadStatus("serverMessages_doNotBlockToYourself", $"UserBlocking('{userId}', '{whonId}') whonId ve userId birbirine eşit geldi. serverMessages_doNotBlockToYourself fırlatıldı");
            if (UserBlockingStatus(userId, whonId))
                Check.BadStatus("serverMessages_thisUserIsAlreadyBlocked", $"UserBlocking('{userId}', '{whonId}') iki kullanıcı arasında engelleme var. serverMessages_thisUserIsAlreadyBlocked fırlatıldı");
            _chatBlockRepository.UserBlocking(userId, whonId);
        }

        /// <summary>
        /// ChatBlock güncelleme
        /// </summary>
        /// <param name="entity">ChatBock entity</param>
        public new void Update(ChatBlock entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.Update\"");
            Check.IsNull(entity, nameof(entity));

            ChatBlock chatBlock = Find(x => x.ChatBlockId == entity.ChatBlockId).FirstOrDefault();
            Check.IsNull(chatBlock, nameof(chatBlock));

            chatBlock.WhoId = entity.WhoId;
            chatBlock.WhonId = entity.WhonId;
            base.Update(chatBlock);
        }

        /// <summary>
        /// Sohbet karşılıklı engelleme durumu
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        /// <returns>İki tarafdan biri engellemiş mi true ise engellemiş false ise engellememiş</returns>
        public bool BlockingStatus(string whoId, string whonId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.BlockingStatus\"");
            Check.IsNullOrEmpty(whoId, nameof(whoId));
            Check.IsNullOrEmpty(whonId, nameof(whonId));
            if (whoId == whonId)
            {
                //     LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') The two id's should not be equal");
                throw new InvalidOperationException("The two id's should not be equal.");
            }
            if (!_userService.IsUserIdControl(whoId))
            {
                //        LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') whoId Id not registry");
                throw new InvalidOperationException("whoId Id not registry.");
            }
            if (!_userService.IsUserIdControl(whonId))
            {
                //       LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') whon Id not registry");
                throw new InvalidOperationException("whon Id not registry.");
            }

            return _chatBlockRepository.BlockingStatus(whoId, whonId);
        }

        /// <summary>
        /// Tek taraflı engelleme kontrol eder
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        /// <returns>Engellemiş ise true engellemiş false ise engellememiş</returns>
        public bool UserBlockingStatus(string whoId, string whonId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.UserBlockingStatus\"");
            Check.IsNullOrEmpty(whoId, nameof(whoId));
            Check.IsNullOrEmpty(whonId, nameof(whonId));
            if (whoId == whonId)
            {
                //     LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') The two id's should not be equal");
                throw new InvalidOperationException("The two id's should not be equal.");
            }
            if (!_userService.IsUserIdControl(whoId))
            {
                //        LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') whoId Id not registry");
                throw new InvalidOperationException("whoId Id not registry.");
            }
            if (!_userService.IsUserIdControl(whonId))
            {
                //       LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') whon Id not registry");
                throw new InvalidOperationException("whon Id not registry.");
            }

            return _chatBlockRepository.UserBlockingStatus(whoId, whonId);
        }

        /// <summary>
        /// Engelleme kaldır
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public void Delete(string whoId, string whonId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.Delete\"");
            Check.IsNullOrEmpty(whoId, nameof(whoId));
            Check.IsNullOrEmpty(whonId, nameof(whonId));
            if (whoId == whonId)
            {
                //   LoggingService.LogError($"BlockingStatus('{whoId}', '{whonId}') The two id's should not be equal");
                throw new InvalidOperationException("The two id's should not be equal.");
            }
            if (!BlockingStatus(whoId, whonId))
                Check.BadStatus("serverMessages.youUserBlock");//0 dan küçük eşit ise kayıt yoktur yani engellememiştir..

            int chatBlockId = GetChatBlockIdByWhonIdAndWhoId(whoId, whonId);
            Delete(chatBlockId);
        }

        /// <summary>
        /// İki kullanıcı arasındaki engellemenin ChatBlockId'sini verir
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public int GetChatBlockIdByWhonIdAndWhoId(string whoId, string whonId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.GetChatBlockIdByWhonIdAndWhoId\"");
            Check.IsNullOrEmpty(whoId, nameof(whoId));
            Check.IsNullOrEmpty(whonId, nameof(whonId));
            return _chatBlockRepository.GetChatBlockIdByWhonIdAndWhoId(whoId, whonId);
        }

        /// <summary>
        /// Kullanıcının engellediği kullanıcılar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Engellenenler listesi</returns>
        public ServiceModel<UserBlockListModel> UserBlockList(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ChatBlockManager.UserBlockList\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            return _chatBlockRepository.UserBlockList(userId, pagingModel);
        }

        #endregion Methods
    }
}