using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class LikesService : ServiceBase, ILikesService
    {
        #region Constructors

        public LikesService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Parametreden gelen kim ne yapıyor Id'sini beğenen kullanıcılar
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Beğenen kullanıcılar</returns>
        public async Task<ServiceModel<LikesModel>> Likes(int PostId, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<LikesModel>>($"Likes/{PostId}{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Beğen
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        public async Task<bool> Like(int PostId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Likes", PostId);
            return result.IsSuccess;
        }

        /// <summary>
        /// Beğenmekten vazgeç
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        public async Task<bool> DisLike(int PostId)
        {
            var result = await RequestProvider.DeleteDataAsync<string>($"Likes", PostId);
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface ILikesService
    {
        Task<ServiceModel<LikesModel>> Likes(int PostId, PagingModel pagingModel);

        Task<bool> Like(int PostId);

        Task<bool> DisLike(int PostId);
    }
}