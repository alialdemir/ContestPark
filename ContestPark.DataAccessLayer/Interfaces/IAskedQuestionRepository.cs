using ContestPark.DataAccessLayer.Tables;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IAskedQuestionRepository : IRepository<AskedQuestion>
    {
        Task DeleteAsync(string userId, int subCategoryId);
    }
}