using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ICpService : IRepository<Cp>
    {
        void AddChip(string userId, int addedChips, ChipProcessNames chipProcessName);

        void RemoveChip(string userId, int diminishingChips, ChipProcessNames chipProcessName);

        int UserTotalCp(string userId);

        void ChipDistribution(int duelId);

        void ChipDistribution(int duelId, byte FounderScore, byte CompetitorScore);

        bool UserChipEquals(string userId, int amountRequired);

        int AddRandomChip(string userId);

        void BuyChip(string userId, string productId);
    }
}