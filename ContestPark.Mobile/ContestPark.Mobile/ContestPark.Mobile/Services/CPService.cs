using ContestPark.Mobile.Services.Base;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class CpService : ServiceBase, ICpService
    {
        #region Constructors

        public CpService(IRequestProvider requestProvider) : base(requestProvider)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Login olan Kullanıcıya göre toplam altın miktarını verir
        /// </summary>
        /// <returns>Toplam altın miktarı</returns>
        public async Task<int> UserTotalCp()
        {
            var result = await RequestProvider.GetDataAsync<int>("Cps");
            return result.Data;
        }

        /// <summary>
        /// Eğer günlük chip almamış ise 10000-20000 arası rasgele chip ekler almış ise 0 döndürür
        /// </summary>
        /// <returns>Eklenen altın miktarı</returns>
        public async Task<int> AddRandomChip()
        {
            var result = await RequestProvider.PostDataAsync<int>("Cps");
            return result.Data;
        }

        /// <summary>
        /// Store'dan altın satın alınca alınan altını ekliyor
        /// </summary>
        /// <param name="productId">satın alınan ürün Id'si</param>
        public async Task<bool> BuyChip(string productId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Cps/{productId}/BuyChip");
            return result.IsSuccess;
        }

        /// <summary>
        /// Kendi duello Id'sine göre duellodaki bahis miktarı kadar Kullanıcılardan bahisi düþer
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        public async Task<bool> ChipDistribution(int duelId)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Cps/{duelId}/ChipDistribution");
            return result.IsSuccess;
        }

        /// <summary>
        /// Bu method bot ile oynanan duellolarda düello sonunda kurucu kazanırrsa bahisini alması için
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        /// <param name="FounderScore">Düello kurucusu puanı</param>
        /// <param name="CompetitorScore">Rakip oyuncu puanı</param>
        public async Task<bool> ChipDistribution(int duelId, byte founderScore, byte competitorScore)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Cps/{duelId}/ChipDistribution/{founderScore}/{competitorScore}");
            return result.IsSuccess;
        }

        #endregion Methods
    }

    public interface ICpService
    {
        Task<int> UserTotalCp();

        Task<int> AddRandomChip();

        Task<bool> BuyChip(string productId);

        Task<bool> ChipDistribution(int duelId);

        Task<bool> ChipDistribution(int duelId, byte founderScore, byte competitorScore);
    }
}