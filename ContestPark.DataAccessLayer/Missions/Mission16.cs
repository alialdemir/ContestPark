using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission16 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission16(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission16;
            }
        }

        /// <summary>
        /// Gece 12 den sonra oyuna girmelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            DateTime lastActiveDate = DbSet
                                .Where(p => p.Id == userId)
                                .Select(p => p.LastActiveDate)
                                .FirstOrDefault();
            return lastActiveDate.Hour >= 24 && lastActiveDate.Hour <= 6;// Gece 12 sabah 6 arası oyuna girsin
        }
    }
}