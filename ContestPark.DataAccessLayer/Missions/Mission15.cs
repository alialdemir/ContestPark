using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission15 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission15(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission15;
            }
        }

        /// <summary>
        /// Akşam saat 6-12 arasında oyuna girmelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            DateTime lastActiveDate = DbSet
                                        .Where(p => p.Id == userId)
                                        .Select(p => p.LastActiveDate)
                                        .FirstOrDefault();
            return lastActiveDate.Hour >= 18 && lastActiveDate.Hour <= 24;// Akşam 18 gece 12 arası oyuna girsin
        }
    }
}