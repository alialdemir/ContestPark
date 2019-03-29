namespace ContestPark.WebApi.Controllers
{
    public class ScoresController : BaseController
    {
        #region Private Variables

        private IScoreService _scoreService;

        #endregion Private Variables

        #region Constructors

        public ScoresController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Alt kategori Id'ye göre sýralama getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="paging">Sayfalama 10 ve katlari olmali</param>
        /// <returns>Sýralama listesi</returns>
        [HttpGet]
        [Route("{subCategoryId}")]
        public IActionResult ScoreRanking(int subCategoryId, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_scoreService.ScoreRanking(subCategoryId, pagingModel));
        }

        /// <summary>
        /// Alt kategori Id'ye göre login olan kullanicinin 2 alt ve 2 üst sýrasýndaki kullanicilarýn sýralama olarak döndürür
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Sýralama listesi</returns>
        [HttpGet]
        [Route("{subCategoryId}/DuelResultRanking")]
        public IActionResult DuelResultRanking(int subCategoryId)
        {
            return Ok(_scoreService.DuelResultRanking(UserId, subCategoryId));
        }

        /// <summary>
        /// kullanicinin facebook arkadaþ listesindeki kullanici bizde ekli ise o kategorisindeki puan ve sýrasýný döndürür
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="facebookFriendRanking">Facebook arkadaþ listesi</param>
        /// <returns>Sýralama listesi</returns>
        [HttpPost]
        [Route("{subCategoryId}/FacebookFriendRanking")]
        public IActionResult FacebookFriendRanking(int subCategoryId, [FromBody]string facebookFriendRanking)
        {
            List<FacebookFriendRankingModel> result = new List<FacebookFriendRankingModel>();
            string[] fbUser = facebookFriendRanking.Split(']');
            result = fbUser.Where(p => p != "").Select(p => new FacebookFriendRankingModel { FacebokId = p }).ToList();
            return Ok(_scoreService.FacebookFriendRanking(UserId, subCategoryId, result));
        }

        /// <summary>
        /// Login olan kullanicinin o kategorideki puanýný döndürür
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Puan</returns>
        [HttpGet]
        [Route("{subCategoryId}/UserTotalScore")]
        public IActionResult UserTotalScore(int subCategoryId)
        {
            return Ok(_scoreService.UserTotalScore(UserId, subCategoryId));
        }

        /// <summary>
        /// kullanicinin takip ettigi arkadaþlarýnýn sýralamadaki durumunu verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="paging">Sayfalama 10 ve katlari olmali</param>
        /// <returns>Takip ettiklerinin sýralama listesi</returns>
        [HttpGet]
        [Route("{subCategoryId}/ScoreRankingFollowing")]
        public IActionResult ScoreRankingFollowing(int subCategoryId, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_scoreService.ScoreRankingFollowing(UserId, subCategoryId, pagingModel));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_scoreService != null)
        //    {
        //        _scoreService.Dispose();
        //        _scoreService = null;
        //    }
        //}
    }
}