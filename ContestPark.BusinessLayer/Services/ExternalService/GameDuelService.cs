using ContestPark.BusinessLayer.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Services.ExternalService
{
    public class GameDuelService : IGameDuelService
    {
        #region Private Variables

        private const string _duelKey = "GameDuelServiceKey";
        private readonly IDistributedCache _distributedCache;

        #endregion Private Variables

        #region Constructor

        public GameDuelService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Add new game duel
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="subCategoryId">Sub category id</param>
        /// <param name="betAmount">bet</param>
        /// <returns></returns>
        public async Task AddGameDuel(GameDuel gameDuel)
        {
            List<GameDuel> GameDuels = await GetGameDuels();
            GameDuels.Add(gameDuel);
            await SetGameDuel(GameDuels);
        }

        private async Task SetGameDuel(List<GameDuel> GameDuels)
        {
            string GameDuelJson = JsonConvert.SerializeObject(GameDuels);

            string existingTime = DateTime.UtcNow.ToString();
            var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));// Not: 15 dk bir sıfırlanıyor o sürede oynayan olursa bug oluşacak test et
            option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            await _distributedCache.SetStringAsync(_duelKey, GameDuelJson, option);
        }

        private async Task<List<GameDuel>> GetGameDuels()
        {
            string GameDuelJson = await _distributedCache.GetStringAsync(_duelKey);
            if (String.IsNullOrEmpty(GameDuelJson))
                return new List<GameDuel>();

            return JsonConvert.DeserializeObject<List<GameDuel>>(GameDuelJson);
        }

        #endregion Methods
    }

    public partial class GameDuel
    {
        public int GameDuelId { get; set; }
        public string FounderUserId { get; set; }
        public string FounderConnectionId { get; set; }
        public string CompetitorUserId { get; set; }
        public string CompetitorConnectionId { get; set; }
        public int DuelId { get; set; }
    }
}