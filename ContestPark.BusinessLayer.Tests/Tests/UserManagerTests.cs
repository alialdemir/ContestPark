using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities;
using ContestPark.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class UserManagerTests : TestBase
    {
        #region Field

        private Mock<IUserRepository> _userRepository;
        private Mock<IGlobalSettings> _globalSettings;
        private Mock<IPostService> _PostService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IEmailSender> _emailSender;
        private Mock<UserManager<User>> _userManager;
        private Mock<RoleManager<IdentityRole>> _roleManager;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _userManager = new Mock<UserManager<User>>();
            _roleManager = new Mock<RoleManager<IdentityRole>>();
            _userRepository = new Mock<IUserRepository>();
            _globalSettings = new Mock<IGlobalSettings>();
            _PostService = new Mock<IPostService>();
            _emailSender = new Mock<IEmailSender>();
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        /// <summary>
        /// Rastgele bir kullanıcı ekleme
        /// </summary>
        /// <returns></returns>
        private async Task<User> InsertUser()
        {
            IUserService service = new UserService(_userManager.Object, _roleManager.Object, _userRepository.Object, _globalSettings.Object, _PostService.Object, _emailSender.Object, _unitOfWork.Object);

            string userName = Guid.NewGuid().ToString().Substring(0, 5) + "test";
            await service.Insert(new UserRegisterModel
            {
                UserName = userName,
                Email = Guid.NewGuid().ToString().Substring(0, 5) + "1test@gmail.com",
                FullName = "Ali Aldemir",
                Password = "19931993",
                ConfirmPassword = "19931993",
                LanguageCode = "tr-TR"
            });

            return null;
        }

        #endregion Test settings method

        //#region Constructors methods tests
        /// <summary>
        /// Eğer RoleManager null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: roleManager")]
        //public void When_RoleManager_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    IUserService service = new UserManager(
        //           _userManager.Object,
        //           null,
        //           _userRepository.Object,
        //           _globalSettings.Object,
        //           _pictureService.Object,
        //           _coverPictureService.Object,
        //           _PostService.Object,
        //           _unitOfWork.Object);
        //   }
        ///// <summary>
        ///// Eğer UserManager null gelirse ArgumentNullException fırlatması lazım
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: userManager")]
        //public void When_UserManager_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    _userService = new UserManager(
        //        null,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        _globalSettings.Object,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //}
        ///// <summary>
        ///// Eğer GlobalSettingsManager null gelirse ArgumentNullException fırlatması lazım
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: globalSettings")]
        //public void When_GlobalSettingsManager_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    _userService = new UserManager(
        //        DatabaseSetupTests.UserManager,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        null,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //}
        //#endregion
        //#region Insert method tests
        ///// <summary>
        ///// Tüm değerler doğru ise üye kayıt ekleme durmumu
        ///// </summary>
        //[TestMethod]
        //public async Task Given_CreateUserRegisterModel_Expect_Registration()
        //{
        //    // Arrange
        //    _userService = new UserManager(
        //        DatabaseSetupTests.UserManager,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        _globalSettings.Object,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //    string userName = Guid.NewGuid().ToString().Substring(0, 5) + "test";
        //    await _userService.Insert(new UserRegisterModel
        //    {
        //        UserName = userName,
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        FullName = "Ali Aldemir",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    });
        //    // Act
        //    bool userExists = DatabaseSetupTests.Context.Users.Where(p => p.UserName == userName).Any();
        //    // Assert
        //    Assert.IsTrue(userExists);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken UserRegisterModel null giderse beklenen hata mesajı: signupPage_requiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "signupPage_requiredFields")]
        //public async Task Insert_Given_RegisterModel_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = null;
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken UserName null giderse beklenen hata mesajı: ServerMessage_userNameRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameRequiredFields")]
        //public async Task Insert_Given_UserName_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        FullName = "Ali Aldemir",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken FullName null giderse beklenen hata mesajı: ServerMessage_fullNameRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_fullNameRequiredFields")]
        //public async Task Insert_Given_FullName_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Email null giderse beklenen hata mesajı: ServerMessage_emailRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_emailRequiredFields")]
        //public async Task Insert_Given_Email_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Password null giderse beklenen hata mesajı: ServerMessage_passwordRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_passwordRequiredFields")]
        //public async Task Insert_Given_Password_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        //      Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Password 8 karakterden az ise beklenen hata mesajı: ServerMessage_passwordMinLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_passwordMinLength")]
        //public async Task Insert_Given_Password_When_Length_3_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "xa",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Password 32 karakterden fazla ise beklenen hata mesajı: ServerMessage_passwordMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_passwordMaxLength")]
        //public async Task Insert_Given_Password_When_Length_33_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "1234567890123456789012345678901234",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Password ile ConfirmPassword uyuşmuyor ise beklenen hata mesajı: ServerMessage_notEqualsConfirmPassword
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_notEqualsConfirmPassword")]
        //public async Task Insert_Given_Password_ConfirmPassword_When_Not_Equals_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "12345678",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken UserName 3 karakterden küçük ise beklenen hata mesajı: ServerMessage_userNameMinLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameMinLength")]
        //public async Task Insert_Given_UserName_When_Length_3_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "st",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken UserName 300 karakterden küçük ise beklenen hata mesajı: ServerMessage_userNameMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameMaxLength")]
        //public async Task Insert_Given_UserName_When_Length_300_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        //        FullName = Guid.NewGuid().ToString().Substring(0, 5),
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken FullName 3 karakterden küçük ise beklenen hata mesajı: ServerMessage_fullNameMinLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_fullNameMinLength")]
        //public async Task Insert_Given_FullName_When_Length_3_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "ssst",
        //        FullName = "13",
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken FullName 300 karakterden küçük ise beklenen hata mesajı: ServerMessage_fullNameMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_fullNameMaxLength")]
        //public async Task Insert_Given_FullName_When_Length_300_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "xsafsa",
        //        FullName = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Email 300 karakterden küçük ise beklenen hata mesajı: ServerMessage_emailMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_emailMaxLength")]
        //public async Task Insert_Given_Email_When_Length_300_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "xsafsa",
        //        FullName = "sda",
        //        Email = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890test@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken Email formatı yanlış ise beklenen hata mesajı: ServerMessage_emailFormating
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_emailFormating")]
        //public async Task Insert_Given_Email_When_Wrong_Format_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "xsafsa",
        //        FullName = "sda",
        //        Email = "dfagmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken UserName türkçe karaakter var ise beklenen hata mesajı: ServerMessage_userNameTurkishCharactersCanNot
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameTurkishCharactersCanNot")]
        //public async Task Insert_Given_UserName_When_Turkish_Character_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = "xsaÜçişüfsa",
        //        FullName = "deneme",
        //        Email = "dfa@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel);
        //}
        ///// <summary>
        ///// Kullanıcı eklerken UserName daha önceden var ise beklenen hata mesajı: ServerMessage_thisUserNameWasUsedByAnotherUser
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_thisUserNameWasUsedByAnotherUser")]
        //public async Task Insert_Given_UserName_When_Previously_Used_UserName_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel1 = new UserRegisterModel
        //    {
        //        UserName = "deneme",
        //        FullName = "deneme",
        //        Email = "dfa@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    UserRegisterModel userRegisterModel2 = new UserRegisterModel
        //    {
        //        UserName = "deneme",
        //        FullName = "deneme",
        //        Email = "deneme@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel1);
        //    await _userService.Insert(userRegisterModel2);
        //}

        ///// <summary>
        ///// Kullanıcı eklerken Email daha önceden var ise beklenen hata mesajı: ServerMessage_thisEmailAddressUsedByAnotherUser
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_thisEmailAddressUsedByAnotherUser")]
        //public async Task Insert_Given_Email_When_Previously_Used_Email_Then_CustomException()
        //{
        //    // Arrange
        //    UserRegisterModel userRegisterModel1 = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(5),
        //        FullName = "deneme",
        //        Email = "deneme@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    UserRegisterModel userRegisterModel2 = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(5),
        //        FullName = "deneme",
        //        Email = "deneme@gmail.com",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    // Act
        //    await _userService.Insert(userRegisterModel1);
        //    await _userService.Insert(userRegisterModel2);
        //}
        //#endregion
        //#region FacebookLogin methods tests
        ///// <summary>
        ///// Eğer parametreden giden facebookId boş gelirse ArgumentNullException fırlatması lazım
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: facebookId")]
        //public void FacebookLogin_When_facebookId_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    _userService.FacebookLogin(String.Empty);
        //}
        ///// <summary>
        ///// Kayıtlı olmayan bir facebbok id gönderilirse null kullanıcı modeli dönmesi lazım
        ///// </summary>
        //[TestMethod]
        //public void FacebookLogin_When_facebookId_Not_Exists_Expect_Null_UserEntity()
        //{
        //    // Arrange
        //    string facebookId = Guid.NewGuid().ToString();
        //    // Act
        //    User userEntity = _userService.FacebookLogin(facebookId);
        //    // Assert
        //    Assert.IsNull(userEntity);
        //}
        ///// <summary>
        ///// İlk önce bir tane kullanıcı eklendi daha sonra eklenen kullanıcıya facebook id ekledim
        ///// daha sonra da FacebookLogin methoduna o facebook id'yi gönderince aynı kullanıcı gelmesi gerekiyor
        ///// </summary>
        //[TestMethod]
        //public async Task FacebookLogin_When_facebookId_True_Expect_UserEntity()
        //{
        //    // Arrange
        //    string userName = Guid.NewGuid().ToString().Substring(0, 5) + "test";
        //    UserRegisterModel userRegisterModel = new UserRegisterModel
        //    {
        //        UserName = userName,
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        FullName = "Ali Aldemir",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    await _userService.Insert(userRegisterModel);

        //    User user = DatabaseSetupTests
        //        .Context
        //        .Users
        //        .Where(p => p.UserName == userName)
        //        .FirstOrDefault();
        //    user.FaceBookId = Guid.NewGuid().ToString();
        //    DatabaseSetupTests.Context.SaveChanges();

        //    // Act
        //    User userEntity = _userService.FacebookLogin(user.FaceBookId);
        //    // Assert
        //    Assert.AreEqual(user.Id, userEntity.Id);
        //}
        //#endregion
        //#region DoUserProfilePicture method tests
        ///// <summary>
        ///// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: userId")]
        //public void DoUserProfilePicture_When_userId_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    _userService.DoUserProfilePicture(String.Empty, "DENEME", Stream.Null);
        //}
        ///// <summary>
        ///// Eğer contentType null gelirse ArgumentNullException fırlatması lazım
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: contentType")]
        //public void DoUserProfilePicture_When_contentType_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    _userService.DoUserProfilePicture("Deneme", String.Empty, Stream.Null);
        //}
        ///// <summary>
        ///// Eğer streamImage null gelirse ArgumentNullException fırlatması lazım
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: streamImage")]
        //public void DoUserProfilePicture_When_streamImage_Null_Expect_ArgumentNullException()
        //{
        //    // Act
        //    _userService.DoUserProfilePicture("Deneme", "deneme", Stream.Null);
        //}
        ///// <summary>
        ///// Eğer resim formatı yanlış girilirse beklenen hata mesajı: serverMessages_pictureFormat
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "serverMessages_pictureFormat")]
        //public async void DoUserProfilePicture_When_Wrong_contentType_Expect_ArgumentNullException()
        //{
        //    // Arrange
        //    byte[] picture = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==");
        //    var _stream = new MemoryStream(picture);
        //    var user = new UserRegisterModel
        //    {
        //        UserName = Guid.NewGuid().ToString().Substring(0, 5) + "test",
        //        Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //        FullName = "Ali Aldemir",
        //        Password = "19931993",
        //        ConfirmPassword = "19931993",
        //        LanguageCode = "tr-TR"
        //    };
        //    await _userService.Insert(user);
        //    string userId = _userService.UserId(user.UserName);
        //    // Act
        //    _userService.DoUserProfilePicture(userId, "yanlış resim formatı", _stream);
        //}
        ///// <summary>
        ///// Kullanıcı id sistemde olmayan id ise beklenen hata mesajı: No such id:userId
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "No such id:userId")]
        //public void DoUserProfilePicture_When_Wrong_UserId_Expect_CustomException()
        //{
        //    // Arrange
        //    byte[] picture = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==");
        //    var pictureStream = new MemoryStream(picture);

        //    string wrongUserId = Guid.NewGuid().ToString().Substring(0, 7);

        //    // Act
        //    _userService.DoUserProfilePicture(wrongUserId, "image/jpg", pictureStream);
        //}
        ///// <summary>
        ///// Kullanıcı id sistemde olmayan id ise
        ///// </summary>
        //[TestMethod]
        //public void DoUserProfilePicture_PictureUpload_Success()
        //{
        //    try
        //    {
        //        // Arrange
        //        byte[] picture = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==");
        //        var pictureStream = new MemoryStream(picture);

        //        string wrongUserId = DatabaseSetupTests.Context.Users.Select(p => p.Id).First();

        //        // Act
        //        string picturePath = _userService.DoUserProfilePicture(wrongUserId, "image/jpg", pictureStream);
        //        // Assert
        //        Assert.IsNotNull(picturePath, picturePath);

        //    }
        //    catch (Exception ex)
        //    {
        //        //Eklenen resmi ftp üzerinden sildim
        //        var moq = new Moq.Mock<PictureManager>();
        //        moq.Object.Delete(EfPictureDal.PictureId);
        //        Assert.Fail(
        //             string.Format("Unexpected exception of type {0} caught: {1}",
        //                            ex.GetType(), ex.Message));
        //    }
        //}
        ///// <summary>
        ///// User id boş giderse ArgumentException fırlatmalı
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentException), "Value cannot be null.\r\nParameter name: userId")]
        //public void DoUserProfilePicture_UserId_Null_CustomExceptiton()
        //{
        //    // Arrange
        //    string nullUserId = String.Empty;
        //    // Act
        //    _userService.DoUserProfilePicture(nullUserId, 1);
        //}
        ///// <summary>
        ///// Picture id 0 giderse ArgumentException fırlatmalı
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void DoUserProfilePicture_PictureId_0_CustomExceptiton()
        //{
        //    // Arrange
        //    _userService = new UserManager(
        //        DatabaseSetupTests.UserManager,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        _globalSettings.Object,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //    string nullUserId = "deneme";
        //    // Act
        //    _userService.DoUserProfilePicture(nullUserId, 0);
        //}
        ///// <summary>
        ///// Picture id -1 giderse ArgumentException fırlatmalı
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void DoUserProfilePicture_PictureId_Negative_CustomExceptiton()
        //{
        //    // Arrange
        //    _userService = new UserManager(
        //        DatabaseSetupTests.UserManager,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        _globalSettings.Object,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //    string nullUserId = "deneme";
        //    // Act
        //    _userService.DoUserProfilePicture(nullUserId, -1);
        //}
        ///// <summary>
        ///// UserId den gelen kullanıcıya ait olmayan bir resim profil resmi yapılmak istenilirse
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_thisPictureIsNotYours")]
        //public void DoUserProfilePicture_PictureId_Not_Equals_User_PictureId_CustomExceptiton()
        //{
        //    // Arrange
        //    _pictureService.Setup(x => x.IsUserPictureIdControl(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
        //    _userService = new UserManager(
        //        DatabaseSetupTests.UserManager,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        _globalSettings.Object,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //    string nullUserId = "deneme";
        //    // Act
        //    _userService.DoUserProfilePicture(nullUserId, 21);
        //}

        ///// <summary>
        ///// Resim id ile kullanıcıya profil resmi ekleme eğer eklerse profilePicture ile picturePath aynı olmalı
        ///// </summary>
        //[TestMethod]
        //public async Task DoUserProfilePicture_PictureId_Equals_User_PictureId_Success()
        //{
        //    // Arrange

        //    _pictureService.Setup(x => x.IsUserPictureIdControl(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
        //    _pictureService.Setup(x => x.GetProfilePictureByUserId(It.IsAny<string>())).Returns("Resimler/user1.jpg");
        //    _userRepository.Setup(x => x.GetById(It.IsAny<object>())).Returns(new User { Id = "1" });
        //    _userRepository.Setup(x => x.Update(It.IsAny<User>()));
        //    _PostService.Setup(p => p.Insert(It.IsAny<Post>()));

        //    _userService = new UserManager(
        //        DatabaseSetupTests.UserManager,
        //        DatabaseSetupTests.RoleManager,
        //        _userRepository.Object,
        //        _globalSettings.Object,
        //        _pictureService.Object,
        //        _coverPictureService.Object,
        //        _PostService.Object,
        //        _unitOfWork.Object);
        //    //string userName = Guid.NewGuid().ToString().Substring(0, 5) + "test";
        //    //await _userService.Insert(new UserRegisterModel
        //    //{
        //    //    UserName = userName,
        //    //    Email = Guid.NewGuid().ToString().Substring(0, 5) + "test@gmail.com",
        //    //    FullName = "Ali Aldemir",
        //    //    Password = "19931993",
        //    //    ConfirmPassword = "19931993",
        //    //    LanguageCode = "tr-TR"
        //    //});
        //    //string userId = _userService.UserId(userName);
        //    //Picture picture = new Picture { UserId = userId, PictureTypeId = (int)PictureTypes.CoverPictures, PicturePath = Guid.NewGuid().ToString().Substring(0, 15) };

        //    //var moq = new Moq.Mock<PictureManager>();
        //    //var pictureManager = moq.Object;
        //    //pictureManager.Insert(picture);

        //    // Act
        //    string picturePath = _userService.DoUserProfilePicture("1", 1);
        //    //string profilePicture = pictureManager.GetProfilePictureByUserId(userId);
        //    //if (picturePath.Equals(DefaultImage.Defaultimage) || profilePicture.Equals(DefaultImage.Defaultimage))
        //    //    Assert.Fail("Resim kayıt edilemedi");
        //    // Assert
        //    Assert.AreEqual(picturePath, "Resimler/user1.jpg");
        //}
        //#endregion
        //#region Update tests
        ///// <summary>
        ///// Tüm değerler doğru ise üye güncelleme durmumu
        ///// </summary>
        //[TestMethod]
        //public async Task Update_FullName_And_UserName_And_Email()
        //{
        //    // Arrange
        //    User updateUser = await InsertUser();

        //    updateUser.FullName = Guid.NewGuid().ToString().Substring(0, 5);
        //    updateUser.UserName = Guid.NewGuid().ToString().Substring(0, 5);
        //    updateUser.Email = Guid.NewGuid().ToString().Substring(0, 5) + "@deneme.com";

        //    _userService.Update(updateUser);
        //    // Act
        //    User afterUpdate = DatabaseSetupTests.Context.Users.Where(p => p.UserName == updateUser.UserName).First();
        //    // Assert
        //    Assert.AreEqual(updateUser.FullName, afterUpdate.FullName);
        //    Assert.AreEqual(updateUser.UserName, afterUpdate.UserName);
        //    Assert.AreEqual(updateUser.Email, afterUpdate.Email);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken User null giderse beklenen hata mesajı: signupPage_requiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "signupPage_requiredFields")]
        //public void Update_Given_User_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    User userUpdate = null;
        //    // Act
        //    _userService.Update(userUpdate);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken User id yanlış giderse beklenen hata mesajı: ArgumentException
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: user")]
        //public void Update_When_Wrong_UserId_Then_CustomException()
        //{
        //    // Arrange
        //    User userUpdate = new User { Id = "yanlış user id" };
        //    // Act
        //    _userService.Update(userUpdate);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken UserName null giderse beklenen hata mesajı: ServerMessage_userNameRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameRequiredFields")]
        //public async void Update_Given_UserName_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.UserName = String.Empty;
        //    // Act
        //    _userService.Update(user);
        //}

        ///// <summary>
        ///// Kullanıcı güncellerken FullName null giderse beklenen hata mesajı: ServerMessage_fullNameRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_fullNameRequiredFields")]
        //public async Task Update_Given_FullName_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.FullName = String.Empty;
        //    // Act
        //    _userService.Update(user);
        //}

        ///////
        ///// <summary>
        ///// Kullanıcı güncellerken Email null giderse beklenen hata mesajı: ServerMessage_emailRequiredFields
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_emailRequiredFields")]
        //public async Task Update_Given_Email_When_Null_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.Email = String.Empty;
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken UserName 3 karakterden küçük ise beklenen hata mesajı: ServerMessage_userNameMinLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameMinLength")]
        //public async Task Update_Given_UserName_When_Length_3_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.UserName = "st";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken UserName 300 karakterden küçük ise beklenen hata mesajı: ServerMessage_userNameMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameMaxLength")]
        //public async Task Update_Given_UserName_When_Length_300_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.UserName = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken FullName 3 karakterden küçük ise beklenen hata mesajı: ServerMessage_fullNameMinLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_fullNameMinLength")]
        //public async Task Update_Given_FullName_When_Length_3_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.FullName = "ac";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken FullName 300 karakterden küçük ise beklenen hata mesajı: ServerMessage_fullNameMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_fullNameMaxLength")]
        //public async Task Update_Given_FullName_When_Length_300_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.FullName = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken Email 300 karakterden küçük ise beklenen hata mesajı: ServerMessage_emailMaxLength
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_emailMaxLength")]
        //public async Task Update_Given_Email_When_Length_300_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.Email = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890test";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken Email formatı yanlış ise beklenen hata mesajı: ServerMessage_emailFormating
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_emailFormating")]
        //public async Task Update_Given_Email_When_Wrong_Format_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.Email = "abc.com";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken UserName türkçe karaakter var ise beklenen hata mesajı: ServerMessage_userNameTurkishCharactersCanNot
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_userNameTurkishCharactersCanNot")]
        //public async Task Update_Given_UserName_When_Turkish_Character_Then_CustomException()
        //{
        //    // Arrange
        //    User user = await InsertUser();
        //    user.UserName = "çişğüılxcçÇŞİĞJ";
        //    // Act
        //    _userService.Update(user);
        //}
        ///// <summary>
        ///// Kullanıcı güncellerken UserName daha önceden var ise beklenen hata mesajı: ServerMessage_thisUserNameWasUsedByAnotherUser
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_thisUserNameWasUsedByAnotherUser")]
        //public async Task Update_Given_UserName_When_Previously_Used_UserName_Then_CustomException()
        //{
        //    // Arrange
        //    User user1 = await InsertUser();
        //    User user2 = await InsertUser();

        //    user1.UserName = user2.UserName;
        //    // Act
        //    _userService.Update(user1);
        //}

        ///// <summary>
        ///// Kullanıcı güncellerken Email daha önceden var ise beklenen hata mesajı: ServerMessage_thisEmailAddressUsedByAnotherUser
        ///// </summary>
        //[TestMethod]
        //[ExpectedExceptionCustom(typeof(NotificationException), "ServerMessage_thisEmailAddressUsedByAnotherUser")]
        //public async Task Update_Given_Email_When_Previously_Used_Email_Then_CustomException()
        //{
        //    // Arrange
        //    User user1 = await InsertUser();
        //    User user2 = await InsertUser();

        //    user1.Email = user2.Email;
        //    // Act
        //    _userService.Update(user1);
        //}
        //#endregion
        //#region IsUserIdControl methods tests
        ///// <summary>
        ///// Kayıtlı olmayan bir user id gönderilirse false dönmesi lazım
        ///// </summary>
        //[TestMethod]
        //public void IsUserIdControl_When_UserId_Not_Exists_Expect_Null_UserEntity()
        //{
        //    // Arrange
        //    string userId = "sample-user-id";
        //    // Act
        //    bool isUserIdControl = _userService.IsUserIdControl(userId);
        //    // Assert
        //    Assert.IsFalse(isUserIdControl);
        //}
        ///// <summary>
        ///// Kayıtlı olmayan bir user id gönderilirse false dönmesi lazım
        ///// </summary>
        //[TestMethod]
        //public void IsUserIdControl_When_UserId_Null_Expect_Null_UserEntity()
        //{
        //    // Arrange
        //    string userId = String.Empty;
        //    // Act
        //    bool isUserIdControl = _userService.IsUserIdControl(userId);
        //    // Assert
        //    Assert.IsFalse(isUserIdControl);
        //}
        //#endregion
    }
}