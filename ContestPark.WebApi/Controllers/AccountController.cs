namespace ContestPark.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        #region Private Variables

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IGlobalSettings _globalSettings;
        private readonly IMissionService _missionService;

        #endregion Private Variables

        #region Constructors

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
        //    IOptions<IdentityCookieOptions> identityCookieOptions,
            IUserService userService,
            IGlobalSettings globalSettings,
           IMissionService missionService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //////////////////          _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _userService = userService;
            _globalSettings = globalSettings;
            _missionService = missionService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Kullanıcı bilgilerini günceller
        /// </summary>
        /// <param name="updateUserInfo">Kullanıcı bilgileri</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetUpdateUserInfo")]
        public async Task<IActionResult> SetUpdateUserInfo([FromBody]UpdateUserInfoModel updateUserInfo)
        {
            if (updateUserInfo == null) BadRequest("signupPage.requiredFields");

            if (_globalSettings.TurkishCharacterControl(updateUserInfo.UserName))
                BadRequest("serverMessages_userNameTurkishCharactersCanNot");
            string userId = UserId;

            if (!String.IsNullOrEmpty(updateUserInfo.NewPassword))
            {
                if (updateUserInfo.NewPassword.Length >= 8 && updateUserInfo.OldPassword.Length >= 8)
                {
                    User user = _userService.Find(x => x.Id == userId).FirstOrDefault();
                    if (user == null) NoContent();

                    IdentityResult result = await _userManager.ChangePasswordAsync(user, updateUserInfo.OldPassword, updateUserInfo.NewPassword);
                    if (!result.Succeeded) BadRequest(result.Errors.FirstOrDefault());
                }
                else BadRequest("serverMessages_oldPasswordAndNewPasswordMinLength");
            }
            _userService
                 .Update(new User
                 {
                     Id = userId,
                     Email = updateUserInfo.Email,
                     FullName = updateUserInfo.FullName,
                     UserName = updateUserInfo.UserName
                 });
            return Ok();
        }

        /// <summary>
        /// Login olan kullanıcı bilgileri
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUpdateUserInfo")]
        public IActionResult GetUpdateUserInfo()
        {
            return Ok(_userService.GetUpdateUserInfo(UserId));
        }

        /// <summary>
        /// Token süresi hala geçirlimi kontrol etmek kullanılır
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TokenControl")]
        public IActionResult TokenControl()
        {
            return Ok();
        }

        ////////////[HttpPost]
        ////////////[Route("DoUserProfilePicture")]
        ////////////public IActionResult DoUserProfilePicture(IFormFile file)
        ////////////{
        ////////////    if (file == null) throw new NotificationException("Not found image file");
        ////////////    if (file.Length > 0)
        ////////////    {
        ////////////        string userId = UserId;
        ////////////        Stream streamImage = file.OpenReadStream();
        ////////////        string contentType = file.ContentType;
        ////////////        string result = _userService.DoUserProfilePicture(userId, contentType, streamImage);
        ////////////        if (!string.IsNullOrEmpty(result)) _missionService.MissionComplete(userId, Missions.Mission27);// Görev yapildi mi? kontrol ettik
        ////////////        return Ok(result);
        ////////////    }
        ////////////    return BadRequest("File length is too small");
        ////////////}
        /// <summary>
        /// Login olan kullanıcının profil bilgilerini getirir
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UserProfile")]
        public IActionResult UserProfile(string userName)//kullanicinin profilindeki  gerekli datalarý buradan gönderin
        {
            if (String.IsNullOrEmpty(userName)) throw new NotificationException("serverMessages_contestantFound");
            return Ok(_userService.GetUserProfileInfo(userName));
        }

        /// <summary>
        /// Üye ol
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            await _userService.Insert(model);
            return Ok();
        }

        /// <summary>
        /// Çıkış yap
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Şifremi unuttum
        /// </summary>
        /// <param name="userNameOrEmailAdress">Kullanıcı adı veya eposta adresi</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotYourPassword")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(MessageModel))]
        public async Task<IActionResult> ForgotYourPassword([FromQuery]string userInfo)
        {
            if (String.IsNullOrEmpty(userInfo))
                return BadRequest(ContestParkResources.ForgetYourPasswordLabel1);

            if (!await _userService.ForgotYourPasswordAsync(userInfo))
                return BadRequest(ContestParkResources.ServerMessage_sendMailErrorMessage);
            return Ok();
        }

        #endregion Services

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Ok();
            }
        }

        #endregion Helpers

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_userManager != null)
        //    {
        //        _userManager.Dispose();
        //        _userManager = null;
        //    }
        //    if (_picture != null)
        //    {
        //        _picture.Dispose();
        //        _picture = null;
        //    }
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //    if (_coverPicture != null)
        //    {
        //        _coverPicture.Dispose();
        //        _coverPicture = null;
        //    }
        //    if (_userService != null)
        //    {
        //        _userService.Dispose();
        //        _userService = null;
        //    }
        //}
    }
}