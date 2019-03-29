using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class CompletedMissionDapperRepository : DapperRepositoryBase<CompletedMission>, ICompletedMissionRepository
    {
        #region Constructor

        public CompletedMissionDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Görevin yapılma durumu
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="mission">Görev tipi</param>
        /// <returns>Görevi yapmış ise true yapmamış ise false</returns>
        public bool MissionStatus(string userId, Entities.Enums.Missions mission)// Görev yapıldı mı onu verir
        {
            string sql = @"SELECT (CASE WHEN EXISTS(
                           SELECT NULL AS [EMPTY] FROM [CompletedMissions] as [cm]
                           where [cm].[UserId]=@UserId and [cm].[MissionId]=@MissionId
                           ) THEN 1 ELSE 0 END) AS[value]";
            return Connection.Query<bool>(sql, new { UserId = userId, MissionId = (int)mission }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının ilgili görevini geri döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="mission">Görev tipi</param>
        /// <returns>Tamamlanan görev entity</returns>
        public CompletedMission UserMission(string userId, Entities.Enums.Missions mission)
        {
            string sql = "SELECT * FROM [CompletedMissions] as [cm] where [cm].[UserId]=@UserId and [cm].[MissionId]=@MissionId";
            return Connection.Query<CompletedMission>(sql, new { UserId = userId, MissionId = (int)mission }).FirstOrDefault();
        }

        #endregion Methods
    }
}