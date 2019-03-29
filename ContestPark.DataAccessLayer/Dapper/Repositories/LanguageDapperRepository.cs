using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class LanguageDapperRepository : DapperRepositoryBase<Language>, ILanguageRepository
    {
        #region Constructor

        public LanguageDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Kullanıcı id'sine ait dil id'sini döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Dil id</returns>
        public byte GetUserLangId(string userId)
        {
            string sql = @"select top(1) [l].[LanguageId] from [Settings] as [s]
                           INNER JOIN [Languages] as [l] on [s].[Value]=[l].[ShortName]
                           where [s].[UserId]=@UserId and [s].[SettingTypeId]=@SettingTypeId";
            return Connection.Query<byte>(sql, new { UserId = userId, SettingTypeId = (byte)SettingTypes.Language }).FirstOrDefault();
        }

        #endregion Methods
    }
}