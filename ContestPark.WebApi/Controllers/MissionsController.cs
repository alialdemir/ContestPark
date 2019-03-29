namespace ContestPark.WebApi.Controllers
{
    public class MissionsController : BaseController
    {
        #region Private Variables

        private IMissionService _missionService;

        #endregion Private Variables

        #region Constructors

        public MissionsController(IMissionService missionService)
        {
            _missionService = missionService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Login olan kullanicinin görev listesi
        /// </summary>
        /// <returns>Görev listesi</returns>
        [HttpGet]
        public IActionResult MissionList([FromQuery]PagingModel pagingModel)
        {
            return Ok(_missionService.MissionList(UserId, pagingModel));
        }

        /// <summary>
        /// Tamamlanan görevin altınını al
        /// </summary>
        /// <param name="mission">Görev Id</param>
        [HttpPost]
        [Route("{mission}")]
        public IActionResult TakesMissionGold(Missions mission)
        {
            _missionService.TakesMissionGold(UserId, mission);
            return Ok();
        }

        /// <summary>
        /// Görevi tamamla
        /// </summary>
        /// <param name="mission">Görev Id</param>
        [HttpPost]
        [Route("{mission}/MissionComplete")]
        public IActionResult MissionComplete(Missions mission)
        {
            _missionService.MissionComplete(UserId, Missions.Mission35);// Görev yapildi mi? kontrol ettik
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //}
    }
}