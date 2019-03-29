using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfLikeDal : EfEntityRepositoryBase<Like>, ILikeRepository
    {
        #region Constructors

        public EfLikeDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Parametreden gelen kullanıcı id'sinin yine parametreden gelen kim ne yapıyor id'sini beğenme durumu
        /// Beğendiyse true beğenmediyse false dönder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns>Beğendiyse true beğenmediyse false</returns>
        public bool IsLike(string userId, int PostId)//Beğenme durumu kontrol
        {
            return DbSet
                      .Where(p => p.UserId == userId && p.PostId == PostId)
                      .Any();
        }

        /// <summary>
        /// Parametreden gelen kim ne yapıyor id'sini beğenen kullanıcı sayısı
        /// </summary>
        /// <param name="PostId">Kim ne yapıyor id</param>
        /// <returns>Beğenen kullanıcı sayısı</returns>
        public int LikeCount(int PostId)
        {
            return DbSet
                       .Where(p => p.PostId == PostId)
                       .Count();
        }

        /// <summary>
        /// Kim ne yapıyoru beğenmekten vazgeç
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">Kim ne yapıyor id</param>
        public void DisLike(string userId, int PostId)
        {
            int row = DbContext.Database.ExecuteSqlCommand($"delete from Likes where UserId={userId} and PostId={PostId}");
            if (row <= 0) BadStatus("ServerMessage_thisLinkUnLike");
        }

        /// <summary>
        /// Parametreden gelen kim ne yapıyor id'sini beğenen kullanıcılar
        /// ve
        /// yine parametreden gelen kullanıcının o postu beğenme durumu
        /// IsFollowUpStatus: beğendiyse true beğenmediyse false
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">Kim ne apıyor id</param>
        /// <param name="pagingModel">Sayfalama</param>
        /// <returns>Beğenen kullanıcı bilgileri</returns>
        public ServiceModel<LikesModel> Likes(string userId, int PostId, PagingModel pagingModel)//Durumu beğenen kullanıcıların listesi
        {
            return (from l in DbSet
                    where l.PostId == PostId
                    join u in DbContext.Set<User>() on l.UserId equals u.Id
                    select new LikesModel
                    {
                        FollowUpUserId = u.Id,
                        FullName = u.FullName,
                        UserName = u.UserName,
                        ProfilePicturePath = u.ProfilePicturePath,
                        IsFollowUpStatus = DbContext.Set<Follow>().Where(p => p.FollowUpUserId == userId && p.FollowedUserId == u.Id).Any(),
                    }).ToServiceModel(pagingModel);
        }

        #endregion Methods
    }
}