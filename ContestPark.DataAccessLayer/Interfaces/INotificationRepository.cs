using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        ServiceModel<UserNotificationListModel> UserNotificationAll(string userId, PagingModel pagingModel);

        int UserNotificationVisibilityCount(string userId);

        void NotificationSeen(string userId);

        PushNotificationInfoModel PushNotificationInfo(NotificationTypes notificationTypes, string userId);

        DateTime LastLikeNotificationTime(string who, string whon, int postId);
    }
}