using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission13 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission13(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission13;
            }
        }

        /// <summary>
        /// Sabah saat 6-10 arasında oyuna girmelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            DateTime lastActiveDate = DbSet
                                        .Where(p => p.Id == userId)
                                        .Select(p => p.LastActiveDate)
                                        .FirstOrDefault();
            return lastActiveDate.Hour >= 6 && lastActiveDate.Hour <= 10;// Sabah 6 ile 10 arasında oyuna girsin
        }
    }
}