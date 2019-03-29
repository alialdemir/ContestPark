using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Services
{
    public class SupportTypeService : ServiceBase<SupportType>, ISupportTypeService
    {
        #region Private Variables

        private ISupportTypeRepository _supportTypeRepository;

        #endregion Private Variables

        #region Constructors

        public SupportTypeService(ISupportTypeRepository SupportTypeRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _supportTypeRepository = SupportTypeRepository ?? throw new ArgumentNullException(nameof(SupportTypeRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///  Kullanıcı diline göre destek tipleri listesi getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Destek tipi isimleri</returns>
        public List<GetSupportTypeModel> GetSupportTypes(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SupportTypeManager.GetSupportTypes\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _supportTypeRepository.GetSupportTypes();
        }

        #endregion Methods
    }
}