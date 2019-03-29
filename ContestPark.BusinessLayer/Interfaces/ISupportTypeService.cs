using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface ISupportTypeService : IRepository<SupportType>
    {
        List<GetSupportTypeModel> GetSupportTypes(string userId);
    }
}