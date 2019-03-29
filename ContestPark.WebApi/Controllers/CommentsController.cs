namespace ContestPark.WebApi.Controllers
{
    public class CommentsController : BaseController
    {
        #region Private Variables

        private ICommentService _commentService;

        #endregion Private Variables

        #region Constructors

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Parametreden gelen Id'nin yorum listesi
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{PostId}")]
        public IActionResult CommentList(int PostId, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_commentService.CommentList(PostId, pagingModel));
        }

        /// <summary>
        /// Parametreden gelen Id'ye yapýlan yorum sayısı
        /// </summary>
        /// <param name="PostId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{PostId}/Count")]
        public IActionResult Count(int PostId)
        {
            return Ok(_commentService.CommentCount(PostId));
        }

        /// <summary>
        /// Yorum ekle
        /// </summary>
        /// <param name="model">PostId ve Yorum</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CommentAdd([FromBody]CommentsModel model)
        {
            _commentService.Insert(new Comment
            {
                UserId = UserId,
                Text = model.Comment,
                PostId = model.PostId,
            });
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_commentService != null)
        //    {
        //        _commentService.Dispose();
        //        _commentService = null;
        //    }
        //}
    }
}