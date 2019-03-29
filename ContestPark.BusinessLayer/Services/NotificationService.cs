using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

//using PushBots.NET;
//using PushBots.NET.Enums;
//using PushBots.NET.Models;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class NotificationService : ServiceBase<Notification>, INotificationService
    {
        #region Private Variables

        private INotificationRepository _notificationRepository;

        #endregion Private Variables

        #region Constructors

        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }

        #endregion Constructors

        #region Private methods

        /// <summary>
        /// Push notification gönderme
        /// </summary>
        /// <param name="notificationTypes"></param>
        /// <param name="userId"></param>
        private void PushNotificationClients(NotificationTypes notificationTypes, string userId)
        {
            // PushNotificationInfo pushNotificationInfo = PushNotificationInfo(notificationTypes, userId);
            //this.PushNotificationClients(pushNotificationInfo.UserName + " " + pushNotificationInfo.NotificationMessage, pushNotificationInfo.UserName);
        }

        #endregion Private methods

        #region Methods

        /// <summary>
        /// Push Notification gönderme
        /// </summary>
        /// <param name="message">Mesaj</param>
        /// <param name="userName">Kullanıcı adı</param>
        public void PushNotificationClients(string message, string userName)
        {
            /* if (!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(userName))
            {//Push notification pasif buradan aktif edilcek
             PushBotsClient client = new PushBotsClient("5629312a177959bd1d8b4567", "6c755d48905581d7f78b2970bf66a668");
              BatchPush pushMessage = new BatchPush()
              {
                  Message = message,
                  Badge = "+1",
                  Platforms = new[] { Platform.Android, Platform.iOS },
                  Alias = userName
              };
              client.Push(pushMessage)
            }  */
        }

        /// <summary>
        /// Bildirim ekleme
        /// </summary>
        /// <param name="entity">Bildirim entity</param>
        public override void Insert(Notification entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.Insert\"");
            Check.IsNull(entity, nameof(entity));

            if (entity.Who == entity.Whon)
                return;

            entity.NotificationDate = DateTime.Now;
            entity.NotificationNumberStatus = false;
            entity.Status = true;
            base.Insert(entity);
            //   PushNotificationClients((NotificationTypes)entity.NotificationTypeId, entity.Who);
        }

        /// <summary>
        /// Bildirim silme
        /// </summary>
        /// <param name="id">Bildirim id</param>
        /// <param name="userId">Kullanıcı id</param>
        public void Delete(int id, string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.Delete\"");
            Check.IsLessThanZero(id, nameof(id));
            Check.IsNullOrEmpty(userId, nameof(userId));

            bool idControl = Find(p => p.WhoId == userId && p.NotificationId == id).Any();

            if (!idControl)
                Check.BadStatus("This id is not yours");// TODO: Çoklu dil desteği getir
            base.Delete(id);
        }

        /// <summary>
        /// Kullanıcının tüm  bildirimleri
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Bildirim listesi</returns>
        public ServiceModel<UserNotificationListModel> UserNotificationAll(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.UserNotificationAll\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _notificationRepository.UserNotificationAll(userId, pagingModel);
        }

        /// <summary>
        /// Kullanıcının görünmeyen bildirim sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Bildirim sayısı</returns>
        public int UserNotificationVisibilityCount(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.UserNotificationVisibilityCount\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _notificationRepository.UserNotificationVisibilityCount(userId);
        }

        /// <summary>
        /// Bildirimleri görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        public void NotificationSeen(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.NotificationSeed\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            _notificationRepository.NotificationSeen(userId);
        }

        /// <summary>
        /// Push notification göndermek için kullanıcı ve bildirim bilgisi getirir
        /// </summary>
        /// <param name="notificationTypes">Notification tipi</param>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Bildirim  ve kullanıcı bilgisi</returns>
        public PushNotificationInfoModel PushNotificationInfo(NotificationTypes notificationTypes, string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.PushNotificationInfo\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _notificationRepository.PushNotificationInfo(notificationTypes, userId);
        }

        /// <summary>
        /// Link beğenmede ilgili linke son beğenildiğinde atılan notification tarihini verir bunu zırt pırt bildirim gitmesin diye koydum
        /// </summary>
        /// <param name="who">Bildirim gönderen Id</param>
        /// <param name="whon">Bildirim alan Id</param>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>En son atılan bildirim tarihi</returns>
        public DateTime LastLikeNotificationTime(string who, string whon, int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.NotificationManager.LastLikeNotificationTime\"");
            Check.IsNullOrEmpty(who, nameof(who));
            Check.IsNullOrEmpty(whon, nameof(whon));
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _notificationRepository.LastLikeNotificationTime(who, whon, PostId);
        }

        #endregion Methods
    }
}