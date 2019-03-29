using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IPostService : IRepository<Post>
    {
        PostListModel Post(string userId, int PostId);

        ServiceModel<PostListModel> PostList(string userId, string userName, PagingModel pagingModel);

        PostInfoModel GetUserId(int PostId);

        bool IsFollowControl(string userId, string contestantId);

        ServiceModel<PostListModel> ContestEnterScreen(string userId, int subCategoryId, PagingModel pagingModel);

        void Update(string userId, string contestantId, int duelId);

        void DeleteAllPostByPictureId(int pictuteId);
    }
}