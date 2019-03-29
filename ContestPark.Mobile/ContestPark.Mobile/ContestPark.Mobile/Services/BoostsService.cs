using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class BoostsService : ServiceBase, IBoostsService
    {
        #region Constructors

        public BoostsService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Joker listesi getirme
        /// </summary>
        public async Task<IEnumerable<BoostModel>> BoostListAsync()
        {
            var result = await RequestProvider.GetDataAsync<IEnumerable<BoostModel>>("Boosts");
            return result.Data;
        }

        // v1/Boosts/{boost}/Buy
        /// <summary>
        /// Joker alma
        /// </summary>
        /// <param name="boost">Boost Id</param>
        /// <returns>İşlem başarılı ise true değilse false</returns>
        public async Task<bool> BuyAsync(int boost)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Boosts/{boost}/Buy");
            return result.IsSuccess;
        }

        /// <summary>
        /// Jokeri geri satma
        /// </summary>
        /// <param name="boost">Boost Id</param>
        /// <returns>İşlem başarılı ise true değilse false</returns>
        public async Task<bool> SellAsync(int boost)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Boosts/{boost}/Sell");
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface IBoostsService
    {
        Task<IEnumerable<BoostModel>> BoostListAsync();

        Task<bool> BuyAsync(int boost);

        Task<bool> SellAsync(int boost);
    }
}