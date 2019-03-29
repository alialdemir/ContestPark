using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class CpInfoDapperRepository : DapperRepositoryBase<CpInfo>, ICpInfoRepository
    {
        #region Constructors

        public CpInfoDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// En son günlük aldığı altının tarihini verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>En son aldığı günlük altın tarihi</returns>
        public DateTime LastDailyChipDateTime(string userId)
        {
            string sql = @"Select top(1) [cpi].[CreatedDate] from [CpInfoes] as [cpi]
                           INNER JOIN [Cps] as [c] on [cpi].[CpId]= [c].[CpId]
                           where [c].[UserId]=@UserId and [cpi].[ChipProcessName]=@DailyChip
                           order by [cpi].[CreatedDate] desc";
            return Connection.Query<DateTime>(sql, new { UserId = userId, DailyChip = (byte)ChipProcessNames.DailyChip }).FirstOrDefault();
        }

        #endregion Methods
    }
}