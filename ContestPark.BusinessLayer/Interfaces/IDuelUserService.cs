using ContestPark.BusinessLayer.Services.ExternalService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IDuelUserService
    {
        Task AddDuelUser(string userId, int subCategoryId, int betAmount);

        Task<List<DuelUser>> GetDuelUsers();

        Task RemoveDuelUser(string userId);
    }
}