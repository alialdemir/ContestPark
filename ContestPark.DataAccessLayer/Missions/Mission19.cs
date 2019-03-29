using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission19 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission19(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission19;
            }
        }

        /// <summary>
        /// Bir düelloda 200.000 bahis veya üzerinde bir bahislik düello yapmalısın.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int duelBet = DbSet
                                .Where(p => p.FounderUserId == userId || p.CompetitorUserId == userId)
                                .Select(p => p.Cp)
                                .FirstOrDefault();
            return duelBet >= 200000;// 200 bin veya üzeri bahislik düello yapsın
        }
    }
}