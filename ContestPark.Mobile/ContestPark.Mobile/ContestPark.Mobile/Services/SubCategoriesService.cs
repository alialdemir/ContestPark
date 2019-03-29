using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class SubCategoriesService : ServiceBase, ISubCategoriesService
    {
        #region Constructors

        public SubCategoriesService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Alt kategori Id'sine göre resim url'sini verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Resim url'si</returns>
        public async Task<string> SubCategoryPictureAsync(int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<string>($"SubCategories/{subCategoryId}");
            return result.Data;
        }

        #endregion Methods
    }

    public interface ISubCategoriesService
    {
        Task<string> SubCategoryPictureAsync(int subCategoryId);
    }
}