using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IContestDateRepository : IRepository<ContestDate>
    {
        ContestStartAndEndDateModel ContestStartAndEndDate();

        void Insert();
    }
}