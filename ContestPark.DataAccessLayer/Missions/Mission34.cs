using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission34 : EfEntityRepositoryBase<FollowCategory>, IMission
    {
        public Mission34(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission34;
            }
        }

        /// <summary>
        /// 7 tane durumu beğen.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int postLikeCount = DbSet
                             .Where(p => p.UserId == userId)
                             .Count();
            return postLikeCount >= 7;// 7 durumu beğen
        }
    }
}