using System;

namespace ContestPark.Entities.Models
{
    public class TokenResponseModel
    {
        public string fullName;

        // [JsonProperty("access_token")]
        public string access_token { get; set; }

        //   [JsonProperty("token_type")]
        //public string token_type { get; set; }
        //   [JsonProperty("expires_in")]
        //public int expires_in { get; set; }
        //   [JsonProperty("userName")]
        //public string userName { get; set; }
        //   [JsonProperty("UserCoverPicturePath")]
        private string _userCoverPicturePath = DefaultImages.DefaultCoverPicture;

        public string UserCoverPicturePath
        {
            get { return _userCoverPicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _userCoverPicturePath = value; }
        }

        //  [JsonProperty("UserProfilePicturePath")]
        private string _userProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string UserProfilePicturePath
        {
            get { return _userProfilePicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _userProfilePicturePath = value; }
        }

        //   [JsonProperty(".issued")]
        //public DateTime issued { get; set; }
        //   [JsonProperty(".expires")]
        public DateTime expires { get; set; }

        public string language { get; set; }
        public string userId { get; set; }
    }
}