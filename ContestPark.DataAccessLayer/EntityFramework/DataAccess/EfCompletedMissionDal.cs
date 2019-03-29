using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfCompletedMissionDal : EfEntityRepositoryBase<CompletedMission>, ICompletedMissionRepository
    {
        #region Constructors

        public EfCompletedMissionDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Görevin yapılma durumu
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="task">Görev tipi</param>
        /// <returns>Görevi yapmış ise true yapmamış ise false</returns>
        public bool MissionStatus(string userId, Entities.Enums.Missions task)// Görev yapıldı mı onu verir
        {
            return DbSet
                    .Where(p => p.UserId == userId && p.MissionId == (int)task)
                    .Any();
        }

        /// <summary>
        /// Kullanıcının ilgili görevini geri döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="mission">Görev tipi</param>
        /// <returns>Tamamlanan görev entity</returns>
        public CompletedMission UserMission(string userId, Entities.Enums.Missions mission)
        {
            return DbSet
                    .Where(p => p.UserId == userId && p.MissionId == (int)mission)
                    .FirstOrDefault();
        }

        #endregion Methods
    }
}