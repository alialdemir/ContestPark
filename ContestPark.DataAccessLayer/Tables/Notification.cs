using System;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Notification : EntityBase
    {
        public int NotificationId { get; set; }
        public int NotificationTypeId { get; set; }
        public string WhonId { get; set; }
        public User Whon { get; set; }
        public string WhoId { get; set; }
        public User Who { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool Status { get; set; }
        public bool NotificationNumberStatus { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public int SubCategoryId { get; set; }
        private string link = String.Empty;

        public string Link
        {
            get
            {
                return String.IsNullOrEmpty(link) ? "#" : link;
            }
            set { link = value; }
        }
    }
}