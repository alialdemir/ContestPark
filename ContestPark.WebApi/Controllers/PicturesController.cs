//using Microsoft.AspNetCore.Mvc;
//using ContestPark.BusinessLayer.Interfaces;
//using ContestPark.Entities.Models;

//namespace ContestPark.WebApi.Controllers
//{
//    public class PicturesController : BaseController
//    {
//        #region Private Variables
//        private IPictureService _pictureService;
//        private IUserService _userService;
//        #endregion
//        #region Constructors
//        public PicturesController(IPictureService pictureService, IUserService userService)
//        {
//            _pictureService = pictureService;
//            _userService = userService;
//        }
//        #endregion
//        #region Services
//        /// <summary>
//        /// Random kullanici resim listesi döndürür
//        /// </summary>
//        /// <returns>Resim Listesi</returns>
//        [HttpGet]
//        public IActionResult RandomUserProfilePictures([FromQuery]PagingModel pagingModel)
//        {
//            return Ok(_pictureService.RandomUserProfilePictures(UserId, pagingModel));
//        }
//        /// <summary>
//        /// Giriş yapan kullanıcının Profil resmi kaldır
//        /// </summary>
//        [HttpDelete]
//        [Route("Profile")]
//        public IActionResult RemoveProfilePicture()
//        {
//            _userService.RemoveProfilePicture(UserId);
//            return Ok();
//        }
//        #endregion
//        //protected override void Dispose(bool disposing)
//        //{
//        //    base.Dispose(disposing);
//        //    if (_pictureService != null)
//        //    {
//        //        _pictureService.Dispose();
//        //        _pictureService = null;
//        //    }
//        //    if (_userService != null)
//        //    {
//        //        _userService.Dispose();
//        //        _userService = null;
//        //    }
//        //}
//    }
//}