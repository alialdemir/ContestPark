namespace ContestPark.WebApi.Controllers
{
    public class ContestDatesController : BaseController
    {
        #region Private Variables

        private IContestDateService _contestDateService;

        #endregion Private Variables

        #region Constructors

        public ContestDatesController(IContestDateService contestDateService)
        {
            _contestDateService = contestDateService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Yarışmaların başlangıç ve bitiş tarihini verir
        /// </summary>
        /// <returns>Yarışmaların başlangıç ve bitiş tarihi</returns>
        [HttpGet]
        [Route("ContestStartAndEndDate")]
        public IActionResult ContestStartAndEndDate()
        {
            return Ok(_contestDateService.ContestStartAndEndDate());
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_contestDateService != null)
        //    {
        //        _contestDateService.Dispose();
        //        _contestDateService = null;
        //    }
        //}
    }
}