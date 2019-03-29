namespace ContestPark.WebApi.Controllers
{
    public class BoostsController : BaseController
    {
        #region Private Variables

        private IBoostService _boostService;
        private ICpService _CpPService;

        #endregion Private Variables

        #region Constructors

        public BoostsController(IBoostService boostService, ICpService CpPService)
        {
            _boostService = boostService;
            _CpPService = CpPService;
        }

        #endregion Constructors

        #region Services

        // v1/Boosts
        /// <summary>
        /// Joker listesi getirme
        /// </summary>
        /// <returns>Joker listesi</returns>
        [HttpGet]
        public IActionResult BoostList()
        {
            return Ok(_boostService.BoostList());
        }

        // v1/Boosts/{boost}/Buy
        /// <summary>
        /// Joker alma
        /// </summary>
        /// <param name="boost">Boost Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{boost}/Buy")]
        public IActionResult Buy(int boost)
        {
            _boostService.BuyBoost(UserId, boost);
            return Ok();
        }

        // v1/Boosts/SellBoosts/{boost}
        /// <summary>
        /// Jokeri geri satma
        /// </summary>
        /// <param name="boost">Boost Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{boost}/Sell")]
        public IActionResult Sell(int boost)
        {
            // TODO: dışarıdan sürekli bu api çağrılırsa çağrılan altın miktarı kadar kullanıcıya altın ekleme bugu var Satın alınan altınları sunucuda bir yerde tutmak lazım....
            byte boostGold = _boostService.FindBoostGold(boost);
            _CpPService.AddChip(UserId, boostGold, ChipProcessNames.Boost);
            return Ok();
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_boostService != null)
        //    {
        //        _boostService.Dispose();
        //        _boostService = null;
        //    }
        //    if (_CpPService != null)
        //    {
        //        _CpPService.Dispose();
        //        _CpPService = null;
        //    }
        //}
    }
}