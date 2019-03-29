using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfContestDateDal : EfEntityRepositoryBase<ContestDate>, IContestDateRepository
    {
        #region Constructors

        public EfContestDateDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Yarışmaların başlangıç ve bitiş tarihini verir
        /// </summary>
        /// <returns>Yarışma başlama ve bitiş tarihi</returns>
        public ContestStartAndEndDateModel ContestStartAndEndDate()
        {
            return DbSet
                .OrderByDescending(cd => cd.ContestDateId)
                .Select(cd => new ContestStartAndEndDateModel()
                {
                    StartDate = cd.StartDate,
                    FinishDate = cd.FinishDate
                })
                .FirstOrDefault();
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
            base.Insert(contestDate);
        }

        #endregion Methods
    }
}