using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class NotificationType : EntityBase
    {
        public int NotificationTypeId { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<NotificationTypeLang> NotificationTypeLangs { get; set; } = new HashSet<NotificationTypeLang>();
    }
}