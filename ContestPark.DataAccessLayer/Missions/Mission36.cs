using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission36 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission36(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission36;
            }
        }

        /// <summary>
        /// Facebook hesabınla giriş yapmalısın.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            return DbSet
                        .Where(p => p.Id == userId && p.FaceBookId != null)
                        .Any();
        }
    }
}