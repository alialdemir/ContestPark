namespace ContestPark.WebApi.Controllers
{
    public class FollowsController : BaseController
    {
        #region Private Variables

        private IFollowService _followService;
        private IMissionService _missionService;

        #endregion Private Variables

        #region Constructors

        public FollowsController(IFollowService followService, IMissionService missionService)
        {
            _followService = followService;
            _missionService = missionService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Login olan kullanıcının takip ettiği kullanıcı listesi
        /// </summary>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>kullanıcı listesi</returns>
        [HttpGet]
        public IActionResult FollowingChatList([FromQuery]string search, [FromQuery]PagingModel pageModel)
        {
            return Ok(_followService.FollowingChatList(UserId, search, pageModel));
        }

        /// <summary>
        /// kullanıcının takip ettiği kişiler
        /// </summary>
        /// <param name="followedUserId">Takip edip eden kullanıcı Id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takipçi listesi</returns>
        [HttpGet]
        [Route("{followedUserId}/Following")]
        public IActionResult Following(string followedUserId, [FromQuery]PagingModel pageModel)
        {
            return Ok(_followService.Following(followedUserId, UserId, pageModel));
        }

        /// <summary>
        /// kullanıcının Takip edenler(Takipçileri)
        /// </summary>
        /// <param name="followedUserId">Takip edip eden kullanıcı Id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takipçi listesi</returns>
        [HttpGet]
        [Route("{followedUserId}/Followers")]
        public IActionResult Followers(string followedUserId, [FromQuery]PagingModel pageModel)
        {
            return Ok(_followService.Followers(followedUserId, UserId, pageModel));
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takipçi sayısı
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        /// <returns>takipçi sayısı</returns>
        [HttpPost]
        [Route("FollowersCount")]
        public IActionResult FollowersCount([FromBody]string followedUserId)
        {
            return Ok(_followService.FollowersCount(followedUserId));
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takip ettiklerinin sayısı
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        /// <returns>takipçi sayısı</returns>
        [HttpPost]
        [Route("FollowUpCount")]
        public IActionResult FollowUpCount([FromBody]string followUpUserId)
        {
            return Ok(_followService.FollowUpCount(followUpUserId));
        }

        /// <summary>
        /// Login olan kullanıcının parametredene gelen kullanıcıyı takip etme durumu
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        /// <returns>Takip ediyorsa true etmiyor ise false</returns>
        [HttpPost]
        [Route("IsFollowUpStatus")]
        public IActionResult IsFollowUpStatus([FromBody]string followedUserId)
        {
            return Ok(_followService.IsFollowUpStatus(UserId, followedUserId));
        }

        /// <summary>
        /// Takibi BIRAK
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        [HttpDelete]
        public IActionResult UnFollow([FromBody]string followedUserId)
        {
            _followService.Delete(UserId, followedUserId);
            return Ok();
        }

        /// <summary>
        /// Takip et
        /// </summary>
        /// <param name="followedUserId">kullanıcı Id</param>
        [HttpPost]
        public IActionResult FollowUp([FromBody]string followedUserId)
        {
            string followUpUserId = UserId;
            _followService.Insert(new Follow
            {
                FollowedUserId = followedUserId,
                FollowUpUserId = followUpUserId
            });

            _missionService.MissionComplete(followUpUserId,
                                                           Missions.Mission24,
                                                           Missions.Mission25,
                                                           Missions.Mission26);// Görev yapıldı mı? kontrol ettik
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_followService != null)
        //    {
        //        _followService.Dispose();
        //        _followService = null;
        //    }
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //}
    }
}