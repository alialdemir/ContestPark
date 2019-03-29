using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfSupportDal : EfEntityRepositoryBase<Support>, ISupportRepository
    {
        #region Constructors

        public EfSupportDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors
    }
}