using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission32 : EfEntityRepositoryBase<Like>, IMission
    {
        public Mission32(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission32;
            }
        }

        /// <summary>
        /// 3 tane durumu beğen.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int postLikeCount = DbSet
                             .Where(p => p.UserId == userId)
                             .Count();
            return postLikeCount >= 3;// 3 durumu beğen
        }
    }
}