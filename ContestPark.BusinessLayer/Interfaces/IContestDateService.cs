using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IContestDateService : IRepository<ContestDate>
    {
        ContestStartAndEndDateModel ContestStartAndEndDate();

        void Insert();
    }
}