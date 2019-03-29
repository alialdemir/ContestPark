using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission27 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission27(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission27;
            }
        }

        /// <summary>
        /// Profil resmini değiştir.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            return DbSet
                .Where(p => p.Id == userId && !String.IsNullOrEmpty(p.ProfilePicturePath))
                .Any();
        }
    }
}