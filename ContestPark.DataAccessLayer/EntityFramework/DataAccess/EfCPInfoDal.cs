using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfCpInfoDal : EfEntityRepositoryBase<CpInfo>, ICpInfoRepository
    {
        #region Constructors

        public EfCpInfoDal(IDbFactory dbFactory)
            : base(dbFactory)
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
            return (from Cpi in DbSet
                    where Cpi.ChipProcessName == ChipProcessNames.DailyChip
                    join Cp in DbContext.Set<Cp>() on Cpi.CpId equals Cp.CpId
                    where Cp.UserId == userId
                    orderby Cpi.CreatedDate descending
                    select Cpi.CreatedDate)
                    .FirstOrDefault();
        }

        #endregion Methods
    }
}