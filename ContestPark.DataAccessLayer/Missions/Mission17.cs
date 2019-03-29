using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission17 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission17(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission17;
            }
        }

        /// <summary>
        /// Bir düelloda 50.000 bahis veya üzerinde bir bahislik düello yapmalısın.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int duelBet = DbSet
                         .Where(p => p.FounderUserId == userId || p.CompetitorUserId == userId)
                         .Select(p => p.Cp)
                         .FirstOrDefault();
            return duelBet >= 50000;// 50 bin veya üzeri bahislik düello yapsın
        }
    }
}