namespace ContestPark.WebApi.Controllers
{
    public class PostsController : BaseController
    {
        #region Private Variables

        private IPostService _PostService;

        #endregion Private Variables

        #region Constructors

        public PostsController(IPostService PostService)
        {
            _PostService = PostService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Parametreden gelen kim ne yapiyor postu döndürür
        /// </summary>
        /// <param name="PostId">Kim ne yapiyor Id</param>
        /// <returns>kim ne yapiyor postu</returns>
        [HttpGet]
        [Route("{PostId}")]
        public IActionResult Post(int PostId)
        {
            return Ok(_PostService.Post(UserId, PostId));
        }

        /// <summary>
        /// Parametreden gelen kullanici adýna göre kim ne yapiyor listesi
        /// </summary>
        /// <param name="userName">kullanici adý</param>
        /// <param name="paging">Sayfalama 4 ve katlari olmali</param>
        /// <returns>Kim ne yapiyor listesi</returns>
        [HttpGet]
        public IActionResult PostList([FromQuery]string userName, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_PostService.PostList(UserId, userName, pagingModel));
        }

        /// <summary>
        /// Alt kategori id'ye göre kim ne yapiyor listesi getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="paging">Sayfalama 4 ve katlari olmali</param>
        /// <returns>Kim ne yapiyor listesi</returns>
        [HttpGet]
        [Route("{subCategoryId}/ContestEnterScreen")]
        public IActionResult ContestEnterScreen(int subCategoryId, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_PostService.ContestEnterScreen(UserId, subCategoryId, pagingModel));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_PostService != null)
        //    {
        //        _PostService.Dispose();
        //        _PostService = null;
        //    }
        //}
    }
}