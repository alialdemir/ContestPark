using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class FollowCategoryService : ServiceBase<FollowCategory>, IFollowCategoryService
    {
        #region Private Variables

        private IFollowCategoryRepository _followCategoryRepository;
        private IOpenSubCategoryService _openSubCategoryService;

        #endregion Private Variables

        #region Constructors

        public FollowCategoryService(IFollowCategoryRepository followCategoryRepository, IOpenSubCategoryService openSubCategoryService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _followCategoryRepository = followCategoryRepository ?? throw new ArgumentNullException(nameof(followCategoryRepository));
            _openSubCategoryService = openSubCategoryService ?? throw new ArgumentNullException(nameof(openSubCategoryService));
        }

        #endregion Constructors

        #region Methods

        public override void Insert(FollowCategory entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowCategoryManager.Insert\"");
            if (IsFollowUpStatus(entity.UserId, entity.SubCategoryId)) Check.BadStatus("ServerMessage_ThisCategoryFollowing");
            if (!_openSubCategoryService.IsSubCategoryOpen(entity.UserId, entity.SubCategoryId)) Check.BadStatus("ServerMessage_ThisCateogoryUnlock");
            base.Insert(entity);
        }

        /// <summary>
        /// Takip ettiği kategoriyi silme
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        public void Delete(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowCategoryManager.Delete\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            int followCategoryId = Find(p => p.UserId == userId && p.SubCategoryId == subCategoryId)
                .Select(p => p.FollowCategoryId)
                .FirstOrDefault();

            Check.IsLessThanZero(followCategoryId, nameof(followCategoryId));

            base.Delete(followCategoryId);
        }

        /// <summary>
        /// SubCategoryId'ye göre o kategoriyi takip eden kullanıcı sayısı
        /// </summary>
        /// <param name="subCategoryId">SubCategoryId</param>
        /// <returns>Kategoriyi takip eden kullanıcı sayısı</returns>
        public int FollowersCount(int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowCategoryManager.FollowersCount\"");
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _followCategoryRepository.FollowersCount(subCategoryId);
        }

        /// <summary>
        /// Kullanıcının takip ettiği kategoriler
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Kullanıcının takip ettiği kategoriler</returns>
        public ServiceModel<SubCategoryModel> FollowingSubCategoryList(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowCategoryManager.FollowingSubCategoryList\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _followCategoryRepository.FollowingSubCategoryList(userId, pagingModel);
        }

        /// <summary>
        /// Takip ettiği kategorileri search sayfasında listeleme
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>search kategori listesi</returns>
        public ServiceModel<SubCategorySearchModel> FollowingSubCategorySearch(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowCategoryManager.FollowingSubCategorySearch\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _followCategoryRepository.FollowingSubCategorySearch(userId, pagingModel);
        }

        /// <summary>
        /// Kullanıcının kategoriyi takip etme durumu
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Alt kategoriyi ise takip ediyor true etmiyorsa ise false</returns>
        public bool IsFollowUpStatus(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FollowCategoryManager.IsFollowUpStatus\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _followCategoryRepository.IsFollowUpStatus(userId, subCategoryId);
        }

        #endregion Methods
    }
}