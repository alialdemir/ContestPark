using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfNotificationDal : EfEntityRepositoryBase<Notification>, INotificationRepository
    {
        #region Constructors

        public EfNotificationDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Link beğenmede ilgili linke son beğenildiğinde atılan notification tarihini verir bunu zırt pırt bildirim gitmesin diye koydum
        /// </summary>
        /// <param name="who">Bildirim gönderen Id</param>
        /// <param name="whon">Bildirim alan Id</param>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>En son atılan bildirim tarihi</returns>
        public DateTime LastLikeNotificationTime(string who, string whon, int PostId)
        {
            DateTime notificationDate = DbSet
                                    .Where(c => c.WhoId == who && c.WhonId == whon && c.NotificationTypeId == (int)NotificationTypes.LinkLike && c.Link == PostId.ToString())
                                    .OrderByDescending(p => p.NotificationDate)
                                    .Select(p => p.NotificationDate)
                                    .FirstOrDefault();
            return notificationDate != null ? notificationDate : new DateTime();
        }

        /// <summary>
        /// Bildirimleri görüldü yapar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        public void NotificationSeen(string userId)//Görünmeyen bildirimleri görüldü yapar
        {
            DbContext.Database.ExecuteSqlCommand($"Update Notifications set NotificationNumberStatus='True' where Whon={userId} and NotificationNumberStatus='False'");
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
            return (from nt in DbContext.Set<NotificationTypeLang>()
                    where nt.NotificationTypeId == (int)notificationTypes && nt.LanguageId == (byte)language
                    from u in DbContext.Set<User>()
                    where u.Id == userId
                    select new PushNotificationInfoModel
                    {
                        NotificationMessage = nt.NotificationName,
                        UserName = u.UserName
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının görünmeyen bildirim sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Bildirim sayısı</returns>
        public int UserNotificationVisibilityCount(string userId)
        {
            return DbSet
                        .Where(p => p.WhonId == userId && p.NotificationNumberStatus == false)
                        .Count();
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
                    //  join btur in DbContext.Set<NotificationTypeLang>() on b.NotificationTypeId equals btur.NotificationTypeId
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