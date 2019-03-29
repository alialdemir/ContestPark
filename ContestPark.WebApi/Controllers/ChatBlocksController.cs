namespace ContestPark.WebApi.Controllers
{
    public class ChatBlocksController : BaseController
    {
        #region Private Variables

        private IChatBlockService _chatBlockService;

        #endregion Private Variables

        #region Constructors

        public ChatBlocksController(IChatBlockService chatBlockService)
        {
            _chatBlockService = chatBlockService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Login olan kullanıcının engellediği kullanıcıların listesi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserBlockList([FromQuery]PagingModel pagingModel)
        {
            return Ok(_chatBlockService.UserBlockList(UserId, pagingModel));
        }

        /// <summary>
        /// Login olan kullanıcı ile parametreden gelen kullanıcı arasýnda engelleme varmý varsa true yoksa false
        /// </summary>
        /// <param name="whonId">Kullanıcı Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{whonId}/BlockingStatus")]
        public IActionResult UserBlockingStatus(string whonId)
        {
            return Ok(_chatBlockService.UserBlockingStatus(UserId, whonId));
        }

        /// <summary>
        /// Parametreden gelen kullanıcı engelle
        /// </summary>
        /// <param name="whonId">kullanıcı Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{whonId}")]
        public IActionResult UserBlocking(string whonId)
        {
            _chatBlockService.UserBlocking(UserId, whonId);
            return Ok();
        }

        /// <summary>
        /// Parametreden gelen kullanıcının engellini kaldýr
        /// </summary>
        /// <param name="whonId">kullanıcı Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{whonId}")]
        public IActionResult Delete(string whonId)
        {
            _chatBlockService.Delete(UserId, whonId);
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_chatBlockService != null)
        //    {
        //        _chatBlockService.Dispose();
        //        _chatBlockService = null;
        //    }
        //}
    }
}