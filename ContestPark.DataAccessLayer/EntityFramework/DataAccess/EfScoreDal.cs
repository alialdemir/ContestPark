using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfScoreDal : EfEntityRepositoryBase<Score>, IScoreRepository
    {
        #region Private Variables

        private readonly IGlobalSettings _globalSettings;
        private readonly ContestStartAndEndDateModel _contestStartAndEndDate;

        #endregion Private Variables

        #region Constructors

        public EfScoreDal(IContestDateRepository contestDate, IGlobalSettings globalSettings, IDbFactory dbFactory)
            : base(dbFactory)
        {
            _contestStartAndEndDate = contestDate.ContestStartAndEndDate();//o yarışma kategorindeki son yarışma tarihlerini alıyoruz ona göre sıralama getiriyoruz
            _globalSettings = globalSettings;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Düello sonucu ekranında kendi sırasının 2 fazla ve 2  eksiğini gösterir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kullanıcının 2 alt ve 2 üst sıralaması</returns>
        public DuelResultRankingModel DuelResultRanking(string userId, int subCategoryId)
        {
            if (_contestStartAndEndDate == null) return null;
            var r = (from s in DbSet.AsEnumerable()
                     where s.SubCategoryId == subCategoryId && s.ScoreDate >= _contestStartAndEndDate.StartDate && s.ScoreDate <= _contestStartAndEndDate.FinishDate
                     group s by s.UserId into sData
                     join u in DbContext.Set<User>() on sData.Key equals u.Id//Kullanıcı
                     select new
                     {
                         UserId = u.Id,
                         TotalScore = _globalSettings.NumberFormating(sData.Sum(p => p.Point))
                     }).OrderByDescending(p => p.TotalScore).ToList().FindIndex(p => p.UserId == userId);
            DuelResultRankingModel duelResultRanking = new DuelResultRankingModel()
            {
                RankIndex = r + 1
            };
            if (r == 0 || r == 1) r = 0;
            else r -= 2;
            if (r >= 0) duelResultRanking.ScoreRankings = ScoreRanking(subCategoryId, new PagingModel { PageNumber = r, PageSize = 5 }).Items;
            return duelResultRanking;
        }

        /// <summary>
        /// Kullanıcının facebook arkadaş listesindeki kullanıcı bizde ekli ise o kategorisindeki puan ve sırasını döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="facebookFriendRanking">Facebook Id'leri</param>
        /// <returns>Facebook arkadaşları puan sıralaması</returns>
        public List<ScoreRankingModel> FacebookFriendRanking(string userId, int subCategoryId, List<FacebookFriendRankingModel> facebookFriendRanking)
        {
            if (_contestStartAndEndDate == null) return null;
            var scoreRanking = (from ffr in facebookFriendRanking.AsEnumerable()
                                join u in DbContext.Set<User>() on ffr.FacebokId equals u.FaceBookId into udata
                                from uData1 in udata.DefaultIfEmpty()
                                select new ScoreRankingModel
                                {
                                    UserName = uData1 != null ? uData1.UserName : "",
                                    UserFullName = uData1 != null ? uData1.FullName : "",
                                    UserProfilePicturePath = uData1.ProfilePicturePath,
                                    //////////////////        TotalScore = UserTotalScorePoint(uData1.Id, subCategoryId)
                                }).ToList();
            scoreRanking.Add(UserTotalScore(userId, subCategoryId));//Kendi puanını kaydettik..
            return scoreRanking.OrderByDescending(p => p.TotalScore).ToList();
        }

        /// <summary>
        /// Alt kategoriye göre sıralma listesi getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Sıralama listesi</returns>
        public ServiceModel<ScoreRankingModel> ScoreRanking(int subCategoryId, PagingModel pagingModel)
        {
            if (_contestStartAndEndDate == null) return null;
            return (from s in DbContext.Set<Score>().Select(s => new { s.UserId, s.SubCategoryId, s.ScoreDate, s.Point })
                    where s.SubCategoryId == subCategoryId && s.ScoreDate >= _contestStartAndEndDate.StartDate && s.ScoreDate <= _contestStartAndEndDate.FinishDate
                    join u in DbContext.Set<User>().Select(o => new
                    {
                        o.Id,
                        o.FullName,
                        o.UserName,
                        o.ProfilePicturePath
                    }) on s.UserId equals u.Id
                    group s by new
                    {
                        u.Id,
                        u.FullName,
                        u.UserName,
                        PicturePath = u.ProfilePicturePath
                    } into sData
                    orderby sData.Sum(p => p.Point) descending
                    select new ScoreRankingModel
                    {
                        UserName = sData.Key.UserName,
                        UserFullName = sData.Key.FullName,
                        UserProfilePicturePath = sData.Key.PicturePath,
                        TotalScore = sData.Select(x => x.Point).Sum(p => p)
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Kullanıcının takip ettiği arkadaşlarının sıralamadaki durumunu verir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <param name="paing">Sayfalama</param>
        /// <returns>Sıralama listesi</returns>
        public ServiceModel<ScoreRankingModel> ScoreRankingFollowing(string userId, int subCategoryId, PagingModel pagingModel)
        {
            if (_contestStartAndEndDate == null) return null;
            var quuery = DbContext.Set<User>()
                .Where(u => u.Id == userId)
                .SelectMany(u => u.FollowUpUsers, (f, o) => new ScoreRankingModel
                {
                    UserName = DbContext.Set<User>().Where(p => p.Id == o.FollowedUserId).Select(p => p.UserName).FirstOrDefault(),
                    UserFullName = DbContext.Set<User>().Where(p => p.Id == o.FollowedUserId).Select(p => p.FullName).FirstOrDefault(),
                    UserProfilePicturePath = (from u in DbContext.Set<User>()
                                              where u.Id == o.FollowedUserId
                                              select u.ProfilePicturePath).FirstOrDefault(),
                    TotalScore = DbSet
                     .Where(s => s.UserId == o.FollowedUserId && s.SubCategoryId == subCategoryId && s.ScoreDate >= _contestStartAndEndDate.StartDate && s.ScoreDate <= _contestStartAndEndDate.FinishDate)
                     .Sum(p => p.Point)
                });
            ServiceModel<ScoreRankingModel> serviceModel = new ServiceModel<ScoreRankingModel>
            {
                Count = quuery.Count(),
                PageSize = pagingModel.PageSize,
                PageNumber = pagingModel.PageNumber,
            };
            if (QueryableExtension.IsGetData(serviceModel.Count, pagingModel))
            {
                List<ScoreRankingModel> ScoreRankingModels = quuery
                                       .Skip(pagingModel.PageSize * (pagingModel.PageNumber - 1))
                                       .Take(pagingModel.PageSize)
                                       .ToList();
                ScoreRankingModels.Add(UserTotalScore(userId, subCategoryId));//Kendi puanını kaydettik..
                serviceModel.Items = ScoreRankingModels.OrderByDescending(p => p.TotalScore).AsEnumerable();
            }
            return serviceModel;
        }

        /// <summary>
        /// Kullanıcının belirtilen yarışmadaki sıralam durumunu verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kullanıcının sıralama durumu</returns>
        public ScoreRankingModel UserTotalScore(string userId, int subCategoryId)
        {
            if (_contestStartAndEndDate == null) return null;
            return DbContext.Set<User>()
                .Where(u => u.Id == userId)
                .Select(u1 => new ScoreRankingModel
                {
                    UserName = u1.UserName,
                    UserFullName = u1.FullName,
                    UserProfilePicturePath = u1.ProfilePicturePath,
                    TotalScore = DbSet
                     .Where(s => s.UserId == u1.Id && s.SubCategoryId == subCategoryId && s.ScoreDate >= _contestStartAndEndDate.StartDate && s.ScoreDate <= _contestStartAndEndDate.FinishDate)
                     .Sum(p => p.Point)
                }).FirstOrDefault();
        }

        #endregion Methods
    }
}