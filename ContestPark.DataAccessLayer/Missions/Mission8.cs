using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission8 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission8(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission8;
            }
        }

        /// <summary>
        /// 5 tane soruya doğru cevap vermelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int trueAnswerCount = (from d in DbSet
                                   join di in DbContext.Set<DuelInfo>() on d.DuelId equals di.DuelId
                                   where (d.FounderUserId == userId && di.FounderUserAnswer == (Stylish)di.TrueAnswer)
                                   || (d.CompetitorUserId == userId && di.CompetitorUserAnswer == (Stylish)di.TrueAnswer)
                                   select di.DuelInfoId).Count();
            return trueAnswerCount >= 5;
        }
    }
}