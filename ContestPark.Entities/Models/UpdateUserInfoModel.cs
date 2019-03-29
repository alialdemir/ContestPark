//using System.ComponentModel.DataAnnotations;
using System;

namespace ContestPark.Entities.Models
{
    public class UpdateUserInfoModel
    {
        //[Required(ErrorMessage = "serverMessages.fullNameRequiredFields")]
        //[MinLength(3, ErrorMessage = "serverMessages.fullNameMinLength")]
        //[MaxLength(255, ErrorMessage = "serverMessages.fullNameMaxLength")]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "serverMessages.userNameRequiredFields")]
        //[MinLength(3, ErrorMessage = "serverMessages.userNameMinLength")]
        //[MaxLength(255, ErrorMessage = "serverMessages.userNameMaxLength")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "serverMessages.emailRequiredFields")]
        //[MaxLength(255, ErrorMessage = "serverMessages.emailMaxLength")]
        //[EmailAddress(ErrorMessage = "serverMessages.emailFormating")]
        public string Email { get; set; }

        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        private string _userCoverPicturePath = DefaultImages.DefaultCoverPicture;

        public string UserCoverPicturePath
        {
            get { return _userCoverPicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _userCoverPicturePath = value; }
        }

        private string _userProfilePicturePath = DefaultImages.DefaultProfilePicture;

        public string UserProfilePicturePath
        {
            get { return _userProfilePicturePath; }
            set { if (!String.IsNullOrEmpty(value)) _userProfilePicturePath = value; }
        }
    }
}