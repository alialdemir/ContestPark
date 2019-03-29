using ContestPark.BusinessLayer.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Services.ExternalService
{
    public class DuelUserService : IDuelUserService
    {
        #region Private Variables

        private const string _duelKey = "DuelUserServiceKey";
        private readonly IDistributedCache _distributedCache;

        #endregion Private Variables

        #region Constructor

        public DuelUserService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Add new duel user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="subCategoryId">Sub category id</param>
        /// <param name="betAmount">bet</param>
        /// <returns></returns>
        public async Task AddDuelUser(string userId, int subCategoryId, int betAmount)
        {
            List<DuelUser> duelUsers = await GetDuelUsers();
            duelUsers.Add(new DuelUser
            {
                SubCategoryId = subCategoryId,
                BetAmount = betAmount,
                UserId = userId
            });
            await SetDuelUser(duelUsers);
        }

        /// <summary>
        /// set duel users
        /// </summary>
        /// <param name="duelUsers"></param>
        /// <returns></returns>
        private async Task SetDuelUser(List<DuelUser> duelUsers)
        {
            string duelUserJson = JsonConvert.SerializeObject(duelUsers);

            string existingTime = DateTime.UtcNow.ToString();
            var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));// Not: 15 dk bir sıfırlanıyor o sürede oynayan olursa bug oluşacak test et
            option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            await _distributedCache.SetStringAsync(_duelKey, duelUserJson, option);
        }

        /// <summary>
        /// all users at redis
        /// </summary>
        /// <returns>Duel user list</returns>
        public async Task<List<DuelUser>> GetDuelUsers()
        {
            string duelUserJson = await _distributedCache.GetStringAsync(_duelKey);
            if (String.IsNullOrEmpty(duelUserJson))
                return new List<DuelUser>();

            return JsonConvert.DeserializeObject<List<DuelUser>>(duelUserJson);
        }

        /// <summary>
        /// Rmove redis user by user id
        /// </summary>
        /// <param name="userId">Removed user id</param>
        public async Task RemoveDuelUser(string userId)
        {
            List<DuelUser> duelUsers = await GetDuelUsers();
            duelUsers.RemoveAll(p => p.UserId == userId);
            await SetDuelUser(duelUsers);
        }

        #endregion Methods
    }

    public class DuelUser
    {
        public int DuelUserId { get; set; }
        public string UserId { get; set; }
        public int SubCategoryId { get; set; }
        public int BetAmount { get; set; }
    }
}