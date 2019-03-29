namespace ContestPark.WebApi.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        #region Properties

        /// <summary>
        /// Current user language
        /// </summary>
        //public Languages UserLanguages
        //{
        //    get
        //    {
        //        if (Request.Headers["Accept-Language"].Count == 0)
        //            return Languages.English;

        //        string langCode = Request.Headers["Accept-Language"].ToString();
        //        if (langCode == "tr-TR") return Languages.Turkish;

        //        return Languages.English;
        //    }
        //}
        private string userId = String.Empty;

        /// <summary>
        /// Current user id
        /// </summary>
        public string UserId
        {
            get
            {
                if (String.IsNullOrEmpty(userId)) userId = User.FindFirst("uid").Value;
                return userId;
            }
        }

        private string userFullName;

        /// <summary>
        /// Current user full name
        /// </summary>
        public string UserFullName
        {
            get
            {
                if (String.IsNullOrEmpty(userFullName)) userFullName = User.FindFirst("fullName").Value;
                return userFullName;
            }
        }

        #endregion Properties
    }
}