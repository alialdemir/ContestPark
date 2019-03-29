using System;

namespace ContestPark.Entities.Models
{
    public class ChatPeopleModel : BaseModel
    {
        public string FullName { get; set; }
        private string profilePicturePath = DefaultImages.DefaultProfilePicture;

        public string ProfilePicturePath
        {
            get { return profilePicturePath; }
            set
            {
                if (!string.IsNullOrEmpty(value)) profilePicturePath = value;
            }
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool OnlineStatus { get; set; }
        public DateTime LastActiveDate { get; set; }
    }
}