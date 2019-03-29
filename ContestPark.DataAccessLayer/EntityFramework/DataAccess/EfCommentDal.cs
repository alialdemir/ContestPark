using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfCommentDal : EfEntityRepositoryBase<Comment>, ICommentRepository
    {
        #region Constructors

        public EfCommentDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// İlgili posta yapılan yorum sayısı
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>Yorum sayısı</returns>
        public int CommentCount(int PostId)
        {
            return DbSet
                    .Where(p => p.PostId == PostId)
                    .Count();
        }

        /// <summary>
        /// İlgili posta yapılan yorumlar
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>Post listesi</returns>
        public ServiceModel<CommentListModel> CommentList(int PostId, PagingModel pagingModel)
        {
            return (from c in DbSet
                    where c.PostId == PostId
                    join u in DbContext.Set<User>() on c.UserId equals u.Id
                    orderby c.CreatedDate descending
                    select new CommentListModel
                    {
                        UserName = u.UserName,
                        ProfilePicturePath = u.ProfilePicturePath,
                        Text = c.Text,
                        Date = c.CreatedDate
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Kullanıcının en son posta yaptığı yorumun tarihi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="PostId">Kim ne yapıyor Id</param>
        /// <returns>En son yapılan yorumun tarihi</returns>
        public DateTime LastCommentTime(string userId, int PostId)
        {
            var date = DbSet
                            .Where(c => c.UserId == userId && c.PostId == PostId)
                            .OrderByDescending(p => p.CreatedDate)
                            .Select(p => p.CreatedDate)
                            .FirstOrDefault();
            return date != null ? date.Date : new DateTime();
        }

        #endregion Methods
    }
}