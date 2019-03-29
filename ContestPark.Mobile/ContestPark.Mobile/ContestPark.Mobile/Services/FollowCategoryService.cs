using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class FollowCategoryService : ServiceBase, IFollowCategoryService
    {
        #region Constructors

        public FollowCategoryService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının takip ettiği kategoriler
        /// </summary>
        /// <returns>Kullanıcının takip ettiği kategoriler</returns>
        public async Task<ServiceModel<SubCategoryModel>> FollowingSubCategoryListAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<SubCategoryModel>>($"FollowCategories{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Alt kategori takip et
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        public async Task<bool> FollowSubCategoryAsync(int subCategoryId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"FollowCategories/{subCategoryId}");
            return result.IsSuccess;
        }

        /// <summary>
        /// Alt kategori takip býrak
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        public async Task<bool> UnFollowSubCategoryAsync(int subCategoryId)
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"FollowCategories/{subCategoryId}");
            return result.IsSuccess;
        }

        /// <summary>
        /// Alt kategori Id'ye göre o kategoriyi takip eden kullanıcı sayısı
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kategoriyi takip eden kullanıcı sayısı</returns>
        public async Task<int> FollowersCountAsync(int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<int>($"FollowCategories/{subCategoryId}/FollowersCount");
            return result.Data;
        }

        /// <summary>
        /// Takip ettiği kategorileri search sayfasıbda listeleme
        /// </summary>
        /// <returns>Alt kategori listesi</returns>
        public async Task<ServiceModel<SubCategorySearchModel>> FollowingSubCategorySearchAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<SubCategorySearchModel>>($"FollowCategories/FollowingSubCategorySearch{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Kullanıcının kategoriyi takip etme durumu
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategoriyi ise takip ediyor true etmiyorsa ise false</returns>
        public async Task<bool> IsFollowUpStatusAsync(int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<bool>($"FollowCategories/{subCategoryId}/IsFollowUpStatus");
            return result.Data;
        }

        public async Task<bool> SubCategoryFollowProgcess(int subCategoryId, bool isSubCategoryFollowUpStatus)
        {
            if (isSubCategoryFollowUpStatus)
                return await UnFollowSubCategoryAsync(subCategoryId);// true ise takip ediyor

            return await FollowSubCategoryAsync(subCategoryId);// false ise takip etmiyor
        }

        #endregion Methods
    }

    public interface IFollowCategoryService
    {
        Task<ServiceModel<SubCategoryModel>> FollowingSubCategoryListAsync(PagingModel pagingModel);

        Task<bool> FollowSubCategoryAsync(int subCategoryId);

        Task<bool> UnFollowSubCategoryAsync(int subCategoryId);

        Task<int> FollowersCountAsync(int subCategoryId);

        Task<ServiceModel<SubCategorySearchModel>> FollowingSubCategorySearchAsync(PagingModel pagingModel);

        Task<bool> IsFollowUpStatusAsync(int subCategoryId);

        Task<bool> SubCategoryFollowProgcess(int subCategoryId, bool isSubCategoryFollowUpStatus);
    }
}