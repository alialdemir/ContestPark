namespace ContestPark.WebApi.Controllers
{
    public class SettingsController : BaseController
    {
        #region Private Variables

        private ISettingService _settingService;

        #endregion Private Variables

        #region Constructors

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Login olan kullanıcının parametreden gelen ayarın değerini döndürü
        /// </summary>
        /// <param name="settingTypeId">Ayar Id</param>
        /// <returns>Ayarýn değeri</returns>
        [HttpGet]
        [Route("{settingTypeId}")]
        public IActionResult GetSettingValue(byte settingTypeId)
        {
            return Ok(_settingService.GetSettingValue(UserId, settingTypeId));
        }

        /// <summary>
        /// Ayar değeri günceller
        /// </summary>
        /// <param name="value">Ayar değeri</param>
        /// <param name="settingTypeId">Ayar Id</param>
        [HttpPost]
        [Route("{settingTypeId}")]
        public IActionResult Update([FromBody]string value, byte settingTypeId)
        {
            _settingService.Update(UserId, value, settingTypeId);
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_settingService != null)
        //    {
        //        _settingService.Dispose();
        //        _settingService = null;
        //    }
        //}
    }
}