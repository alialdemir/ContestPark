using System;
using System.ComponentModel;

namespace ContestPark.Entities.Models
{
    public class ChatHistoryModel : BaseModel, INotifyPropertyChanged
    {
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged(nameof(Message));
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        private string picturePath = DefaultImages.DefaultProfilePicture;

        public string PicturePath
        {
            get { return picturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    picturePath = value;
                    NotifyPropertyChanged(nameof(PicturePath));
                }
            }
        }

        private string senderId;

        public string SenderId
        {
            get { return senderId; }
            set
            {
                senderId = value;
                NotifyPropertyChanged(nameof(SenderId));
            }
        }

        public string UserName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}