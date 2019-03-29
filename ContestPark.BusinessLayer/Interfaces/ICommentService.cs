using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ICommentService : IRepository<Comment>
    {
        int CommentCount(int PostId);

        ServiceModel<CommentListModel> CommentList(int PostId, PagingModel pagingModel);

        DateTime LastCommentTime(string userId, int PostId);
    }
}