namespace ContestPark.WebApi.Controllers
{
    public class ChatRepliesController : BaseController
    {
        #region Private Variables

        private IChatReplyService _chatReplyService;

        #endregion Private Variables

        #region Constructors

        public ChatRepliesController(IChatReplyService chatReplyService)
        {
            _chatReplyService = chatReplyService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Kullanıcının okunmamış mesaj sayısı
        /// </summary>
        /// <returns>Okunmamış mesaj sayısı</returns>
        [HttpGet]
        public IActionResult UserChatVisibilityCount()
        {
            return Ok(_chatReplyService.UserChatVisibilityCount(UserId));
        }

        /// <summary>
        /// Parametreden gelen chat id'yi görüldü yapar
        /// </summary>
        /// <param name="chatId">Mesaj Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{chatId:int}")]
        public IActionResult ChatSeen(int chatId)
        {
            _chatReplyService.ChatSeen(UserId, chatId);
            return Ok();
        }

        /// <summary>
        /// kullanıcının görmediði tüm mesajlarýný görüldü yapar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Allseen")]
        public IActionResult ChatSeen()
        {
            return Ok(_chatReplyService.ChatSeen(UserId));
        }

        /// <summary>
        /// Mesaj sil
        /// </summary>
        /// <param name="receiverUserId">Alıcı kullanıcı Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{receiverUserId}")]
        public IActionResult Delete(string receiverUserId)
        {
            return Ok(_chatReplyService.Delete(UserId, receiverUserId));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_chatReplyService != null)
        //    {
        //        _chatReplyService.Dispose();
        //        _chatReplyService = null;
        //    }
        //}
    }
}