using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class CategoryServices : ServiceBase, ICategoryServices
    {
        #region Constructors

        public CategoryServices(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kategorileri listeleme
        /// </summary>
        /// <returns>Tüm kategorileri döndürür.</returns>
        public async Task<ServiceModel<CategoryModel>> CategoryListAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<CategoryModel>>($"ContestCategories{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Alt kategori Id'ye göre kategori listesi getirir 0 ise tüm kategoriler gelir
        /// </summary>
        /// <param name="CategoryId">CategoryId</param>
        /// <returns></returns>
        public async Task<ServiceModel<SubCategorySearchModel>> SearchAsync(int CategoryId, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<SubCategorySearchModel>>($"ContestCategories/{CategoryId}{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Login olan kullanýcýnýn diline göre alt kategorilerin isim listesini getirir
        /// </summary>
        /// <param name="subcategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori liste</returns>
        public async Task<string> SubCategoryNameByLanguageAsync(int CategoryId)
        {
            var result = await RequestProvider.GetDataAsync<string>($"ContestCategories/{CategoryId}/SubCategoryNameByLanguage");
            return result.Data;
        }

        #endregion Methods
    }

    public interface ICategoryServices
    {
        Task<ServiceModel<CategoryModel>> CategoryListAsync(PagingModel pagingModel);

        Task<ServiceModel<SubCategorySearchModel>> SearchAsync(int CategoryId, PagingModel pagingModel);

        Task<string> SubCategoryNameByLanguageAsync(int CategoryId);
    }
}