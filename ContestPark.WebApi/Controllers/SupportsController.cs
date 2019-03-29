namespace ContestPark.WebApi.Controllers
{
    public class SupportsController : BaseController
    {
        #region Private Variables

        private ISupportService _supportService;
        private ISupportTypeService _supportTypeService;

        #endregion Private Variables

        #region Constructors

        public SupportsController(ISupportService supportService, ISupportTypeService supportTypeService)
        {
            _supportService = supportService;
            _supportTypeService = supportTypeService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Destek tiplerinin listesini döndürür
        /// </summary>
        /// <returns>Destek tip listesi</returns>
        [HttpGet]
        public IActionResult GetSupportTypes()
        {
            return Ok(_supportTypeService.GetSupportTypes(UserId));
        }

        /// <summary>
        /// Destek isteği gönder
        /// </summary>
        /// <param name="model">Destek mesajı ve destek tipi</param>
        /// <returns>Başarılı mesajı</returns>
        [HttpPost]
        public IActionResult Insert([FromBody]SupportModel model)
        {
            _supportService
                .Insert(new Support()
                {
                    UserId = UserId,
                    Message = model.Message,
                    SupportTypeId = (byte)model.SupportTypeId,
                    Status = true
                });
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_supportService != null)
        //    {
        //        _supportService.Dispose();
        //        _supportService = null;
        //    }
        //    if (_supportTypeService != null)
        //    {
        //        _supportTypeService.Dispose();
        //        _supportTypeService = null;
        //    }
        //}
    }
}