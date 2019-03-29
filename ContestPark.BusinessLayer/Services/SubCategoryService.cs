using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class SubCategoryService : ServiceBase<SubCategory>, ISubCategoryService
    {
        #region Private Variables

        private ISubCategoryRepository _subCategoryRepository;

        #endregion Private Variables

        #region Constructors

        public SubCategoryService(ISubCategoryRepository SubCategoryRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _subCategoryRepository = SubCategoryRepository ?? throw new ArgumentNullException(nameof(SubCategoryRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// SubCategoryId ye göre alt kategori resmi verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Picture Path</returns>
        public string SubCategoryPicture(int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SubCategoryManager.SubCategoryPicture\"");
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _subCategoryRepository.SubCategoryPicture(subCategoryId);
        }

        /// <summary>
        /// Alt kategorinin fiyatını verir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori fiyatı</returns>
        public int SubCategoryPrice(int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SubCategoryManager.SubCategoryPrice\"");
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _subCategoryRepository.SubCategoryPrice(subCategoryId);
        }

        #endregion Methods
    }
}