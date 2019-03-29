using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfPostDal : EfEntityRepositoryBase<Post>, IPostRepository
    {
        #region Private Variables

        private readonly ILikeRepository _like;
        private readonly ICommentRepository _comment;

        #endregion Private Variables

        #region Constructors

        public EfPostDal(ILikeRepository like, ICommentRepository comment, IDbFactory dbFactory)
            : base(dbFactory)
        {
            _comment = comment;
            _like = like;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// kim ne yapıyor Id'sine göre kullanıı Id'sini verir
        /// </summary>
        /// <param name="PostId">kim ne yapıyor Id</param>
        /// <returns>Kim ne yapıyor bilgi modeli</returns>
        public PostInfoModel GetUserId(int PostId)
        {
            return DbSet
                    .Where(p => p.PostId == PostId)
                    .Select(wwd => new PostInfoModel
                    {
                        ContestantId = wwd.ContestantId,
                        UserId = wwd.UserId,
                        PostTypes = (PostTypes)wwd.PostTypeId
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Kim ne yapıyoru tek post olarak göstermek için
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">kim ne yapıyor Id</param>
        /// <returns>Kim ne yapıyor modeli</returns>
        public PostListModel Post(string userId, int PostId)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from wwd in DbSet.AsEnumerable()
                    where wwd.PostId == PostId
                    join wwdtl in DbContext.Set<PostTypeLang>() on wwd.PostTypeId equals wwdtl.PostTypeId
                    where wwdtl.LanguageId == (byte)language

                    join sc in DbContext.Set<SubCategory>() on wwd.SubCategoryId equals sc.SubCategoryId into scData
                    from scData1 in scData.DefaultIfEmpty()

                    join scl in DbContext.Set<SubCategoryLang>().Where(p => p.LanguageId == (byte)language) on (int)wwd.SubCategoryId equals scl.SubCategoryId into cData
                    from cData1 in cData.DefaultIfEmpty()

                    join founderUser in DbContext.Set<User>() on wwd.UserId equals founderUser.Id
                    join competitorUser in DbContext.Set<User>() on wwd.ContestantId equals competitorUser.Id
                    select new PostListModel
                    {
                        Date = wwd.Date,//Post eklenme tarihi
                        SubCategoryId = wwd.SubCategoryId,
                        AlternativeId = wwd.DuelId.ToString(),
                        SubCategoryName = cData1 != null ? cData1.SubCategoryName : "",

                        AlternativePicturePath = wwd.PostTypeId == (int)PostTypes.ProfilePictureChanged
                        || wwd.PostTypeId == (int)PostTypes.CoverPictureChanged
                        ? ""
                        //DbContext.Set<Picture>().Where(p => p.PictureId == wwd.DuelId).Select(p => p.PicturePath).FirstOrDefault()

                        : scData1 != null ? scData1.PictuePath : "",//PostTypeId profil resmi veya kapak resmi ise resim yolunu getirir.
                        PostsDescription = wwdtl.Description,

                        PostType = (PostTypes)wwd.PostTypeId,//Post tip Id

                        PostId = wwd.PostId,
                        IsLike = _like.IsLike(userId, wwd.PostId),
                        LikeCount = _like.LikeCount(wwd.PostId),
                        CommentCount = _comment.CommentCount(wwd.PostId),
                        //Kurucu
                        FounderFullName = founderUser.FullName,
                        FounderUserName = founderUser.UserName,
                        FounderProfilePicturePath = founderUser.ProfilePicturePath,
                        FounderTrueAnswerCount = DbContext.Set<DuelInfo>().Where(dqi => dqi.DuelId == wwd.DuelId && dqi.FounderUserAnswer == dqi.TrueAnswer).Count(),
                        //Rakip
                        CompetitorFullName = competitorUser.FullName,
                        CompetitorUserName = competitorUser.UserName,
                        CompetitorProfilePicturePath = competitorUser.ProfilePicturePath,
                        CompetitorTrueAnswerCount = DbContext.Set<DuelInfo>().Where(dqi => dqi.DuelId == wwd.DuelId && dqi.CompetitorUserAnswer == dqi.TrueAnswer).Count()
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcı yarışmacıyı takip ediyor mu kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="contestantId">Rakip kullanıcı id</param>
        /// <returns>Takip ediyor ise true etmiyor ise false</returns>
        public bool IsFollowControl(string userId, string contestantId)
        {
            return DbSet
                      .Where(p => p.UserId == userId && p.ContestantId == contestantId && (p.Date - DateTime.Now.Date).Seconds < 120)
                      .Any();
        }

        /// <summary>
        /// Yarışmalarının giriş ekranında kim ne yapıyordan gelen datalardan yarışma sonuçlarını listelemek için kullandım
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="paging">Sayfalama için 4 ve katları olmalı</param>
        /// <returns>Yarışma kategorisine ait kim yapıyor listesi</returns>
        public ServiceModel<PostListModel> ContestEnterScreen(string userId, int subCategoryId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from wwd in DbSet
                    join founderUser in DbContext.Set<User>() on wwd.UserId equals founderUser.Id
                    join competitorUser in DbContext.Set<User>() on wwd.ContestantId equals competitorUser.Id
                    let PostTypeId = (int)PostTypes.ContestDuel
                    let xUserId = userId
                    where wwd.PostTypeId == PostTypeId && wwd.ContestantContestStatus == false && wwd.SubCategoryId == subCategoryId
                    join sc in DbContext.Set<SubCategory>() on wwd.SubCategoryId equals sc.SubCategoryId
                    orderby wwd.Date descending
                    select new PostListModel
                    {
                        SubCategoryId = wwd.SubCategoryId,
                        AlternativeId = wwd.DuelId.ToString(),
                        PostId = wwd.PostId,
                        IsLike = wwd.Likes.Where(l => l.UserId == xUserId).Any(),
                        LikeCount = wwd.Likes.Count(),
                        CommentCount = wwd.Comments.Count(),
                        Date = wwd.Date,
                        AlternativePicturePath = sc.PictuePath,
                        PostType = (PostTypes)wwd.PostTypeId,
                        SubCategoryName = (from scl in DbContext.Set<SubCategoryLang>()
                                           where scl.SubCategoryId == wwd.SubCategoryId && scl.LanguageId == (byte)language
                                           select scl.SubCategoryName).FirstOrDefault(),

                        //Kurucu
                        FounderFullName = founderUser.FullName,
                        FounderUserName = founderUser.UserName,
                        FounderProfilePicturePath = founderUser.ProfilePicturePath,// _picture.GetPicuturePathByPictureId(founderUser.ProfileResimId),
                        FounderTrueAnswerCount = (from di in DbContext.Set<DuelInfo>()
                                                  where di.DuelId == wwd.DuelId && wwd.PostTypeId == PostTypeId
                                                  join s in DbContext.Set<Score>() on di.DuelInfoId equals s.DuelInfoId
                                                  where s.UserId == founderUser.Id
                                                  select s.Point).Sum(x => x),
                        //Rakip
                        CompetitorFullName = competitorUser.FullName,
                        CompetitorUserName = competitorUser.UserName,
                        CompetitorProfilePicturePath = competitorUser.ProfilePicturePath, // _picture.GetPicuturePathByPictureId(competitorUser.ProfileResimId),
                        CompetitorTrueAnswerCount = (from di in DbContext.Set<DuelInfo>()
                                                     where di.DuelId == wwd.DuelId && wwd.PostTypeId == PostTypeId
                                                     join s in DbContext.Set<Score>() on di.DuelInfoId equals s.DuelInfoId
                                                     where s.UserId == competitorUser.Id
                                                     select s.Point).Sum(x => x)
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Kim yapıyor güncelle
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="contestantId">Rakip kullanıcı id</param>
        /// <param name="duelId">Düello id</param>
        public void Update(string userId, string contestantId, int duelId)
        {
            Post PostUpdate = DbSet
                                                .Where(wwd => wwd.DuelId == duelId && ((wwd.UserId == userId && wwd.ContestantId == contestantId) || (wwd.UserId == contestantId && wwd.ContestantId == userId)))
                                                .Select(p => p)
                                                .FirstOrDefault();
            if (PostUpdate != null)
            {
                PostUpdate.ContestantContestStatus = true;
                base.Update(PostUpdate);
            }
        }

        /// <summary>
        /// Parametreden gelen kullanıcı adına göre tüm kim ne yapıyor listesi
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="paging">Sayfalama için 4 ve katları olmalı</param>
        /// <returns>Kim ne yapıyor listesi</returns>
        public ServiceModel<PostListModel> PostList(string userId, string userName, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string xuserId = DbContext.Set<User>().Where(p => p.UserName == userName).Select(p => p.Id).FirstOrDefault();
            return (from wwd in DbSet
                    where (wwd.UserId == xuserId || wwd.ContestantId == xuserId) && wwd.ContestantContestStatus

                    join founderUser in DbContext.Set<User>() on wwd.UserId equals founderUser.Id
                    join competitorUser in DbContext.Set<User>() on wwd.ContestantId equals competitorUser.Id

                    orderby wwd.Date descending
                    let languageId = (byte)language
                    let SubCategoryName = (from scl in DbContext.Set<SubCategoryLang>()
                                           where scl.SubCategoryId == wwd.SubCategoryId && scl.LanguageId == languageId
                                           select scl.SubCategoryName).FirstOrDefault()
                    let SubCategoryPicturePath = (from sc in DbContext.Set<SubCategory>()
                                                  where sc.SubCategoryId == wwd.SubCategoryId
                                                  select sc.PictuePath).FirstOrDefault()
                    select new PostListModel
                    {
                        Date = wwd.Date,
                        SubCategoryId = wwd.SubCategoryId,
                        AlternativeId = wwd.DuelId.ToString(),
                        SubCategoryName = SubCategoryName,
                        AlternativePicturePath = wwd.PostTypeId == (int)PostTypes.ContestDuel
                                                ? SubCategoryPicturePath
                                                : ""/*DbContext.Set<Picture>().Where(p => p.PictureId == wwd.DuelIdOrPictureId).Select(p => p.PicturePath).FirstOrDefault()*/,
                        PostsDescription = wwd.PostType.PostTypeLangs.Where(p => p.LanguageId == languageId).Select(p => p.Description).FirstOrDefault(),
                        PostType = (PostTypes)wwd.PostTypeId,
                        PostId = wwd.PostId,
                        IsLike = wwd.Likes.Where(p => p.UserId == userId).Any(),
                        LikeCount = wwd.Likes.Count(),
                        CommentCount = wwd.Comments.Count(),

                        ////Kurucu
                        FounderFullName = founderUser.FullName,
                        FounderUserName = founderUser.UserName,
                        FounderProfilePicturePath = founderUser.ProfilePicturePath,
                        FounderTrueAnswerCount = (from di in DbContext.Set<DuelInfo>()
                                                  where di.DuelId == wwd.DuelId && wwd.PostTypeId == (int)PostTypes.ContestDuel
                                                  join s in DbContext.Set<Score>() on di.DuelInfoId equals s.DuelInfoId
                                                  where s.UserId == founderUser.Id
                                                  select s.Point).Sum(p => p),
                        ////Rakip
                        CompetitorFullName = competitorUser.FullName,
                        CompetitorUserName = competitorUser.UserName,
                        CompetitorProfilePicturePath = competitorUser.ProfilePicturePath,
                        CompetitorTrueAnswerCount = (from di in DbContext.Set<DuelInfo>()
                                                     where di.DuelId == wwd.DuelId && wwd.PostTypeId == (int)PostTypes.ContestDuel
                                                     join s in DbContext.Set<Score>() on di.DuelInfoId equals s.DuelInfoId
                                                     where s.UserId == competitorUser.Id
                                                     select s.Point).Sum(p => p)
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// PostTypeId'ye göre profil veya kapak resmimlerinin PostId'lerini verir
        /// </summary>
        /// <param name="pictuteId">Resim Id</param>
        /// <returns>PostTypeId list</returns>
        public IEnumerable<int> GetPostsByPictureId(int pictuteId)
        {
            return DbContext.Set<Post>()
                    .Where(wwd => wwd.DuelId == pictuteId && (wwd.PostTypeId == (int)PostTypes.ProfilePictureChanged || wwd.PostTypeId == (int)PostTypes.CoverPictureChanged))
                    .Select(wwd => wwd.PostId).ToList();
        }

        #endregion Methods
    }
}