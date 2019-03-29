using System;

namespace ContestPark.Entities.Models
{
    public class DuelResultModel
    {
        public string FounderUserId { get; set; }
        public string FounderFullName { get; set; }
        private string _founderProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string FounderProfilePicturePath
        {
            get { return _founderProfilePicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _founderProfilePicturePath = value; }
        }

        public int FounderTrueAnswerCount { get; set; }
        public string FounderUserName { get; set; }
        public bool FounderNullAnswerCount { get; set; }
        public bool FounderEscapeStatus { get; set; }
        public byte FounderScorePoint { get; set; }
        public string CompetitorUserId { get; set; }
        public string CompetitorFullName { get; set; }
        private string _competitorProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string CompetitorProfilePicturePath
        {
            get { return _competitorProfilePicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _competitorProfilePicturePath = value; }
        }

        public int CompetitorTrueAnswerCount { get; set; }
        public string CompetitorUserName { get; set; }
        public bool CompetitorNullAnswerCount { get; set; }
        public bool CompetitorEscapeStatus { get; set; }
        public byte CompetitorScorePoint { get; set; }
        public int Bet { get; set; }
    }
}