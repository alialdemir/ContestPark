using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission18 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission18(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission18;
            }
        }

        /// <summary>
        /// Bir düelloda 100.000 bahis veya üzerinde bir bahislik düello yapmalısın.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int duelBet = DbSet
                            .Where(p => p.FounderUserId == userId || p.CompetitorUserId == userId)
                            .Select(p => p.Cp)
                            .FirstOrDefault();
            return duelBet >= 100000;// 100 bin veya üzeri bahislik düello yapsın
        }
    }
}