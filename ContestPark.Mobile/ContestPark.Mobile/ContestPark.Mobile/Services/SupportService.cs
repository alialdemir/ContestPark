using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class SupportService : ServiceBase, ISupportService
    {
        #region Constructors

        public SupportService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Destek tiplerinin listesini döndürür
        /// </summary>
        /// <returns>Destek tip listesi</returns>
        public async Task<IEnumerable<GetSupportTypeModel>> GetSupportTypesAsync()
        {
            var result = await RequestProvider.GetDataAsync<IEnumerable<GetSupportTypeModel>>("Supports");
            return result.Data;
        }

        /// <summary>
        /// Destek isteği gönder
        /// </summary>
        /// <param name="model">Destek mesajı ve destek tipi</param>
        /// <returns>Başarılı mesajı</returns>
        public async Task<bool> InsertSupportAsync(SupportModel supportModel)
        {
            var result = await RequestProvider.PostDataAsync<string>("Supports", supportModel);
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface ISupportService
    {
        Task<IEnumerable<GetSupportTypeModel>> GetSupportTypesAsync();

        Task<bool> InsertSupportAsync(SupportModel supportModel);
    }
}