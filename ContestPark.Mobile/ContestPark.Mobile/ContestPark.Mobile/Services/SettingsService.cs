using ContestPark.Entities.Enums;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class SettingsService : ServiceBase, ISettingsService
    {
        #region Constructors

        public SettingsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Login olan kullanıcının parametreden gelen ayarın değerini döndürü
        /// </summary>
        /// <param name="settingTypeId">Ayar Id</param>
        /// <returns>Ayarýn değeri</returns>
        public async Task<string> GetSettingValueAsync(byte settingTypeId)
        {
            var result = await RequestProvider.GetDataAsync<string>($"Settings/{settingTypeId}");
            return result.Data;
        }

        /// <summary>
        /// Ayar değeri günceller
        /// </summary>
        /// <param name="value">Ayar değeri</param>
        /// <param name="settingTypeId">Ayar Id</param>
        public async Task<bool> UpdateSettingsAsync(string value, SettingTypes settingTypeId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Settings/{(byte)settingTypeId}", value);
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface ISettingsService
    {
        Task<string> GetSettingValueAsync(byte settingTypeId);

        Task<bool> UpdateSettingsAsync(string value, SettingTypes settingTypeId);
    }
}