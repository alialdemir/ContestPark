using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfPostTypeDal : EfEntityRepositoryBase<PostType>, IPostTypeRepository
    {
        #region Constructors

        public EfPostTypeDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors
    }
}