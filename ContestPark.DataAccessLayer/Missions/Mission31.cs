using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission31 : EfEntityRepositoryBase<FollowCategory>, IMission
    {
        public Mission31(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission31;
            }
        }

        /// <summary>
        /// İlgini çeken 7 kategoriyi takip et.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int followCategoriesCount = DbSet
                                          .Where(p => p.UserId == userId)
                                          .Count();
            return followCategoriesCount >= 7;// 7 Kategori takip et
        }
    }
}