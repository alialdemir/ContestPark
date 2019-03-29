using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        #region Private Variables

        private ICategoryRepository _CategoryRepository;

        #endregion Private Variables

        #region Constructors

        public CategoryService(
            ICategoryRepository CategoryRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _CategoryRepository = CategoryRepository ?? throw new ArgumentNullException(nameof(CategoryRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kategorilerin listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kategori listesi</returns>
        public ServiceModel<CategoryModel> CategoryList(string userId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CategoryManager.CategoryList\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _CategoryRepository.CategoryList(userId, pagingModel);
        }

        /// <summary>
        /// Alt kategori Id'ye göre kategori listesi getirir 0 ise tüm kategoriler gelir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Aranan kategorilerin listesi</returns>
        public ServiceModel<SubCategorySearchModel> SearchCategory(string userId, int subCategoryId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.CategoryManager.SearchCategory\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _CategoryRepository.SearchCategory(userId, subCategoryId, pagingModel);
        }

        #endregion Methods
    }
}