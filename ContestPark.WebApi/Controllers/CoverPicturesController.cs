//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IO;
//using ContestPark.BusinessLayer.Interfaces;

//namespace ContestPark.WebApi.Controllers
//{
//    public class CoverPicturesController : BaseController
//    {
//        #region Private Variables
//        private ICoverPictureService _coverPictureService;
//        #endregion
//        #region Constructors
//        public CoverPicturesController(ICoverPictureService coverPictureService)
//        {
//            _coverPictureService = coverPictureService;
//        }
//        #endregion
//        #region Services
//        /// <summary>
//        /// Kullanıcı adına göre kapak resim yolu verir
//        /// </summary>
//        /// <param name="userName">Kullanıcı adı</param>
//        /// <returns>Kapak resim URL'si</returns>
//        [HttpGet]
//        [Route("{userName}")]
//        public IActionResult UserCoverPicturePath(string userName)
//        {
//            return Ok(_coverPictureService.GetCoverPictureByUserName(userName));
//        }
//        /// <summary>
//        /// Resim Id'sine göre kapak resmi yapak
//        /// </summary>
//        /// <param name="pictureId">Resim Id</param>
//        /// <returns>Kapak resmi yapýlan resmin url'si</returns>
//        [HttpPost]
//        [Route("{pictureId}")]
//        public IActionResult DoUserCoverBackgroundPicture(int pictureId)
//        {
//            return Ok(_coverPictureService.DoUserCoverBackgroundPicture(UserId, pictureId));
//        }
//        /// <summary>
//        /// Kullanıcı kapak resmi yükleme
//        /// </summary>
//        /// <param name="file">Kapak resmim dosyası</param>
//        /// <returns>Yüklenen resmin url'si</returns>
//        [HttpPost]
//        [Route("DoUserCoverBackgroundPicture")]
//        public IActionResult DoUserCoverBackgroundPicture(IFormFile file)//Resim YÜKLEME apisi ama kullanýlmýyor KULLANINCA BU YAZIYI KALDIR :D
//        {
//            if (file == null) BadRequest("Not found image file");
//            if (file.Length > 0)
//            {
//                Stream streamImage = file.OpenReadStream();
//                string contentType = file.ContentType;
//                return Ok(_coverPictureService.DoUserCoverBackgroundPicture(UserId, contentType, streamImage));
//            }
//            return BadRequest("File length is too small");
//        }
//        /// <summary>
//        /// Kullanıcının kapak resmini kaldır
//        /// </summary>
//        [HttpDelete]
//        public IActionResult Delete()
//        {
//            _coverPictureService.RemoveCoverPicture(UserId);
//            return Ok();
//        }
//        #endregion
//        //protected override void Dispose(bool disposing)
//        //{
//        //    base.Dispose(disposing);
//        //    if (_coverPictureService != null)
//        //    {
//        //        _coverPictureService.Dispose();
//        //        _coverPictureService = null;
//        //    }
//        //}
//    }
//}