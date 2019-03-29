namespace ContestPark.WebApi.Controllers
{
    public class FollowCategoriesController : BaseController
    {
        #region Private Variables

        private IFollowCategoryService _followCategoryService;
        private IMissionService _missionService;

        #endregion Private Variables

        #region Constructors

        public FollowCategoriesController(IFollowCategoryService followCategoryService, IMissionService missionService)
        {
            _followCategoryService = followCategoryService;
            _missionService = missionService;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Kullanıcının takip ettiği kategoriler
        /// </summary>
        /// <returns>Kullanıcının takip ettiği kategoriler</returns>
        [HttpGet]
        public IActionResult FollowingSubCategoryList([FromQuery]PagingModel pagingModel)
        {
            return Ok(_followCategoryService.FollowingSubCategoryList(UserId, pagingModel));
        }

        /// <summary>
        /// Alt kategori takip et
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        [HttpPost]
        [Route("{subCategoryId}")]
        public IActionResult FollowSubCategory(int subCategoryId)
        {
            string userId = UserId;
            _followCategoryService.Insert(new FollowCategory
            {
                UserId = userId,
                SubCategoryId = subCategoryId
            });
            _missionService.MissionComplete(userId,
                Missions.Mission29,
                Missions.Mission30,
                Missions.Mission31);// Görev yapıldı mı? kontrol ettik
            return Ok();
        }

        /// <summary>
        /// Alt kategori takip býrak
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        [HttpDelete]
        [Route("{subCategoryId}")]
        public IActionResult UnFollowSubCategory(int subCategoryId)
        {
            _followCategoryService.Delete(UserId, subCategoryId);
            return Ok();
        }

        /// <summary>
        /// Alt kategori Id'ye göre o kategoriyi takip eden kullanıcı sayısı
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kategoriyi takip eden kullanıcı sayısı</returns>
        [HttpGet]
        [Route("{subCategoryId}/FollowersCount")]
        public IActionResult FollowersCount(int subCategoryId)
        {
            return Ok(_followCategoryService.FollowersCount(subCategoryId));
        }

        /// <summary>
        /// Takip ettiği kategorileri search sayfasıbda listeleme
        /// </summary>
        /// <returns>Alt kategori listesi</returns>
        [HttpGet]
        [Route("FollowingSubCategorySearch")]
        public IActionResult FollowingSubCategorySearch([FromQuery]PagingModel pagingModel)
        {
            return Ok(_followCategoryService.FollowingSubCategorySearch(UserId, pagingModel));
        }

        /// <summary>
        /// Kullanıcının kategoriyi takip etme durumu
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategoriyi ise takip ediyor true etmiyorsa ise false</returns>
        [HttpGet]
        [Route("{subCategoryId}/IsFollowUpStatus")]
        public IActionResult IsFollowUpStatus(int subCategoryId)
        {
            return Ok(_followCategoryService.IsFollowUpStatus(UserId, subCategoryId));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_followCategoryService != null)
        //    {
        //        _followCategoryService.Dispose();
        //        _followCategoryService = null;
        //    }
        //    if (_missionService != null)
        //    {
        //        _missionService.Dispose();
        //        _missionService = null;
        //    }
        //}
    }
}