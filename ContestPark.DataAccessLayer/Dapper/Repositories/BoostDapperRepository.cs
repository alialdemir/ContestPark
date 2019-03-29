using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class BoostDapperRepository : DapperRepositoryBase<Boost>, IBoostRepository
    {
        #region Constructors

        public BoostDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Joker listesi
        /// </summary>
        /// <returns>Joker listesi döndürür</returns>
        public List<BoostModel> BoostList()
        {
            string sql = "select [b].[BoostId], [b].[BoostName], [b].[BoostPicturePath], [b].[Gold] from [Boosts] as [b] where [b].[Visibility]=1";
            return Connection.Query<BoostModel>(sql).ToList();
        }

        /// <summary>
        /// Jokerin altın miktarı
        /// </summary>
        /// <param name="boostId">Joker Id</param>
        /// <returns>Altın miktarı</returns>
        public byte FindBoostGold(int boostId)
        {
            string sql = "select [b].[Gold] from [Boosts] as [b] where [b].[BoostId]=@BoostId";
            return Connection.Query<byte>(sql, new { BoostId = boostId }).FirstOrDefault();
        }

        /// <summary>
        /// Parametreden gelen boost id var mı
        /// </summary>
        /// <param name="boostId">Joker id</param>
        /// <returns>Joker varsa true yoksa false</returns>
        public bool IsBoostIdControl(int boostId)
        {
            string sql = @"SELECT
                          (CASE
                          WHEN EXISTS(
                          SELECT NULL AS [EMPTY] FROM [Boosts] as [b] where [b].[BoostId]=@BoostId
                          ) THEN 1 ELSE 0 END) AS [value]";
            return Connection.Query<bool>(sql, new { BoostId = boostId }).FirstOrDefault();
        }

        #endregion Methods
    }
}