using ContestPark.WebApi.Providers;

namespace ContestPark.WebApi.Controllers
{
    [Route("token")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TokenController : Controller
    {
        #region Private Variables

        private UserManager<User> _userManager;
        private IMissionService _missionService;
        private ILanguageService _languageService;
        private IUserService _userService;
        private IOptions<TokenProviderOptions> _options;

        #endregion Private Variables

        #region Constructors

        public TokenController(
             UserManager<User> userManager,
             IOptions<TokenProviderOptions> options,
             IMissionService missionService,
             IUserService userService,
             ILanguageService languageService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _missionService = missionService ?? throw new ArgumentNullException(nameof(missionService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Kullanıcılara token verir
        /// </summary>
        /// <param name="username">Kulanıcı adı</param>
        /// <param name="password">Şifre</param>
        [HttpPost, Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GetToken(string username, string password)
        {
            User user = await _userManager.FindByNameAsync(username.ToLower());
            if (user == null) return BadRequest(new MessageModel("ServerMessage_UsernameOrPasswordIsIncorrect"));

            bool isSuccess = _userManager.CheckPasswordAsync(user, password).Result;
            if (!isSuccess) return BadRequest(new MessageModel("ServerMessage_UsernameOrPasswordIsIncorrect"));

            user.LastActiveDate = DateTime.Now;
            _userService.Update(user);

            _missionService.MissionComplete(user.Id, Missions.Mission13,
                                                     Missions.Mission14,
                                                     Missions.Mission15,
                                                     Missions.Mission16);// Görev yapıldı mı? kontrol ettik
            var token = await GetJwtSecurityToken(user);
            LoggingService.LogInformation("A user has logged in.");
            return Ok(new TokenResponseModel
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo,
                language = _languageService.GetUserLangId(user.Id).ToLanguageCode(),
                userId = user.Id,
                fullName = user.FullName,
                UserCoverPicturePath = user.CoverPicturePath,
                UserProfilePicturePath = user.ProfilePicturePath
            });
        }

        #endregion Services

        #region Helpers

        private async Task<JwtSecurityToken> GetJwtSecurityToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            return new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: GetTokenClaims(user).Union(userClaims),
                expires: user.LastActiveDate.Add(_options.Value.Expiration),
                signingCredentials: _options.Value.SigningCredentials
            );
        }

        private static IEnumerable<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>
                        {
                            new Claim("uid", user.Id),
                            new Claim("fullName", user.FullName),// TODO: ad soyad güncellenirse tokeni de güncellemek lazım client tarafından
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                        };
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
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //    if (_userService != null)
        //    {
        //        _userService.Dispose();
        //        _userService = null;
        //    }
        //    if (_languageService != null)
        //    {
        //        _languageService.Dispose();
        //        _languageService = null;
        //    }
        //    if (_coverPictureService != null)
        //    {
        //        _coverPictureService.Dispose();
        //        _coverPictureService = null;
        //    }
        //    if (_pictureService != null)
        //    {
        //        _pictureService.Dispose();
        //        _pictureService = null;
        //    }
        //}
    }
}