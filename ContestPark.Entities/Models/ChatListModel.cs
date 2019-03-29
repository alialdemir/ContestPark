using System;
using System.ComponentModel;

namespace ContestPark.Entities.Models
{
    public class ChatListModel : BaseModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string senderUserId;

        public string SenderUserId
        {
            get { return senderUserId; }
            set
            {
                senderUserId = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SenderUserId)));
                }
            }
        }

        private string userProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string UserProfilePicturePath
        {
            get { return userProfilePicturePath; }
            set
            {
                if (value != null)
                {
                    userProfilePicturePath = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(UserProfilePicturePath)));
                    }
                }
            }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Date)));
                }
            }
        }

        private string userFullName;

        public string UserFullName
        {
            get { return userFullName; }
            set
            {
                userFullName = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(UserFullName)));
                }
            }
        }

        private bool visibilityStatus = false;

        public bool VisibilityStatus
        {
            get { return visibilityStatus; }
            set
            {
                visibilityStatus = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilityStatus)));
                }
            }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(UserName)));
                }
            }
        }
    }
}