using SQLite.Net.Attributes;

namespace ContestPark.Mobile.Models
{
    public class UserModel
    {
        [PrimaryKey]
        public string UserName { get; set; }

        public string Password { get; set; }
        public string AccessToken { get; set; }
        public string UserCoverPicturePath { get; set; }
        public string UserProfilePicturePath { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
}