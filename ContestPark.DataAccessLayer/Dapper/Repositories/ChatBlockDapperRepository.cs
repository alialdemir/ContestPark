using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class ChatBlockDapperRepository : DapperRepositoryBase<ChatBlock>, IChatBlockRepository
    {
        #region Constructor

        public ChatBlockDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Sohbet karşılıklı engelleme durumu
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        /// <returns>İki tarafdan biri engellemiş mi true ise engellemiş false ise engellememiş</returns>
        public bool BlockingStatus(string whoId, string whonId)//Kullanıcılar arası sohbet engelleme durumunu kontrol eder
        {
            string sql = @"SELECT
                          (CASE
                          WHEN EXISTS(
                          SELECT NULL AS [EMPTY] FROM [ChatBlocks] as [cb]
                          where ([cb].[WhoId] = @WhoId and [cb].[WhonId] = @WhonId) or ([cb].[WhoId] = @WhonId and [cb].[WhonId] = @WhoId)
                          ) THEN 1 ELSE 0 END) AS[value]";
            return Connection.Query<bool>(sql, new { WhoId = whoId, WhonId = whonId }).FirstOrDefault();
        }

        /// <summary>
        /// Tek taraflı engelleme kontrol eder
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        /// <returns>Engellemiş ise true engellemiş false ise engellememiş</returns>
        public bool UserBlockingStatus(string whoId, string whonId)//Kullanıcılar arası sohbet engelleme durumunu kontrol eder
        {
            string sql = @"SELECT
                          (CASE
                          WHEN EXISTS(
                          SELECT NULL AS[EMPTY] FROM[ChatBlocks] as [cb]
                          where ([cb].[WhoId] = @WhoId and [cb].[WhonId] = @WhonId)
                          ) THEN 1 ELSE 0 END) AS[value]";
            return Connection.Query<bool>(sql, new { WhoId = whoId, WhonId = whonId }).FirstOrDefault();
        }

        /// <summary>
        /// İki kullanıcı arasındaki engellemenin ChatBlockId'sini verir
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public int GetChatBlockIdByWhonIdAndWhoId(string whoId, string whonId)
        {
            string sql = @"SELECT [cb].[ChatBlockId] FROM[ChatBlocks] as [cb]
                           where([cb].[WhoId] = @WhoId and [cb].[WhonId] = @WhonId)";
            return Connection.Query<int>(sql, new { WhoId = whoId, WhonId = whonId }).FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcı Engelle
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public void UserBlocking(string whoId, string whonId)
        {
            Insert(new ChatBlock
            {
                WhoId = whoId,
                WhonId = whonId
            });
        }

        /// <summary>
        /// Kullanıcının engellediği kullanıcılar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Engellenenler listesi</returns>
        public ServiceModel<UserBlockListModel> UserBlockList(string userId, PagingModel pagingModel)
        {
            string sql = @"SELECT [u].[Id] as [UserId], [u].[FullName], [cb].[CreatedDate] as [BlockDate] FROM [ChatBlocks] as [cb]
                           INNER JOIN [AspNetUsers] as [u] on [cb].[WhoId]=[u].[Id]
                           where [cb].[WhoId]=@UserId
                           order by [cb].[CreatedDate] desc";
            return Connection.QueryPaging<UserBlockListModel>(sql, new { UserId = userId }, pagingModel);
        }

        #endregion Methods
    }
}