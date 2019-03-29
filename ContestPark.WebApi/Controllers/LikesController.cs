namespace ContestPark.WebApi.Controllers
{
    public class LikesController : BaseController
    {
        #region Private Variables

        private ILikeService _likeService;
        private IMissionService _missionService;

        #endregion Private Variables

        #region Constructors

        public LikesController(ILikeService likeService, IMissionService missionService)
        {
            _likeService = likeService;
            _missionService = missionService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Parametreden gelen kim ne yapıyor Id'sini begenen kullanicilar
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <param name="paging">Sayfalama 10 ve katlari olmali</param>
        /// <returns>begenen kullanicilar</returns>
        [HttpGet]
        [Route("{PostId}")]
        public IActionResult Likes(int PostId, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_likeService.Likes(UserId, PostId, pagingModel));
        }

        /// <summary>
        /// Beðen
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        [HttpPost]
        public IActionResult Like([FromBody]int PostId)
        {
            string userId = UserId;
            _likeService.Insert(new Like
            {
                UserId = userId,
                PostId = PostId,
            });

            _missionService.MissionComplete(userId,
                                                Missions.Mission32,
                                                Missions.Mission33,
                                                Missions.Mission34);// Görev yapildi mi? kontrol ettik

            return Ok();
        }

        /// <summary>
        /// Beðenmekten vazgeç
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        [HttpDelete]
        public IActionResult DisLike([FromBody]int PostId)
        {
            _likeService.DisLike(UserId, PostId);
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_likeService != null)
        //    {
        //        _likeService.Dispose();
        //        _likeService = null;
        //    }
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //}
    }
}