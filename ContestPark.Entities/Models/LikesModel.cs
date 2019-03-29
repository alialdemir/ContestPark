using System;

namespace ContestPark.Entities.Models
{
    public class LikesModel : BaseModel
    {
        public string FollowUpUserId { get; set; }
        public string FullName { get; set; }
        public bool IsFollowUpStatus { get; set; }
        private string _profilePicturePath = DefaultImages.DefaultProfilePicture;

        public string ProfilePicturePath
        {
            get { return _profilePicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _profilePicturePath = value; }
        }

        public string UserName { get; set; }
    }
}