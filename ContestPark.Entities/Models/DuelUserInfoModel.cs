using System.ComponentModel;

namespace ContestPark.Entities.Models
{
    public class DuelUserInfoModel : INotifyPropertyChanged
    {
        private string _founderProfilePicturePath;

        public string FounderProfilePicturePath
        {
            get { return _founderProfilePicturePath; }
            set
            {
                _founderProfilePicturePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FounderProfilePicturePath)));
            }
        }

        public string FounderFullName { get; set; }
        public string CompetitorProfilePicturePath { get; set; }
        public string CompetitorFullName { get; set; }
        public byte FounderScore { get; set; }
        public byte CompetitorScore { get; set; }
        public bool IsFounder { get; set; }
        public int Bet { get; set; }
        public string SubCategoryPicturePath { get; set; }
        public string SubCategoryName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}