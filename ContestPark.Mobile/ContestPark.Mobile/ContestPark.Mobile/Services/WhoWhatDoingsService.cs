using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class PostsService : ServiceBase, IPostsService
    {
        #region Constructors

        public PostsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Parametreden gelen kim ne yapiyor postu döndürür
        /// </summary>
        /// <param name="PostId">Kim ne yapiyor Id</param>
        /// <returns>kim ne yapiyor postu</returns>
        public async Task<PostListModel> PostAsync(int PostId)
        {
            var result = await RequestProvider.GetDataAsync<PostListModel>($"Posts/{PostId}");
            return result.Data;
        }

        /// <summary>
        /// Parametreden gelen kullanici adýna göre kim ne yapiyor listesi
        /// </summary>
        /// <param name="userName">kullanici adý</param>
        /// <param name="paging">Sayfalama 4 ve katlari olmali</param>
        /// <returns>Kim ne yapiyor listesi</returns>
        public async Task<ServiceModel<PostListModel>> PostListAsync(string userName, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<PostListModel>>($"Posts{pagingModel.ToString()}&userName={userName}");
            return result.Data;
        }

        /// <summary>
        /// Alt kategori id'ye göre kim ne yapiyor listesi getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <returns>Kim ne yapiyor listesi</returns>
        public async Task<ServiceModel<PostListModel>> ContestEnterScreenAsync(int subCategoryId, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<PostListModel>>($"Posts/{subCategoryId}/ContestEnterScreen{pagingModel.ToString()}");
            return result.Data;
        }

        #endregion Methods
    }

    public interface IPostsService
    {
        Task<PostListModel> PostAsync(int PostId);

        Task<ServiceModel<PostListModel>> PostListAsync(string userName, PagingModel pagingModel);

        Task<ServiceModel<PostListModel>> ContestEnterScreenAsync(int subCategoryId, PagingModel pagingModel);
    }
}