using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class SettingService : ServiceBase<Setting>, ISettingService
    {
        #region Private Variables

        private ISettingRepository _settingRepository;

        #endregion Private Variables

        #region Constructors

        public SettingService(ISettingRepository settingRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        #endregion Constructors

        #region Methods

        public override void Insert(Setting entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SettingManager.Insert\"");
            if (!IsAddSetting(entity.UserId, entity.SettingTypeId))
            {
                base.Insert(entity);
            }
        }

        /// <summary>
        /// Ayar güncelle
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="value">Ayar değeri</param>
        /// <param name="settingTypeId">Ayar tipi Id</param>
        public void Update(string userId, string value, byte settingTypeId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SettingManager.Update\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(value, nameof(value));
            Check.IsLessThanZero(settingTypeId, nameof(settingTypeId));

            Setting setting = Find(s => s.UserId == userId && s.SettingTypeId == settingTypeId)
                             .Select(p => p)
                             .FirstOrDefault();
            if (setting == null)
            {
                Insert(new Setting
                {
                    UserId = userId,
                    Value = value,
                    SettingTypeId = settingTypeId
                });
            }
            else
            {
                setting.Value = value;
                this.Update(setting);
            }
        }

        /// <summary>
        /// Kullanıcının ilgili ayardaki seçtiği değeri getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="settingTypeId">Ayar Id</param>
        /// <returns>Ayar değeri</returns>
        public string GetSettingValue(string userId, byte settingTypeId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SettingManager.GetSettingValue\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(settingTypeId, nameof(settingTypeId));

            return _settingRepository.GetSettingValue(userId, settingTypeId);
        }

        /// <summary>
        /// İlgili ayar daha öncden eklenmişmi kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="settingTypeId">Ayar Id</param>
        /// <returns>Ayar ekli olma durumu</returns>
        public bool IsAddSetting(string userId, int settingTypeId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SettingManager.IsAddSetting\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(settingTypeId, nameof(settingTypeId));
            return _settingRepository.IsAddSetting(userId, settingTypeId);
        }

        #endregion Methods
    }
}