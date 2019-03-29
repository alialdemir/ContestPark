using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfDuelDal : EfEntityRepositoryBase<Duel>, IDuelRepository
    {
        #region Constructors

        public EfDuelDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının düello bilgisini verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>Düello bilgisi</returns>
        public DuelUserInfoModel DuelUserInfo(string userId, int duelId)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from di in DbContext.Set<DuelInfo>().AsEnumerable()
                    where di.DuelId == duelId
                    join d in DbSet on di.DuelId equals d.DuelId
                    //Kurucu
                    join founderUser in DbContext.Set<User>() on d.FounderUserId equals founderUser.Id
                    join founderScorePoint in DbContext.Set<Score>() on di.DuelInfoId equals founderScorePoint.DuelInfoId into founderScoreData
                    //Rakip
                    join competitorUser in DbContext.Set<User>() on d.CompetitorUserId equals competitorUser.Id
                    join competitorScorePoint in DbContext.Set<Score>() on di.DuelInfoId equals competitorScorePoint.DuelInfoId into competitorScorePointData
                    // Kategori bilgisi
                    join sc in DbContext.Set<SubCategory>() on d.SubCategoryId equals sc.SubCategoryId
                    join scl in DbContext.Set<SubCategoryLang>() on sc.SubCategoryId equals scl.SubCategoryId
                    where scl.LanguageId == (byte)language
                    select new DuelUserInfoModel
                    {
                        // Duello Bahis Miktarı
                        SubCategoryPicturePath = sc.PictuePath,
                        SubCategoryName = scl.SubCategoryName,
                        Bet = d.Cp,
                        IsFounder = d.FounderUserId == userId ? true : false,
                        //Kurucu
                        FounderProfilePicturePath = founderUser.ProfilePicturePath,
                        FounderFullName = founderUser.FullName,
                        FounderScore = (byte)founderScoreData.Sum(p => p.Point),
                        //Rakip
                        CompetitorProfilePicturePath = competitorUser.ProfilePicturePath,
                        CompetitorFullName = competitorUser.FullName,
                        CompetitorScore = (byte)competitorScorePointData.Sum(p => p.Point)
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının ilgili kategorideki tamamlanmış düello sayısı eğer _subCategoryId 0 ise tüm kategorilerdeki oyun sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Tamamlanmış toplam düello sayısı</returns>
        public int GameCount(string userId, int subCategoryId)
        {
            return (from d in DbSet
                    where (subCategoryId > 0 && d.SubCategoryId == subCategoryId) || (subCategoryId <= 0)
                    join di in DbContext.Set<DuelInfo>() on d.DuelId equals di.DuelId
                    where (d.FounderUserId == userId && di.FounderUserAnswer != Stylish.Cevaplamadi) || (d.CompetitorUserId == userId && di.CompetitorUserAnswer != Stylish.Cevaplamadi)
                    select di.DuelInfoId)
                    .Count();
        }

        /// <summary>
        /// Duellonun kurucusu mu yoksa rakip mi kurucu ise true rakip ise false döner
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>kurucusu mu yoksa rakip mi kurucu ise true rakip ise false döner</returns>
        public bool IsFounder(string userId, int duelId)
        {
            return DbSet
                .Where(p => p.DuelId == duelId && p.FounderUserId == userId)
                .Any();//Duellonun kurucusu ise true rakibi ise false döner
        }

        ///////////// <summary>
        ///////////// Düello ekle
        ///////////// </summary>
        ///////////// <param name="entity">Düello entity</param>
        //////////public override void Insert(Duel entity)//Düello ekleme
        //////////{
        //////////    entity.SubCategoryId = _subCategoryId;
        //////////    base.Insert(entity);
        //////////}
        ///////////// <summary>
        ///////////// Düello güncelle
        ///////////// </summary>
        ///////////// <param name="entity">Düello enity</param>
        //////////public override void Update(Duel entity)//düello güncelleme
        //////////{
        //////////    entity.SubCategoryId = _subCategoryId;
        //////////    base.Update(entity);
        //////////}

        #endregion Methods
    }
}