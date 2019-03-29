namespace ContestPark.WebApi.Controllers
{
    public class SubCategoriesController : BaseController
    {
        #region Private Variables

        private ISubCategoryService _subCategoryService;

        #endregion Private Variables

        #region Constructors

        public SubCategoriesController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        #endregion Constructors

        #region Service

        /// <summary>
        /// Alt kategori Id'sine göre resim url'sini verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Resim url'si</returns>
        [HttpGet]
        [Route("{subCategoryId}")]
        public IActionResult SubCategoryPicture(int subCategoryId)
        {
            return Ok(_subCategoryService.SubCategoryPicture(subCategoryId));
        }

        #endregion Service

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_subCategoryService != null)
        //    {
        //        _subCategoryService.Dispose();
        //        _subCategoryService = null;
        //    }
        //}
    }
}