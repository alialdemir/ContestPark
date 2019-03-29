using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class MissionDapperRepository : DapperRepositoryBase<Mission>, IMissionRepository
    {
        #region Constructor

        public MissionDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Kullanıcının görevlerini listeler
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görev listesi</returns>
        public MissionListModel MissionList(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            string sql = @"select
                           [m].[MissionId],
                           [m].[Gold],
                           [ml].[MissionName],
                           [ml].[MissionDescription],
                           CASE
                           WHEN [cm].[CompletedMissionId]  IS NOT NULL
                           then [m].[MissionOpeningImage]
                           else [m].[MissionCloseingImage]
                           END AS [MissionPicturePath]
                           CASE
                           WHEN [cm].[CompletedMissionId]  IS NOT NULL
                           then [cm].[MissionComplate]
                           else 1
                           END AS [MissionStatus],
                           CASE
                           WHEN [cm].[CompletedMissionId]  IS NOT NULL
                           then 1
                           else 0
                           END AS [IsCompleteMission]
                           from [Missions] as [m]
                           INNER JOIN [MissionLangs] as [ml] on [m].[MissionId]=[ml].[MissionId]
                           LEFT JOIN [CompletedMissions] as [cm] on [m].[MissionId]=[cm].[MissionId] and [cm].[UserId]=@UserId
                           where [m].[Visibility]=1 and [ml].[LanguageId]=@LanguageId
                           order by [m].[MissionId] asc";
            string percent = "SELECT TOP (100) PERCENT " + sql.Substring(6),
                   query1 = $@"SELECT COUNT(*) FROM ({percent}) AS c;";

            MissionListModel missionListModel = new MissionListModel();
            missionListModel.CompleteMissionCount = Connection.Query<byte>(query1, new { UserId = userId, LanguageId = (byte)language }).FirstOrDefault();

            var serviceModel = Connection.QueryPaging<MissionModel>(sql, new { UserId = userId, LanguageId = (byte)language }, pagingModel);
            missionListModel.Count = serviceModel.Count;
            missionListModel.Items = serviceModel.Items;
            missionListModel.PageNumber = serviceModel.PageNumber;
            missionListModel.PageSize = serviceModel.PageSize;
            return missionListModel;
        }

        /// <summary>
        /// Parametreden gelen görevin verdiği altın miktirı
        /// </summary>
        /// <param name="mission">Görev id</param>
        /// <returns>Görevin altın miktarı</returns>
        public int MissionGold(Entities.Enums.Missions mission)
        {
            string sql = "select top(1) [m].[Gold] from [Missions] as [m] where [m].[MissionId]=@MissionId";
            return Connection.Query<int>(sql, new { MissionId = (int)mission }).FirstOrDefault();
        }

        #endregion Methods
    }
}