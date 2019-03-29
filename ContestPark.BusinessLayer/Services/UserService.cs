using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities;
using ContestPark.Entities.AppResources;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Services
{
    public class UserService : ServiceBase<User>, IUserService
    {
        #region Private Variables

        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IUserRepository _userRepository;
        private IGlobalSettings _globalSettings;
        private IPostService _PostService;
        private readonly IEmailSender _emailSender;

        #endregion Private Variables

        #region Constructors

        public UserService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository,
            IGlobalSettings globalSettings,
            IPostService PostService,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _globalSettings = globalSettings ?? throw new ArgumentNullException(nameof(globalSettings));
            _PostService = PostService ?? throw new ArgumentNullException(nameof(PostService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Şifremi unuttum
        /// </summary>
        /// <param name="userNameOrPassword">Kullaıcı adı veya eposta</param>
        /// <returns>Kullanıcının yeni şifresi email adresine gönderilmiş ise true aksi durumda false</returns>
        public async Task<bool> ForgotYourPasswordAsync(string userNameOrPassword)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserService.ForgotYourPasswordAsync\"");
            Check.IsNullOrEmpty(userNameOrPassword, nameof(userNameOrPassword));

            User forgetYourPassword = await FindByEmailOrNameAsync(userNameOrPassword);
            if (forgetYourPassword == null) Check.BadStatus(ContestParkResources.ServerMessage_ourRegisteredMembersOfThisInformationDoesNotExist);

            string newPasswordstr = _globalSettings.GenerateRandomString(12);// Kendim rastgele şifre verdim

            await ResetPasswordAsync(forgetYourPassword, newPasswordstr);

            string text = ForgotPasswordEmailMessage(forgetYourPassword.Email, forgetYourPassword.UserName, newPasswordstr);
            return _emailSender.EmailSend(forgetYourPassword.Email, "ContestPark forgot password", text);
        }

        private async Task ResetPasswordAsync(User forgetYourPassword, string newPasswordstr)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(forgetYourPassword);// şifre değiştirme için kod oluşturuldu
            IdentityResult result = await _userManager.ResetPasswordAsync(forgetYourPassword, code, newPasswordstr);// şifre kayıt edildi
            if (!result.Succeeded) Check.BadStatus(ContestParkResources.ServerMessage_sendPasswordErrorMessage);
        }

        /// <summary>
        /// Şifremi unuttum email içeriği
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="newPasswordstr"></param>
        /// <returns></returns>
        private string ForgotPasswordEmailMessage(string email, string userName, string newPasswordstr)
        {
            return DateTime.Now.ToLocalTime() +
                           " ContestPark your password " + email +
                           "<br/><br/><strong>Username :</strong> " +
                           userName + "<br/><strong>New Password :</strong> " + newPasswordstr +
                           "  <strong><br/>Email Adress :</strong> " + email +
                           "<br/><br/>";
        }

        /// <summary>
        /// Kullanıcı adı veya eposta adresine göre user döndürür
        /// </summary>
        /// <param name="userNameOrPassword">Kullanıcı adı veya eposta</param>
        /// <returns>Kullanıcı nesnesi yoksa null</returns>
        private async Task<User> FindByEmailOrNameAsync(string userNameOrPassword)
        {
            if (_globalSettings.IsEmailValid(userNameOrPassword))
                return await _userManager.FindByEmailAsync(userNameOrPassword);// Eposta adresi ise eposta ile aradık

            return await _userManager.FindByNameAsync(userNameOrPassword);// Kullanıcı adı ile aradık
        }

        /// <summary>
        /// Parametreden gelen registerModel'in kontrollerini gerçeekleştirir
        /// </summary>
        /// <param name="registerModel"></param>
        private void UserEntityValidationControl(UserRegisterModel registerModel)
        {
            #region Validations

            if (registerModel == null) Check.BadStatus("signupPage_requiredFields");// Tüm alanlar boş ise
            else if (String.IsNullOrEmpty(registerModel.UserName)) Check.BadStatus("ServerMessage_userNameRequiredFields");// Kullanıcı adı boş ise
            else if (String.IsNullOrEmpty(registerModel.FullName)) Check.BadStatus("ServerMessage_fullNameRequiredFields");// Ad soyad boş ise
            else if (String.IsNullOrEmpty(registerModel.Email)) Check.BadStatus("ServerMessage_emailRequiredFields");// Eposta boş ise
            else if (String.IsNullOrEmpty(registerModel.Password)) Check.BadStatus("ServerMessage_passwordRequiredFields");// Şifre adı boş ise
            else if (registerModel.UserName.Length < 3) Check.BadStatus("ServerMessage_userNameMinLength");// Kullanocı adı 3 karakterden küçük olamaz
            else if (registerModel.UserName.Length > 255) Check.BadStatus("ServerMessage_userNameMaxLength");// Kullanocı adı 255 karakterden büyük olamaz
            else if (registerModel.FullName.Length < 3) Check.BadStatus("ServerMessage_fullNameMinLength");// Ad soyad 3 karakterden küçük olamaz
            else if (registerModel.FullName.Length > 255) Check.BadStatus("ServerMessage_fullNameMaxLength");// Ad soyad 255 karakterden büyük olamaz
            else if (registerModel.Email.Length > 255) Check.BadStatus("ServerMessage_emailMaxLength");// Eposta adresi 255 karakterden büyük olamaz
            else if (!new EmailAddressAttribute().IsValid(registerModel.Email)) Check.BadStatus("ServerMessage_emailFormating");// Eposta adresi formatı doğru mu
            else if (registerModel.Password.Length < 8) Check.BadStatus("ServerMessage_passwordMinLength");// Kullanocı adı 8 karakterden küçük olamaz
            else if (registerModel.Password.Length > 32) Check.BadStatus("ServerMessage_passwordMaxLength");// Kullanocı adı 32 karakterden büyük olamaz
            else if (!registerModel.Password.Equals(registerModel.ConfirmPassword)) Check.BadStatus("ServerMessage_notEqualsConfirmPassword");// Şifre ve şifre tekrarla uyuşmuyor ise
            else if (_globalSettings.TurkishCharacterControl(registerModel.UserName)) Check.BadStatus("ServerMessage_userNameTurkishCharactersCanNot");// Kullanıcı adında türkçe karakter olamaz
            else if (IsUserNameControl(registerModel.UserName)) Check.BadStatus("ServerMessage_thisUserNameWasUsedByAnotherUser");// Kullanıcı adı daha önce kullanılmış mı
            else if (IsEmailControl(registerModel.Email)) Check.BadStatus("ServerMessage_thisEmailAddressUsedByAnotherUser");// Eposta adresi daha önce kullanılmış mı

            #endregion Validations
        }

        /// <summary>
        /// Kullanıcı Id'sine göre kullanıcıya ait bilgileri getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kullanıcı bilgileri</returns>
        public UpdateUserInfoModel GetUpdateUserInfo(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.GetUpdateUserInfo\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _userRepository.GetUpdateUserInfo(userId);
        }

        /// <summary>
        /// Facebook bilgisine göre kullanıcı kayıt eder
        /// </summary>
        /// <param name="facebookLogin">Kullanıcının facebook bilgisi</param>
        /// <returns>User entity</returns>
        public User FacebookRegister(FacebookUserViewModel facebookLogin)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.FacebookRegister\"");
            Check.IsNull(facebookLogin, nameof(facebookLogin));

            facebookLogin.name = _globalSettings.ReplaceTurksihEnglishCharacter(facebookLogin.name);
            return _userRepository.FacebookRegister(facebookLogin);
        }

        public override void Insert(User entity)
        {
            // Bu entity için insert methodu kullanılmasın
        }

        /// <summary>
        /// Üye kaydetme işlemi
        /// </summary>
        /// <param name="registerModel">Üye modeli</param>
        /// <returns></returns>
        public async Task Insert(UserRegisterModel registerModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.Insert\"");
            Check.IsNull(registerModel, nameof(registerModel));

            UserEntityValidationControl(registerModel);//Validations

            DateTime systemDate = DateTime.Now;
            var user = new User()
            {
                UserName = registerModel.UserName.Trim().Replace(" ", ""),
                Email = registerModel.Email,
                RegistryDate = systemDate,
                LastActiveDate = systemDate,
                Status = true,
                FullName = _globalSettings.InitialsLarge(registerModel.FullName),
                LanguageCode = String.IsNullOrEmpty(registerModel.LanguageCode) ? "en-US" : registerModel.LanguageCode
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded) Check.BadStatus(result.Errors.FirstOrDefault().Description);
            else
            {
                string roleName = "User";
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (roleExist)
                    await _userManager.AddToRolesAsync(user, new string[] { roleName });
            }
        }

        /// <summary>
        /// Facebook Id'sine göre kullanıcı getir bulamazsa null User nesnesi döndürür
        /// </summary>
        /// <param name="facebookLogin">Facebook entity</param>
        /// <returns>User nesnesi</returns>
        public User FacebookLogin(string facebookId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.FacebookLogin\"");
            Check.IsNullOrEmpty(facebookId, nameof(facebookId));

            var user = _userRepository.FacebookLogin(facebookId);
            if (user != null)
            {
                user.LastActiveDate = DateTime.Now;
                Update(user);
            }
            return user;
        }

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <param name="entity">Kullanıcı entity</param>
        public override void Update(User entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.Update\"");
            if (entity == null)
                Check.BadStatus("signupPage_requiredFields");// Tüm alanlar boş ise

            User user = Find(x => x.Id == entity.Id).FirstOrDefault();
            Check.IsNull(user, nameof(user));

            #region Validations

            if (String.IsNullOrEmpty(entity.UserName)) Check.BadStatus("ServerMessage_userNameRequiredFields");// Kullanıcı adı boş ise
            else if (String.IsNullOrEmpty(entity.FullName)) Check.BadStatus("ServerMessage_fullNameRequiredFields");// Ad soyad boş ise
            else if (String.IsNullOrEmpty(entity.Email)) Check.BadStatus("ServerMessage_emailRequiredFields");// Eposta boş ise
            else if (entity.UserName.Length < 3) Check.BadStatus("ServerMessage_userNameMinLength");// Kullanocı adı 3 karakterden küçük olamaz
            else if (entity.UserName.Length > 255) Check.BadStatus("ServerMessage_userNameMaxLength");// Kullanocı adı 255 karakterden büyük olamaz
            else if (entity.FullName.Length < 3) Check.BadStatus("ServerMessage_fullNameMinLength");// Ad soyad 3 karakterden küçük olamaz
            else if (entity.FullName.Length > 255) Check.BadStatus("ServerMessage_fullNameMaxLength");// Ad soyad 255 karakterden büyük olamaz
            else if (entity.Email.Length > 255) Check.BadStatus("ServerMessage_emailMaxLength");// Eposta adresi 255 karakterden büyük olamaz
            else if (!new EmailAddressAttribute().IsValid(entity.Email)) Check.BadStatus("ServerMessage_emailFormating");// Eposta adresi formatı doğru mu
            else if (_globalSettings.TurkishCharacterControl(entity.UserName)) Check.BadStatus("ServerMessage_userNameTurkishCharactersCanNot");// Kullanıcı adında türkçe karakter olamaz
            else if (entity.UserName != user.UserName && IsUserNameControl(entity.UserName)) Check.BadStatus("ServerMessage_thisUserNameWasUsedByAnotherUser");// Kullanıcı adı daha önce kullanılmış mı
            else if (entity.Email != user.Email && IsEmailControl(entity.Email)) Check.BadStatus("ServerMessage_thisEmailAddressUsedByAnotherUser");// Eposta adresi daha önce kullanılmış mı
            else if (user.LastActiveDate < entity.LastActiveDate) user.LastActiveDate = entity.LastActiveDate;

            #endregion Validations

            user.FullName = entity.FullName;
            user.UserName = entity.UserName.Replace(" ", "");
            user.NormalizedUserName = user.UserName.ToLower();
            user.Email = entity.Email;
            user.NormalizedEmail = user.Email.ToLower();
            user.LastActiveDate = entity.LastActiveDate;
            user.ProfilePicturePath = entity.ProfilePicturePath;
            user.CoverPicturePath = entity.CoverPicturePath;
            base.Update(user);
        }

        /// <summary>
        /// Kullanıcı aktif olma durumu
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullacı aktif ise true pasif ise false döner</returns>
        public bool IsUserStatus(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.IsUserStatus\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _userRepository.IsUserStatus(userName);
        }

        /// <summary>
        /// Parametreden gelen kullanıcı adı başka kullanıcı tarafından kullanıldı mı kontrol eder
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı adı kullanılmış ise true kullanılmamış ise false döner</returns>
        public bool IsUserNameControl(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.IsUserNameControl\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _userRepository.IsUserNameControl(userName);
        }

        /// <summary>
        /// Parametreden gelen eposta adresi başka kullanıcı tarafından kullanıldı mı kontrol eder
        /// </summary>
        /// <param name="email">Email adresi</param>
        /// <returns>Eposta adresi başka kullanıcı tarafından kullanılmış ise true kullanılmamış ise false döner</returns>
        public bool IsEmailControl(string email)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.IsEmailControl\"");
            Check.IsNullOrEmpty(email, nameof(email));

            return _userRepository.IsEmailControl(email);
        }

        /// <summary>
        /// Facebook Id'sinin başka kullanıcı tarafından kullanıldımı kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="facebookId">Facebook Id</param>
        /// <returns>Facebook Id başka kullanıcı tarafından kullanılıyor mu? kullanılıyor ise true kullanılmıyor ise false döner</returns>
        public bool IsFacebookRegister(string userId, string facebookId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.IsFacebookRegister\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(facebookId, nameof(facebookId));

            return _userRepository.IsFacebookRegister(userId, facebookId);
        }

        /// <summary>
        /// Kullanıcı adına göre ve kullanıcı aktif ise kullanıcının ad ve soyadını verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Ad soyad</returns>
        public string UserFullName(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.UserFullName\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _userRepository.UserFullName(userName);
        }

        /// <summary>
        /// Kullanıcı adına göre kullanıcı Id'sini verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı Id</returns>
        public string UserId(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.UserId\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _userRepository.UserId(userName);
        }

        /// <summary>
        /// Kullanıcı adına göre en son giriş yaptığı tarihi verir
        /// </summary>
        /// <param name="userName">Kullanıcı Adı</param>
        /// <returns>Son giriş yapma tarihi</returns>
        public DateTime UserLastActiveDate(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.UserLastActiveDate\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _userRepository.UserLastActiveDate(userName);
        }

        /// <summary>
        /// Facebook Id taşıma işlemi gerçekleştiril..
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="facebookId">Facebook Id</param>
        public void FacebookRegister(string userId, string facebookId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.FacebookRegister\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(facebookId, nameof(facebookId));

            _userRepository.FacebookRegister(userId, facebookId);
        }

        public bool IsUserIdControl(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.IsUserIdControl\"");
            if (String.IsNullOrEmpty(userId))
                return false;

            return _userRepository.IsUserIdControl(userId);
        }

        /// <summary>
        /// Kullanıcı adına ait kullanıcı bilgilerini döndürür
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı</returns>
        public User GetUserByUserName(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.UserManager.GetUserByUserName\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _userRepository.GetUserByUserName(userName);
        }

        /// <summary>
        /// Kullanıcı id'ye ait profil resmi
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Profil resim url</returns>
        public string GetProfilePictureByUserId(string userId)
        {
            Check.IsNullOrEmpty(userId, nameof(userId));
            return _userRepository.GetProfilePictureByUserId(userId);
        }

        /// <summary>
        /// Kullanıcı adına göre profilde kullanılan bilgiler
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>User profileP page model </returns>
        public UserProfilePageModel GetUserProfileInfo(string userName)
        {
            Check.IsNullOrEmpty(userName, nameof(userName));
            return _userRepository.GetUserProfileInfo(userName);
        }

        #endregion Methods
    }
}