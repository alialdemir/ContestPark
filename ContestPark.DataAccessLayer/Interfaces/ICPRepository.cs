using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ICpRepository : IRepository<Cp>
    {
        int AddChip(string userId, int addedChips, ChipProcessNames chipProcessName);

        int RemoveChip(string userId, int diminishingChips, ChipProcessNames chipProcessName);

        int UserTotalCp(string userId);

        void ChipDistribution(int duelId);

        void ChipDistribution(int duelId, byte FounderScore, byte CompetitorScore);

        bool UserChipEquals(string userId, int amountRequired);

        int AddRandomChip(string userId, DateTime lastDailyChipDateTime);

        void BuyChip(string userId, string productId);
    }
}