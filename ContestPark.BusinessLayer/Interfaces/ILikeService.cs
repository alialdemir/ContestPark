using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ILikeService : IRepository<Like>
    {
        bool IsLike(string userId, int PostId);

        int LikeCount(int PostId);

        void DisLike(string userId, int PostId);

        ServiceModel<LikesModel> Likes(string userId, int PostId, PagingModel pagingModel);
    }
}