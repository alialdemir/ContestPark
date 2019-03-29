using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class CommentsService : ServiceBase, ICommentsService
    {
        #region Constructors

        public CommentsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Parametreden gelen Id'nin yorum listesi
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns></returns>
        public async Task<ServiceModel<CommentListModel>> CommentListAsync(int PostId, PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<ServiceModel<CommentListModel>>($"Comments/{PostId}{pagingModel}");
            return result.Data;
        }

        /// <summary>
        /// Parametreden gelen Id'ye yapýlan yorum sayýsý
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns></returns>
        public async Task<int> CountAsync(int PostId)
        {
            var result = await RequestProvider.GetDataAsync<int>($"Comments/{PostId}/Count");
            return result.Data;
        }

        /// <summary>
        /// Yorum ekle
        /// </summary>
        /// <param name="model">PostId ve Yorum</param>
        /// <returns></returns>
        public async Task<bool> CommentAddAsync(CommentsModel model)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Comments", model);
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface ICommentsService
    {
        Task<ServiceModel<CommentListModel>> CommentListAsync(int PostId, PagingModel pagingModel);

        Task<int> CountAsync(int PostId);

        Task<bool> CommentAddAsync(CommentsModel model);
    }
}