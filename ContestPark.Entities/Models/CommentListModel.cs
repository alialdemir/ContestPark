using System;

namespace ContestPark.Entities.Models
{
    public class CommentListModel
    {
        public DateTime Date { get; set; }
        private string _profilePicturePath;

        public string ProfilePicturePath
        {
            get { return _profilePicturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value)) _profilePicturePath = value;
                else _profilePicturePath = DefaultImages.DefaultProfilePicture;
            }
        }

        public string Text { get; set; }
        public string UserName { get; set; }
    }
}