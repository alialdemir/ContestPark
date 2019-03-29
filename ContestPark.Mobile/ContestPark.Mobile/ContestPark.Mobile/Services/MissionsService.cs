using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class MissionsService : ServiceBase, IMissionsService
    {
        #region Constructors

        public MissionsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Görev listesi
        /// </summary>
        /// <returns>Görevler</returns>
        public async Task<MissionListModel> MissionListAsync(PagingModel pagingModel)
        {
            var result = await RequestProvider.GetDataAsync<MissionListModel>($"Missions{pagingModel.ToString()}");
            return result.Data;
        }

        /// <summary>
        /// Görevin altınını topla
        /// </summary>
        /// <param name="mission">Görev Id</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> TakesMissionGoldAsync(Missions mission)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Missions/{(byte)mission}");
            return result.IsSuccess;
        }

        /// <summary>
        /// Görevi tamamla
        /// </summary>
        /// <param name="mission">Görev Id</param>
        public async Task<bool> MissionCompleteAsync(Missions mission)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Missions/{(byte)mission}/MissionComplete");
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface IMissionsService
    {
        Task<MissionListModel> MissionListAsync(PagingModel pagingModel);

        Task<bool> TakesMissionGoldAsync(Missions mission);

        Task<bool> MissionCompleteAsync(Missions mission);
    }
}