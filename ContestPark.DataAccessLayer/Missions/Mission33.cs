using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission33 : EfEntityRepositoryBase<Like>, IMission
    {
        public Mission33(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission33;
            }
        }

        /// <summary>
        /// 5 tane durumu beğen.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int postLikeCount = DbSet
                                 .Where(p => p.UserId == userId)
                                 .Count();
            return postLikeCount >= 5;// 5 durumu beğen
        }
    }
}