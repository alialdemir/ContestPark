namespace ContestPark.WebApi.Controllers
{
    public class CpsController : BaseController
    {
        #region Private Variables

        private ICpService _CpService;
        private IMissionService _missionService;

        #endregion Private Variables

        #region Constructors

        public CpsController(ICpService CpService, IMissionService missionService)
        {
            _CpService = CpService;
            _missionService = missionService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Login olan Kullanıcıya göre toplam altın miktarını verir
        /// </summary>
        /// <returns>Toplam altın miktarı</returns>
        [HttpGet]
        public IActionResult UserTotalCp()
        {
            return Ok(_CpService.UserTotalCp(UserId));
        }

        /// <summary>
        /// Eğer günlük chip almamış ise 10000-20000 arası rasgele chip ekler almış ise 0 döndürür
        /// </summary>
        /// <returns>Eklenen altın miktarı</returns>
        [HttpPost]
        public IActionResult AddRandomChip()
        {
            return Ok(_CpService.AddRandomChip(UserId));
        }

        /// <summary>
        /// Store'dan altın satın alınca alınan altını ekliyor
        /// </summary>
        /// <param name="productId">satın alınan ürün Id'si</param>
        [HttpPost]
        [Route("{productId}/BuyChip")]
        public IActionResult BuyChip(string productId)
        {
            _CpService.BuyChip(UserId, productId);
            return Ok();
        }

        /// <summary>
        /// Kendi duello Id'sine göre duellodaki bahis miktarı kadar Kullanıcılardan bahisi düþer
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        [HttpPost]
        [Route("{duelId}/ChipDistribution")]
        public IActionResult ChipDistribution(int duelId)
        {
            _CpService.ChipDistribution(duelId);
            _missionService.MissionComplete(UserId,
                Missions.Mission1,
                Missions.Mission2,
                Missions.Mission3,
                Missions.Mission4,
                Missions.Mission5,
                Missions.Mission6,
                Missions.Mission7,
                Missions.Mission8,
                Missions.Mission9,
                Missions.Mission10,
                Missions.Mission11,
                Missions.Mission12);// Görev yapıldı mı? kontrol ettik
            return Ok();
        }

        /// <summary>
        /// Bu method bot ile oynanan duellolarda düello sonunda kurucu kazanırrsa bahisini alması için
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        /// <param name="FounderScore">Düello kurucusu puanı</param>
        /// <param name="CompetitorScore">Rakip oyuncu puanı</param>
        [HttpPost]
        [Route("{duelId}/ChipDistribution/{founderScore}/{competitorScore}")]
        public IActionResult ChipDistribution(int duelId, byte founderScore, byte competitorScore)
        {
            _CpService.ChipDistribution(duelId, founderScore, competitorScore);
            _missionService.MissionComplete(UserId,
                Missions.Mission1,
                Missions.Mission2,
                Missions.Mission3,
                Missions.Mission4,
                Missions.Mission5,
                Missions.Mission6,
                Missions.Mission7,
                Missions.Mission8,
                Missions.Mission9,
                Missions.Mission10,
                Missions.Mission11,
                Missions.Mission12);// Görev yapıldı mı? kontrol ettik
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_CpService != null)
        //    {
        //        _CpService.Dispose();
        //        _CpService = null;
        //    }
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //}
    }
}