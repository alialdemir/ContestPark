using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission28 : EfEntityRepositoryBase<User>, IMission
    {
        public Mission28(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission28;
            }
        }

        /// <summary>
        /// Kapak resmini değiştir.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            return DbSet
                        .Where(u => u.Id == userId && !String.IsNullOrEmpty(u.CoverPicturePath))
                        .Any();
        }
    }
}