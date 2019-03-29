namespace ContestPark.WebApi.Controllers
{
    public class NotificationsController : BaseController
    {
        #region Private Variables

        private INotificationService _notificationService;

        #endregion Private Variables

        #region Constructors

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Login olan kullanicinin t�m bildirimleri
        /// </summary>
        /// <param name="paging">Sayfalama 10 ve katlari olmali</param>
        /// <returns>Bildirim listesi</returns>
        [HttpGet]
        public IActionResult UserNotificationAll([FromQuery]PagingModel pagingModel)
        {
            return Ok(_notificationService.UserNotificationAll(UserId, pagingModel));
        }

        /// <summary>
        /// Login olan kullanicinin g�r�nmeyen bildirim sayisin� verir
        /// </summary>
        /// <returns>G�r�lmeyen bildirim sayisi</returns>
        [HttpGet]
        [Route("UserNotificationVisibilityCount")]
        public IActionResult UserNotificationVisibilityCount()
        {
            return Ok(_notificationService.UserNotificationVisibilityCount(UserId));
        }

        /// <summary>
        /// Bildirim sil
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{notificationId}")]
        public IActionResult Delete(int notificationId)
        {
            _notificationService.Delete(notificationId, UserId);
            return Ok();
        }

        /// <summary>
        /// Login olan kullanicinin t�m bildirimlerini g�r�ld� yapar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Allseen")]
        public IActionResult NotificationSeen()
        {
            _notificationService.NotificationSeen(UserId);
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_notificationService != null)
        //    {
        //        _notificationService.Dispose();
        //        _notificationService = null;
        //    }
        //}
    }
}