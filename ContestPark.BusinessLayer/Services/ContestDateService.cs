using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class ContestDateService : ServiceBase<ContestDate>, IContestDateService
    {
        #region Private Variables

        private IContestDateRepository _contestDateRepository;

        #endregion Private Variables

        #region Constructors

        public ContestDateService(IContestDateRepository contestDateRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _contestDateRepository = contestDateRepository ?? throw new ArgumentNullException(nameof(contestDateRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Yarışmaların başlangıç ve bitiş tarihini verir
        /// </summary>
        /// <returns>Yarışma başlama ve bitiş tarihi</returns>
        public ContestStartAndEndDateModel ContestStartAndEndDate()
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ContestDateManager.ContestStartAndEndDate\"");
            return _contestDateRepository.ContestStartAndEndDate();
        }

        /// <summary>
        /// Yarışma tarihi ekle
        /// </summary>
        public void Insert()
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.ContestDateManager.Insert\"");
            _contestDateRepository.Insert();
        }

        #endregion Methods
    }
}