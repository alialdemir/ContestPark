using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IAskedQuestionService : IRepository<AskedQuestion>
    {
        Task DeleteAsync(string userId, int subCategoryId);
    }
}