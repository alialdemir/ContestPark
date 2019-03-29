using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class ContestDatesService : ServiceBase, IContestDatesService
    {
        #region Constructors

        public ContestDatesService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Yarışmanın başlangıç ve bitiş tarihi
        /// </summary>
        /// <returns>Yarışma başlangıç ve bitiş tarihi</returns>
        public async Task<ContestStartAndEndDateModel> ContestStartAndEndDateAsync()
        {
            var result = await RequestProvider.GetDataAsync<ContestStartAndEndDateModel>($"ContestDates/ContestStartAndEndDate");
            return result.Data;
        }

        #endregion Methods
    }

    public interface IContestDatesService
    {
        Task<ContestStartAndEndDateModel> ContestStartAndEndDateAsync();
    }
}