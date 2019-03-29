using System;
using System.ComponentModel;

namespace ContestPark.Entities.Models
{
    public class UserProfilePageModel : INotifyPropertyChanged
    {
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserId)));
            }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
            }
        }

        private string coverPicture = DefaultImages.DefaultCoverPicture;

        public string CoverPicture
        {
            get { return coverPicture; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    coverPicture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CoverPicture)));
                }
            }
        }

        private string userProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserProfilePicturePath
        {
            get { return userProfilePicturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    userProfilePicturePath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserProfilePicturePath)));
                }
            }
        }
    }
}