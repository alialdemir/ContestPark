using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission12 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission12(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission12;
            }
        }

        /// <summary>
        /// Bir düelloda son soruya doğru cevap vermelisin.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            return (from d in DbSet
                    join di in DbContext.Set<DuelInfo>() on d.DuelId equals di.DuelId into diData
                    where (d.FounderUserId == userId && diData.OrderByDescending(p => p.DuelInfoId).FirstOrDefault().FounderUserAnswer == (Stylish)diData.OrderByDescending(p => p.DuelInfoId).FirstOrDefault().TrueAnswer)
                    || (d.CompetitorUserId == userId && diData.OrderByDescending(p => p.DuelInfoId).FirstOrDefault().CompetitorUserAnswer == (Stylish)diData.OrderByDescending(p => p.DuelInfoId).FirstOrDefault().TrueAnswer)
                    select diData.FirstOrDefault().DuelInfoId).Any();
        }
    }
}