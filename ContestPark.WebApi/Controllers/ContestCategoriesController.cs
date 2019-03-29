namespace ContestPark.WebApi.Controllers
{
    public class ContestCategoriesController : BaseController
    {
        #region Private Variables

        private ICategoryService _CategoryService;
        private ISubCategoryLangService _subCategoryLangService;
        // private IMemoryCache _memoryCache;

        #endregion Private Variables

        #region Constructors

        public ContestCategoriesController(ICategoryService CategoryService, ISubCategoryLangService subCategoryLangService/*, IMemoryCache memoryCache*/)
        {
            _CategoryService = CategoryService;
            _subCategoryLangService = subCategoryLangService;
            //   _memoryCache = memoryCache;
        }

        #endregion Constructors

        //     private const string CategoryListCacheKey = "CategoriesViewModel-CategoryList-cache-key";

        #region Services

        /// <summary>
        /// Kategorileri listeleme
        /// </summary>
        /// <returns>Tüm kategorileri döndürür.</returns>
        [HttpGet]
        public IActionResult CategoryList([FromQuery]PagingModel pagingModel)
        {
            // CACHE sample
            /*  if (_memoryCache.TryGetValue(CategoryListCacheKey + UserId, out ServiceModel<CategoryModel> categoryList))
              {
                  return Ok(categoryList);
              }*/

            /// categoryList = _CategoryService.CategoryList(UserId, UserLanguages, pagingModel);
            //  _memoryCache.Set(CategoryListCacheKey + UserId, categoryList, TimeSpan.FromMinutes(30));
            //   return Ok(categoryList);
            return Ok(_CategoryService.CategoryList(UserId, pagingModel));
        }

        /// <summary>
        /// Alt kategori Id'ye göre kategori listesi getirir 0 ise tüm kategoriler gelir
        /// </summary>
        /// <param name="CategoryId">CategoryId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{categoryId}")]
        public IActionResult Search(int categoryId, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_CategoryService.SearchCategory(UserId, categoryId, pagingModel));
        }

        /// <summary>
        /// Login olan kullanicinin diline göre alt kategorilerin isim listesini getirir
        /// </summary>
        /// <param name="subcategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori liste</returns>
        [HttpGet]
        [Route("{subcategoryId}/SubCategoryNameByLanguage")]
        public IActionResult SubCategoryNameByLanguage(int subcategoryId)
        {
            return Ok(_subCategoryLangService.SubCategoryNameByLanguage(UserId, subcategoryId));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_CategoryService != null)
        //    {
        //        _CategoryService.Dispose();
        //        _CategoryService = null;
        //    }
        //    if (_subCategoryLangService != null)
        //    {
        //        _subCategoryLangService.Dispose();
        //        _subCategoryLangService = null;
        //    }
        //}
    }
}