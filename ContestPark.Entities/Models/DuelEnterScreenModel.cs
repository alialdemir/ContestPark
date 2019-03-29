using System;
using System.ComponentModel;

namespace ContestPark.Entities.Models
{
    public class DuelEnterScreenModel : BaseModel, INotifyPropertyChanged
    {
        private string _founderProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string FounderProfilePicturePath
        {
            get { return _founderProfilePicturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _founderProfilePicturePath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FounderProfilePicturePath)));
                }
            }
        }

        private string _founderCoverPicturePath = DefaultImages.DefaultCoverPicture;

        public string FounderCoverPicturePath
        {
            get { return _founderCoverPicturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _founderCoverPicturePath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FounderCoverPicturePath)));
                }
            }
        }

        private string _founderFullName;

        public string FounderFullName
        {
            get { return _founderFullName; }
            set
            {
                _founderFullName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FounderFullName)));
            }
        }

        private string _competitorProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string CompetitorProfilePicturePath
        {
            get { return _competitorProfilePicturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _competitorProfilePicturePath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompetitorProfilePicturePath)));
                }
            }
        }

        private string _competitorCoverPicturePath = DefaultImages.DefaultCoverPicture;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CompetitorCoverPicturePath
        {
            get { return _competitorCoverPicturePath; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _competitorCoverPicturePath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompetitorCoverPicturePath)));
                }
            }
        }

        private string _competitorFullName;

        public string CompetitorFullName
        {
            get { return _competitorFullName; }
            set
            {
                _competitorFullName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompetitorFullName)));
            }
        }

        private string _founderDegree;

        public string FounderDegree
        {
            get { return _founderDegree; }
            set
            {
                _founderDegree = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FounderDegree)));
            }
        }

        private string _competitorDegree;

        public string CompetitorDegree
        {
            get { return _competitorDegree; }
            set
            {
                _competitorDegree = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompetitorDegree)));
            }
        }

        public int DuelId { get; set; }
    }
}