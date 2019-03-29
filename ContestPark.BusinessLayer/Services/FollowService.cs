using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class FollowService : ServiceBase<Follow>, IFollowService
    {
        #region Private Variables

        private IFollowRepository _followRepository;
        private INotificationService _notificationService;
        private IPostService _PostService;

        #endregion Private Variables

        #region Constructors

        public FollowService(IFollowRepository followRepository,
            INotificationService notificationService,
            IPostService PostService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _followRepository = followRepository ?? throw new ArgumentNullException(nameof(followRepository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _PostService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcıların birbirini takip etme durumu
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı Id</param>
        /// <param name="followedUserId">takip edilen kullanıcı Id</param>
        /// <returns>Takip ediyorsa true etmiyorsa false</returns>
        public bool IsFollowUpStatus(string followUpUserId, string followedUserId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.IsFollowUpStatus\"");
            Check.IsNullOrEmpty(followUpUserId, nameof(followUpUserId));
            Check.IsNullOrEmpty(followedUserId, nameof(followedUserId));

            if (followUpUserId == followedUserId)
                return false;

            return _followRepository.IsFollowUpStatus(followUpUserId, followedUserId);
        }

        /// <summary>
        /// Kullanıcıyı takip etme
        /// </summary>
        /// <param name="entity">Takip entity</param>
        public new void Insert(Follow entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.Insert\"");
            Check.IsNull(entity, nameof(entity));

            if (entity.FollowUpUserId == entity.FollowedUserId)
                Check.BadStatus("ServerMessages_youCannotFollowASelf");

            bool isFollowUpStatus = IsFollowUpStatus(entity.FollowUpUserId, entity.FollowedUserId);
            if (isFollowUpStatus)
                Check.BadStatus("ServerMessages_thisUserAreAlreadyFollowing");

            base.Insert(entity);

            _notificationService.Insert(new Notification//Bildirim eklendi..
            {
                NotificationTypeId = (int)NotificationTypes.Follow,
                WhoId = entity.FollowUpUserId,
                WhonId = entity.FollowedUserId
            });

            bool isFollowControl = _PostService.IsFollowControl(entity.FollowUpUserId, entity.FollowedUserId);
            if (!isFollowControl)//Takip etti mesajı zaten gitmiş ise
            {
                _PostService
                    .Insert(new Post//Kim ne yapıyor eklendi..
                    {
                        ContestantId = entity.FollowedUserId,
                        UserId = entity.FollowUpUserId,
                        PostTypeId = (int)PostTypes.Follow,
                        ContestantContestStatus = true
                    });
            }
        }

        /// <summary>
        /// Kullanıcının Takip edenler(Takipçileri)
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takipçi listesi</returns>
        public ServiceModel<ListOfFollowerModel> Followers(string followedUserId, string followUpUserId, PagingModel pageModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.Followers\"");
            Check.IsNullOrEmpty(followedUserId, nameof(followedUserId));
            Check.IsNullOrEmpty(followUpUserId, nameof(followUpUserId));

            return _followRepository.Followers(followedUserId, followUpUserId, pageModel);
        }

        /// <summary>
        /// Kullanıcının takip ettikleri
        /// </summary>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takip ettiklerinin listesi</returns>
        public ServiceModel<ListOfFollowerModel> Following(string followedUserId, string followUpUserId, PagingModel pageModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.Following\"");
            Check.IsNullOrEmpty(followedUserId, nameof(followedUserId));
            Check.IsNullOrEmpty(followUpUserId, nameof(followUpUserId));

            return _followRepository.Following(followedUserId, followUpUserId, pageModel);
        }

        //string followUpUserId = takip eden , string followedUserId  = takip edilen
        /// <summary>
        /// İki kullanıcı arasındaki takip etme durumunu kaldırır
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        public void Delete(string followUpUserId, string followedUserId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.Delete\"");
            Check.IsNullOrEmpty(followUpUserId, nameof(followUpUserId));
            Check.IsNullOrEmpty(followedUserId, nameof(followedUserId));

            _followRepository.Delete(followUpUserId, followedUserId);
        }

        /// summary>
        /// Parametreden gelen kullanıcı id'ye ait takip eden kullanıcı sayısını dönrürür
        /// </summary>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <returns>Takip eden kullanıcı sayısı</returns>
        public int FollowersCount(string followedUserId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.FollowersCount\"");
            Check.IsNullOrEmpty(followedUserId, nameof(followedUserId));

            return _followRepository.FollowersCount(followedUserId);
        }

        /// <summary>
        /// Parametreden gelen kullanıcı id'ye ait takip ettiği kullanıcıların sayısını döndürür
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <returns>Takip ettiği kullanıcı sayısı</returns>
        public int FollowUpCount(string followUpUserId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.FollowUpCount\"");
            Check.IsNullOrEmpty(followUpUserId, nameof(followUpUserId));

            return _followRepository.FollowUpCount(followUpUserId);
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takip ettiği kullanıcı listesi
        /// </summary>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>kullanıcı listesi</returns>
        public ServiceModel<ChatPeopleModel> FollowingChatList(string followedUserId, string search, PagingModel pageModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowManager.FollowingChatList\"");
            Check.IsNullOrEmpty(followedUserId, nameof(followedUserId));
            if (search == null) search = "";

            return _followRepository.FollowingChatList(followedUserId, search.ToLower(), pageModel);
        }

        #endregion Methods
    }
}