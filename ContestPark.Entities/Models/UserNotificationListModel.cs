using System;

namespace ContestPark.Entities.Models
{
    public class UserNotificationListModel : BaseModel
    {
        public int NotificationId { get; set; }
        public DateTime NotificationDate { get; set; }
        public string NotificationType { get; set; }
        public string WhoFullName { get; set; }
        private string _picturePath = DefaultImages.DefaultProfilePicture;

        public string PicturePath
        {
            get { return _picturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value)) _picturePath = value;
            }
        }

        public int NotificationTypeId { get; set; }
        public bool NotificationStatus { get; set; }
        public string WhoUserName { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public int SubCategoryId { get; set; }
        public bool IsContest { get; set; }
    }
}