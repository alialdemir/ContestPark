using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission14 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission14(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission14;
            }
        }

        /// <summary>
        /// Sabah 10 akşam 6 arasında oyuna girmelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            DateTime lastActiveDate = DbSet
                                        .Where(p => p.Id == userId)
                                        .Select(p => p.LastActiveDate)
                                        .FirstOrDefault();
            return lastActiveDate.Hour >= 10 && lastActiveDate.Hour <= 18;// Sabah 10 akşam 18 arası oyuuna girsin
        }
    }
}