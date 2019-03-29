using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface INotificationService : IRepository<Notification>
    {
        ServiceModel<UserNotificationListModel> UserNotificationAll(string userId, PagingModel pagingModel);

        int UserNotificationVisibilityCount(string userId);

        void NotificationSeen(string userId);

        PushNotificationInfoModel PushNotificationInfo(NotificationTypes notificationTypes, string userId);

        void PushNotificationClients(string message, string userName);

        DateTime LastLikeNotificationTime(string who, string whon, int PostId);

        void Delete(int id, string userId);
    }
}