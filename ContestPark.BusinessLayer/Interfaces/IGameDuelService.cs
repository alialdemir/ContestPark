using ContestPark.BusinessLayer.Services.ExternalService;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IGameDuelService
    {
        Task AddGameDuel(GameDuel gameDuel);
    }
}