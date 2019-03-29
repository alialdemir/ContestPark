using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        PostListModel Post(string userId, int PostId);

        ServiceModel<PostListModel> PostList(string userId, string userName, PagingModel pagingModel);

        PostInfoModel GetUserId(int PostId);

        bool IsFollowControl(string userId, string contestantId);

        ServiceModel<PostListModel> ContestEnterScreen(string userId, int subCategoryId, PagingModel pagingModel);

        void Update(string userId, string contestantId, int duelId);

        IEnumerable<int> GetPostsByPictureId(int pictuteId);
    }
}