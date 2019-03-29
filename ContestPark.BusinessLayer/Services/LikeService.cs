using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class LikeService : ServiceBase<Like>, ILikeService
    {
        #region Private Variables

        private ILikeRepository _likeRepository;
        private INotificationService _notificationService;
        private IPostService _PostService;

        #endregion Private Variables

        #region Constructors

        public LikeService(ILikeRepository likeRepository,
            INotificationService notificationService,
            IPostService PostService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _likeRepository = likeRepository ?? throw new ArgumentNullException(nameof(likeRepository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _PostService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Post beğeni ekleme
        /// </summary>
        /// <param name="entity">Like entity</param>
        public override void Insert(Like entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.LikeManager.Insert\"");
            Check.IsNull(entity, nameof(entity));

            if (IsLike(entity.UserId, entity.PostId))
                Check.BadStatus("serverMessages_inThisPostYouHaveAlreadyLiked");

            base.Insert(entity);

            PostInfoModel PostInfo = _PostService.GetUserId(entity.PostId);
            Check.IsNull(PostInfo, nameof(PostInfo));

            DateTime lastLikeNotificationTime = _notificationService.LastLikeNotificationTime(entity.UserId, PostInfo.UserId, entity.PostId);
            TimeSpan time = DateTime.Now - lastLikeNotificationTime;
            if (time.Minutes >= 28)//28 dakkika da bir bildirim gitsin zırt pırt gitmesin
            {
                if (PostInfo.UserId != entity.UserId)//Kendi bağlantısını beğenirse bildirim gitmesin..
                {
                    _notificationService.Insert(new Notification//Bildirim eklendi..
                    {
                        NotificationTypeId = (int)NotificationTypes.LinkLike,
                        WhoId = entity.UserId,
                        WhonId = PostInfo.UserId,
                        Link = entity.PostId.ToString()
                    });
                }

                if (PostInfo.PostTypes.HasFlag(PostTypes.ContestDuel))
                {//Eğer bilgi yada müzik yarışmasının kim ne yapıyoru beğenilmiş ise rakibe bildirim gitmesini sağladık...
                    if (PostInfo.ContestantId != entity.UserId)//Üçüncü bir kişi beğeni yapıyor ise rakibe de bildirim gönderdik
                    {
                        _notificationService.Insert(new Notification//Beğendiğine dair bildirim gönderildi.
                        {
                            WhoId = entity.UserId,
                            NotificationTypeId = (int)NotificationTypes.LinkLike,
                            Link = entity.PostId.ToString(),
                            WhonId = PostInfo.ContestantId
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Parametreden gelen kullanıcı id'sinin yine parametreden gelen kim ne yapıyor id'sini beğenme durumu
        /// Beğendiyse true beğenmediyse false dönder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns>Beğendiyse true beğenmediyse false</returns>
        public bool IsLike(string userId, int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.LikeManager.IsLike\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _likeRepository.IsLike(userId, PostId);
        }

        /// <summary>
        /// Parametreden gelen kim ne yapıyor id'sini beğenen kullanıcı sayısı
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns>Beğenen kullanıcı sayısı</returns>
        public int LikeCount(int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.LikeManager.LikeCount\"");
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _likeRepository.LikeCount(PostId);
        }

        /// <summary>
        /// Kim ne yapıyoru beğenmekten vazgeç
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">Kim ne yapıyor id</param>
        public void DisLike(string userId, int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.LikeManager.DisLike\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(PostId, nameof(PostId));

            _likeRepository.DisLike(userId, PostId);
        }

        /// <summary>
        /// Parametreden gelen kim ne yapıyor id'sini beğenen kullanıcılar
        /// ve
        /// yine parametreden gelen kullanıcının o postu beğenme durumu
        /// IsFollowUpStatus: beğendiyse true beğenmediyse false
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">Kim ne apıyor id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Beğenen kullanıcı bilgileri</returns>
        public ServiceModel<LikesModel> Likes(string userId, int PostId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.LikeManager.Likes\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _likeRepository.Likes(userId, PostId, pagingModel);
        }

        #endregion Methods
    }
}