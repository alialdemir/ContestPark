using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission24 : EfEntityRepositoryBase<Follow>, IMission
    {
        public Mission24(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission24;
            }
        }

        /// <summary>
        /// Aynı anda 5 kişiyi takip et.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int followCount = DbSet
                              .Where(p => p.FollowUpUserId == userId)
                              .Count();
            return followCount >= 5;
        }
    }
}