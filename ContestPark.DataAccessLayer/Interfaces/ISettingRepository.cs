using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>
    {
        string GetSettingValue(string userId, byte settingTypeId);

        bool IsAddSetting(string userId, int settingTypeId);
    }
}