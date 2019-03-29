using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission11 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission11(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission11;
            }
        }

        /// <summary>
        /// 100 tane soruya doğru cevap vermelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int trueAnswerCount = (from d in DbSet
                                   join di in DbContext.Set<DuelInfo>() on d.DuelId equals di.DuelId
                                   where (d.FounderUserId == userId && di.FounderUserAnswer == di.TrueAnswer)
                                   || (d.CompetitorUserId == userId && di.CompetitorUserAnswer == di.TrueAnswer)
                                   select di.DuelInfoId).Count();
            return trueAnswerCount >= 100;
        }
    }
}