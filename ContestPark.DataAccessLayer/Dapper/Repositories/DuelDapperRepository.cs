using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class DuelDapperRepository : DapperRepositoryBase<Duel>, IDuelRepository
    {
        #region Constructor

        public DuelDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

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
            string sql = @"select top(1)
                            -- Duel info
                            [sc].[PictuePath] as [SubCategoryPicturePath],
                            (select [scl].[SubCategoryName] from [SubCategoryLangs] AS [scl] where [scl].[SubCategoryId]=[d].SubCategoryId and [scl].[LanguageId]=@LanguageId) as [SubCategoryName],
                            [d].[Cp] as [Bet],
                            (case
                            when [d].[FounderUserId]=@UserId then 1 else 0
                            end) as [IsFounder],
                            -- Founder
                            [founderUser].[ProfilePicturePath] as [FounderProfilePicturePath],
                            [founderUser].[FullName] as [FounderFullName],
                            (select sum([s].[Point]) from [Scores] as [s] where [s].[DuelInfoId]=[di].[DuelInfoId] and [s].[UserId]=[d].[FounderUserId]) as [CompetitorScore],
                            -- Competitor
                            [competitorUser].[ProfilePicturePath] as [CompetitorProfilePicturePath],
                            [competitorUser].[FullName] as [CompetitorFullName],
                            (select sum([s].[Point]) from [Scores] as [s] where [s].[DuelInfoId]=[di].[DuelInfoId] and [s].[UserId]=[d].[CompetitorUserId]) as [CompetitorScore]
                            from [DuelInfoes] as [di]
                            INNER JOIN [Duels] as [d] on [di].[DuelId]=[d].[DuelId]
                            INNER JOIN [AspNetUsers] as [founderUser] on [d].[FounderUserId]=[founderUser].[Id]
                            INNER JOIN [AspNetUsers] as [competitorUser] on [d].[CompetitorUserId]= [competitorUser].[Id]
                            INNER JOIN [SubCategories] as [sc] on [d].[SubCategoryId]=[sc].[SubCategoryId]
                            where [di].[DuelId]=@DuelId";
            return Connection.Query<DuelUserInfoModel>(sql, new { UserId = userId, DuelId = duelId, LanguageId = (byte)language }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının ilgili kategorideki tamamlanmış düello sayısı eğer _subCategoryId 0 ise tüm kategorilerdeki oyun sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Tamamlanmış toplam düello sayısı</returns>
        public int GameCount(string userId, int subCategoryId)
        {
            string sql = @"select count([di].[DuelInfoId]) from [Duels] as [d]
                           INNER JOIN [DuelInfoes] as [di] on [d].[DuelId]=[di].[DuelId]
                           where ((@SubCategoryId > 0 and d.SubCategoryId = @SubCategoryId) or (@SubCategoryId <= 0))
                           and (([d].FounderUserId = @UserId and [di].FounderUserAnswer <> @Cevaplamadi) or ([d].CompetitorUserId = @UserId and di.CompetitorUserAnswer <> @Cevaplamadi))";
            return Connection.Query<int>(sql, new { UserId = userId, SubCategoryId = subCategoryId, Cevaplamadi = (byte)Stylish.Cevaplamadi }).FirstOrDefault();
        }

        /// <summary>
        /// Duellonun kurucusu mu yoksa rakip mi kurucu ise true rakip ise false döner
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>kurucusu mu yoksa rakip mi kurucu ise true rakip ise false döner</returns>
        public bool IsFounder(string userId, int duelId)
        {
            string sql = @"SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS[EMPTY] FROM[Duels] as [d]
                           where [d].[DuelId] = @DuelId and [d].[FounderUserId] = @UserId
                           ) THEN 1 ELSE 0 END) AS[value]";
            return Connection.Query<bool>(sql, new { UserId = userId, DuelId = duelId }).FirstOrDefault();//Duellonun kurucusu ise true rakibi ise false döner
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