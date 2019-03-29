namespace ContestPark.WebApi.Controllers
{
    public class QuestionsController : BaseController
    {
        #region Private Variables

        private IQuestionService _questionService;

        #endregion Private Variables

        #region Constructors

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion Constructors

        #region Service

        /// <summary>
        /// Duellodaki soruları getirir
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Düelloda sorulan soruların listesi</returns>
        [HttpGet]
        [Route("{duelId}")]
        public IActionResult DuelQuestions(int duelId)
        {
            return Ok(_questionService.DuelQuestions(UserId, duelId));
        }

        #endregion Service

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_questionService != null)
        //    {
        //        _questionService.Dispose();
        //        _questionService = null;
        //    }
        //}
    }
}