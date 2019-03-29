using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class CoverPicturesService : ServiceBase, ICoverPicturesService
    {
        #region Constructors

        public CoverPicturesService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcı adına göre kapak resim yolu verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kapak resim URL'si</returns>
        public async Task<string> UserCoverPicturePath()
        {
            var result = await RequestProvider.GetDataAsync<string>("CoverPictures");
            return result.Data;
        }

        /// <summary>
        /// Resim Id'sine göre kapak resmi yapak
        /// </summary>
        /// <param name="pictureId">Resim Id</param>
        /// <returns>Kapak resmi yapýlan resmin url'si</returns>
        public async Task<string> DoUserCoverBackgroundPicture(int pictureId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"CoverPictures/{pictureId}");
            return result.Data;
        }

        /// <summary>
        /// Kullanıcı kapak resmi yükleme
        /// </summary>
        /// <returns>Yüklenen resmin url'si</returns>
        public async Task<string> DoUserCoverBackgroundPicture()// çalışmıyor bu file transfer olması lazım
        {
            var result = await RequestProvider.PostDataAsync<string>($"CoverPictures/DoUserCoverBackgroundPicture");
            return result.Data;
        }

        /// <summary>
        /// Kullanıcının kapak resmini kaldır
        /// </summary>
        public async Task<string> RemoveCoverPicture()
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"CoverPictures");
            return result.Data;
        }

        #endregion Methods
    }

    public interface ICoverPicturesService
    {
        Task<string> UserCoverPicturePath();

        Task<string> DoUserCoverBackgroundPicture(int pictureId);

        Task<string> DoUserCoverBackgroundPicture();

        Task<string> RemoveCoverPicture();
    }
}