//using System.ComponentModel.DataAnnotations;
namespace ContestPark.Entities.Models
{
    public class UserRegisterModel
    {
        //[Required(ErrorMessage = "ServerMessages_userNameRequiredFields")]
        //[MinLength(3, ErrorMessage = "ServerMessages_userNameMinLength")]
        //[MaxLength(255, ErrorMessage = "ServerMessages_userNameMaxLength")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "ServerMessages_fullNameRequiredFields")]
        //[MinLength(3, ErrorMessage = "ServerMessages_fullNameMinLength")]
        //[MaxLength(255, ErrorMessage = "ServerMessages_fullNameMaxLength")]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "ServerMessages_emailRequiredFields")]
        //[MaxLength(255, ErrorMessage = "ServerMessages_emailMaxLength")]
        //[EmailAddress(ErrorMessage = "ServerMessages_emailFormating")]
        public string Email { get; set; }

        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "ServerMessages_passwordRequiredFields")]
        //[MinLength(8, ErrorMessage = "ServerMessages_passwordMinLength")]
        //[MaxLength(32, ErrorMessage = "ServerMessages_passwordMaxLength")]
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                ConfirmPassword = value;
            }
        }

        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "ServerMessages_notEqualsConfirmPassword")]
        public string ConfirmPassword { get; set; }

        public string LanguageCode { get; set; }
    }
}