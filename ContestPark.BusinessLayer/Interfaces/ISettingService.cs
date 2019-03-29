using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ISettingService : IRepository<Setting>
    {
        void Update(string userId, string value, byte settingTypeId);

        string GetSettingValue(string userId, byte settingTypeId);

        bool IsAddSetting(string userId, int settingTypeId);
    }
}