using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class NotificationDapperRepository : DapperRepositoryBase<Notification>, INotificationRepository
    {
        #region Constructor

        public NotificationDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Link beğenmede ilgili linke son beğenildiğinde atılan notification tarihini verir bunu zırt pırt bildirim gitmesin diye koydum
        /// </summary>
        /// <param name="whoId">Bildirim gönderen Id</param>
        /// <param name="whonId">Bildirim alan Id</param>
        /// <param name="postId">Kim ne yapıyor Id</param>
        /// <returns>En son atılan bildirim tarihi</returns>
        public DateTime LastLikeNotificationTime(string whoId, string whonId, int postId)
        {
            string sql = @"select top(1) [n].[NotificationDate] from [Notifications] as [n]
                           where [n].[WhoId]=@WhoId and [n].[WhonId]=@WhonId and [n].[NotificationTypeId]=@NotificationTypeId and [n].[Link]=@PostId
                           order by [n].[CreatedDate] desc";
            DateTime notificationDate = Connection.Query<DateTime>(sql, new
            {
                WhoId = whoId,
                WhonId = whonId,
                NotificationTypeId = (int)NotificationTypes.LinkLike,
                PostId = postId
            }).FirstOrDefault();
            return notificationDate != null ? notificationDate : new DateTime();
        }

        /// <summary>
        /// Bildirimleri görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        public void NotificationSeen(string userId)
        {
            string sql = "Update Notifications set NotificationNumberStatus=1 where WhonId=@UserId and NotificationNumberStatus=0";
            Connection.Execute(sql, new { UserId = userId });
        }

        /// <summary>
        /// Push notification göndermek için kullanıcı ve bildirim bilgisi getirir
        /// </summary>
        /// <param name="notificationTypes">Notification tipi</param>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Bildirim  ve kullanıcı bilgisi</returns>
        public PushNotificationInfoModel PushNotificationInfo(NotificationTypes notificationTypes, string userId)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string sql = @"select top(1) [nl].[NotificationName] as [NotificationMessage], [u].[UserName]
                           from [NotificationTypeLangs] as [nl], [AspNetUsers] as [u]
                           where [u].[Id]=@UserId and [nl].[NotificationTypeId]=@NotificationTypeId and [nl].[LanguageId]=@LanguageId";
            return Connection.Query<PushNotificationInfoModel>(sql, new { UserId = userId, NotificationTypeId = (int)notificationTypes, LanguageId = language }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının görünmeyen bildirim sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Bildirim sayısı</returns>
        public int UserNotificationVisibilityCount(string userId)
        {
            string sql = "select Count([n].[NotificationId]) from [Notifications] as [n] where [n].[WhonId]=@UserId and [n].[NotificationNumberStatus]=0";
            return Connection.Query<int>(sql, new { UserId = userId }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının tüm  bildirimleri
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Bildirim listesi</returns>
        public ServiceModel<UserNotificationListModel> UserNotificationAll(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from b in DbSet
                    let langId = (byte)language
                    //  join btur   in DbContext.Set<NotificationTypeLang>() on b.NotificationTypeId equals btur.NotificationTypeId
                    join scl in DbContext.Set<SubCategoryLang>() on b.SubCategoryId equals scl.SubCategoryId into cData
                    from cData1 in cData.DefaultIfEmpty()
                    orderby b.NotificationDate descending
                    where b.WhonId == userId && /*btur.LanguageId == langId &&*/ (cData1 != null && cData1.LanguageId == langId)
                    select new UserNotificationListModel
                    {
                        NotificationId = b.NotificationId,
                        NotificationDate = b.NotificationDate,
                        NotificationType = b.NotificationType
                                            .NotificationTypeLangs
                                            .Where(p => p.LanguageId == langId)
                                            .Select(m => m.NotificationName)
                                            .FirstOrDefault()
                                            .Replace("{yarisma}", cData1 != null ? cData1.SubCategoryName : "").Replace("{kullaniciadi}", b.Who.FullName + ","),
                        NotificationTypeId = b.NotificationTypeId,
                        NotificationStatus = b.Status,
                        WhoFullName = b.Who.FullName,
                        WhoUserName = b.Who.UserName,
                        PicturePath = b.Who.ProfilePicturePath,
                        Link = b.Link,
                        Date = b.NotificationDate,
                        SubCategoryId = b.SubCategoryId,
                        IsContest = b.SubCategoryId > 0 ? true : false
                    }).ToServiceModel(pagingModel);
        }

        #endregion Methods
    }
}