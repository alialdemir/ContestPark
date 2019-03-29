using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class ContestDateDapperRepository : DapperRepositoryBase<ContestDate>, IContestDateRepository
    {
        #region Constructor

        public ContestDateDapperRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Yarışmaların başlangıç ve bitiş tarihini verir
        /// </summary>
        /// <returns>Yarışma başlama ve bitiş tarihi</returns>
        public ContestStartAndEndDateModel ContestStartAndEndDate()
        {
            string sql = "select [cd].[StartDate], [cd].[FinishDate] from [ContestDates] as [cd] order by [cd].[ContestDateId] desc";
            return Connection.Query<ContestStartAndEndDateModel>(sql).FirstOrDefault();
        }

        /// <summary>
        /// Yarışma tarihi ekle
        /// </summary>
        public void Insert()
        {
            DateTime systemDate = DateTime.Now;
            ContestDate contestDate = new ContestDate()
            {
                StartDate = systemDate,
                FinishDate = systemDate.AddHours(168),
            };
            contestDate.StartDate = contestDate.StartDate.AddMilliseconds(-2);
            contestDate.FinishDate = contestDate.FinishDate.AddMilliseconds(-2);
            Insert(contestDate);
        }

        #endregion Methods
    }
}