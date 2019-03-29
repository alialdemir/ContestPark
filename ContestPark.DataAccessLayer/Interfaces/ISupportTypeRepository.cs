using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface ISupportTypeRepository : IRepository<SupportType>
    {
        List<GetSupportTypeModel> GetSupportTypes();
    }
}