using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class OpenSubCategoryService : ServiceBase, IOpenSubCategoryService
    {
        #region Constructors

        public OpenSubCategoryService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Alt kategori açma
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori açma durumu</returns>
        public async Task<bool> OpenCategoryAsync(int subCategoryId)
        {
            var result = await RequestProvider.GetDataAsync<bool>($"OpenSubCategories/{subCategoryId}");
            return result.Data;
        }

        #endregion Methods
    }

    public interface IOpenSubCategoryService
    {
        Task<bool> OpenCategoryAsync(int subCategoryId);
    }
}