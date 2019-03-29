using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Extensions;
using System;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class LanguageService : ServiceBase<Language>, ILanguageService
    {
        #region Private Variables

        private ILanguageRepository _languageRepository;
        private ISettingService _settingService;
        private IUnitOfWork _unitOfWork;

        #endregion Private Variables

        #region Constructors

        public LanguageService(ILanguageRepository languageRepository,
            ISettingService settingService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
            _settingService = settingService ?? throw new ArgumentNullException(nameof(settingService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcı id'sine ait dil id'sini döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Dil id</returns>
        public Languages GetUserLangId(string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.LanguageManager.GetUserLangId\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            byte result = _languageRepository.GetUserLangId(userId);
            if (result != 0) return (Languages)result;

            IRepository<User> userRepository = _unitOfWork.Repository<User>();//Kullanıcının ülke kodunu aldık ona göre de default dil ataması gerçekleşcek

            string userLanguageCode = userRepository
                                                .Find(p => p.Id == userId)
                                                .Select(p => p.LanguageCode)
                                                .FirstOrDefault();

            if (String.IsNullOrEmpty(userLanguageCode))// Kullanıcılar tablosunda da kullanıcıya ait dil yoksa default olarak ingilizce verdim
            {
                User user = userRepository.Find(x => x.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    user.LanguageCode = Languages.English.ToLanguageCode();
                    userRepository.Update(user);
                }
            }

            userLanguageCode = userLanguageCode.ToLanguageCode();// Eğer kullanıcının kayıt olurken alınan dili bizim deesteklediğimiz dillerde yoksa en-US döner

            _settingService.Insert(new Setting
            {
                SettingTypeId = (int)SettingTypes.Language,
                UserId = userId,
                Value = userLanguageCode
            });

            return userLanguageCode.ToLanguagesEnum();
        }

        #endregion Methods
    }
}