namespace ContestPark.Entities.Models
{
    public class ScoreRankingModel : BaseModel
    {
        //IGlobalSettings _globalSettings;
        //public ScoreRankingModel()
        //{
        //    _globalSettings = new GlobalSettingsManager();
        //}
        private int _totalScore;

        public int TotalScore
        {
            get { return _totalScore; }
            set
            {
                _totalScore = value;// _globalSettings.NumberFormating(Convert.ToInt32(value));
            }
        }

        public string UserFullName { get; set; }
        private string userProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string UserProfilePicturePath
        {
            get { return userProfilePicturePath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    userProfilePicturePath = value;
                }
            }
        }

        public string UserName { get; set; }
    }
}