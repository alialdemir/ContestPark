using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class OpenSubCategoryService : ServiceBase<OpenSubCategory>, IOpenSubCategoryService
    {
        #region Private Variables

        private IOpenSubCategoryRepository _openSubCategoryRepository;
        private ISubCategoryService _subCategoryService;
        private ICpService _CpService;

        #endregion Private Variables

        #region Constructors

        public OpenSubCategoryService(IOpenSubCategoryRepository openSubCategoryRepository, ISubCategoryService subCategoryService, ICpService CpService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _openSubCategoryRepository = openSubCategoryRepository ?? throw new ArgumentNullException(nameof(openSubCategoryRepository));
            _subCategoryService = subCategoryService ?? throw new ArgumentNullException(nameof(subCategoryService));
            _CpService = CpService ?? throw new ArgumentNullException(nameof(CpService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Alt kategori açma
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategori açma durumu</returns>
        public bool OpenCategory(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.OpenSubCategoryManager.OpenCategory\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            if (IsSubCategoryOpen(userId, subCategoryId)) Check.BadStatus("ServerMessage_ThisCategoryOpen");

            int subCategoryPrice = _subCategoryService.SubCategoryPrice(subCategoryId);
            bool chipEquals = _CpService.UserChipEquals(userId, subCategoryPrice);//kullanıcının chip istenilen chipden fazla mı? fazla ise true değilse false
            if (!chipEquals) return false;// BadStatus("serverMessages.youDoNotHaveASufficientAmountOfGoldToOpenThisCategory");

            Insert(new OpenSubCategory
            {
                SubCategoryId = subCategoryId,
                UserId = userId
            });
            _CpService.RemoveChip(userId, subCategoryPrice, ChipProcessNames.OpenCategory);
            return true;
        }

        /// <summary>
        /// Kullanıcı id'ye göre ilgili alt kategori açık mı kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <returns>Açık ise true değilse false</returns>
        public bool IsSubCategoryOpen(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.MissionManager.OpenSubCategoryManager.IsSubCategoryOpen\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _openSubCategoryRepository.IsSubCategoryOpen(userId, subCategoryId);
        }

        #endregion Methods
    }
}