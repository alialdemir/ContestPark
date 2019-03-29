using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class PicturesService : ServiceBase, IPicturesService
    {
        #region Constructors

        public PicturesService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Random kullanici resim listesi döndürür
        /// </summary>
        /// <returns>Resim Listesi</returns>
        public async Task<ServiceModel<string>> RandomUserProfilePicturesAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<string>>($"Pictures{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Giriş yapan kullanıcının Profil resmi kaldır
        /// </summary>
        public async Task<bool> RemoveProfilePicture()
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"Pictures/Profile");
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface IPicturesService
    {
        Task<ServiceModel<string>> RandomUserProfilePicturesAsync(PagingModel pagingModel);

        Task<bool> RemoveProfilePicture();
    }
}