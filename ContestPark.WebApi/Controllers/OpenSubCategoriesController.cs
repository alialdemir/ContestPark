namespace ContestPark.WebApi.Controllers
{
    public class OpenSubCategoriesController : BaseController
    {
        #region Private Variables

        private IOpenSubCategoryService _openSubCategoryService;

        #endregion Private Variables

        #region Constructors

        public OpenSubCategoriesController(IOpenSubCategoryService openSubCategoryService)
        {
            _openSubCategoryService = openSubCategoryService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Alt kategori açma
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori açma durumu</returns>
        [HttpGet]
        [Route("{subCategoryId}")]
        public IActionResult OpenCategory(int subCategoryId)
        {
            return Ok(_openSubCategoryService.OpenCategory(UserId, subCategoryId));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_openSubCategoryService != null)
        //    {
        //        _openSubCategoryService.Dispose();
        //        _openSubCategoryService = null;
        //    }
        //}
    }
}