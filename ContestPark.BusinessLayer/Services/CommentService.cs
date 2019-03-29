using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class CommentService : ServiceBase<Comment>, ICommentService
    {
        #region Private Variables

        private ICommentRepository _commentRepository;
        private INotificationService _notificationService;
        private IPostService _PostService;

        #endregion Private Variables

        #region Constructors

        public CommentService(ICommentRepository commentRepository,
            INotificationService notificationService,
            IPostService PostService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _PostService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// İlgili posta yapılan yorum sayısı
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>Yorum sayısı</returns>
        public int CommentCount(int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CommentManager.CommentCount\"");
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _commentRepository.CommentCount(PostId);
        }

        /// <summary>
        /// İlgili posta yapılan yorumlar
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>Post listesi</returns>
        public ServiceModel<CommentListModel> CommentList(int PostId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CommentManager.CommentList\"");
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _commentRepository.CommentList(PostId, pagingModel);
        }

        /// <summary>
        /// Yorum ekleme
        /// </summary>
        /// <param name="entity">Yorum entitys</param>
        public new void Insert(Comment entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CommentManager.Insert\"");
            Check.IsNull(entity, nameof(entity));

            base.Insert(entity);

            SendCommentNotification(entity);
        }

        /// <summary>
        /// Commente önceden yorum yapmış kullanıcılara bildirim gönderir.
        /// </summary>
        /// <param name="entity">Yorum entitys</param>
        private void SendCommentNotification(Comment entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CommentManager.SendCommentNotification\"");
            DateTime lastCommentTime = _commentRepository.LastCommentTime(entity.UserId, entity.PostId);
            TimeSpan time = entity.CreatedDate - lastCommentTime;
            if (time.Minutes < 28)//28 dakkika da bir bildirim gitsin zırt pırt gitmesin
                return;
            var PostInfo = _PostService.GetUserId(entity.PostId);
            if (PostInfo.UserId != entity.UserId)//Kendi bağlantısına yorum yapmış ise bildirim gitmesin..
            {
                _notificationService.Insert(new Notification
                {
                    NotificationTypeId = (int)NotificationTypes.Comment,
                    WhoId = entity.UserId,
                    WhonId = PostInfo.UserId,
                    Link = entity.PostId.ToString()
                });
            }

            if (PostInfo.PostTypes.HasFlag(PostTypes.ContestDuel) && PostInfo.ContestantId != entity.UserId)
            {//Eğer yarışmaya ait kim ne yapıyor yorumu yapmış ise rakibe bildirim gitmesini sağladık... Üçüncü bir kişi yorum yapmış ise rakibe de bildirim gönderdik
                _notificationService.Insert(new Notification
                {
                    WhoId = entity.UserId,
                    NotificationTypeId = (int)NotificationTypes.Comment,
                    Link = entity.PostId.ToString(),
                    WhonId = PostInfo.ContestantId
                });
            }
        }

        /// <summary>
        /// Kullanıcının en son posta yaptığı yorumun tarihi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>En son yapılan yorumun tarihi</returns>
        public DateTime LastCommentTime(string userId, int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CommentManager.LastCommentTime\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _commentRepository.LastCommentTime(userId, PostId);
        }

        #endregion Methods
    }
}