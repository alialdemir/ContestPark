using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class SubCategoryLangService : ServiceBase<SubCategoryLang>, ISubCategoryLangService
    {
        #region Private Variables

        private ISubCategoryLangRepository _subCategoryLangRepository;

        #endregion Private Variables

        #region Constructors

        public SubCategoryLangService(ISubCategoryLangRepository subCategoryLangRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _subCategoryLangRepository = subCategoryLangRepository ?? throw new ArgumentNullException(nameof(subCategoryLangRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının diline göre alt kategori adını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subcategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori adı</returns>
        public string SubCategoryNameByLanguage(string userId, int subcategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SubCategoryLangManager.SubCategoryNameByLanguage\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subcategoryId, nameof(subcategoryId));

            return _subCategoryLangRepository.SubCategoryNameByLanguage(subcategoryId);
        }

        #endregion Methods
    }
}