using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;

namespace ContestPark.BusinessLayer.Services
{
    public class PostTypeService : ServiceBase<PostType>, IPostTypeService
    {
        #region Private Variables

        //   private readonly IPostTypeRepository _PostType;

        #endregion Private Variables

        #region Constructors

        public PostTypeService(/*IPostTypeRepository PostType,*/ IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            //  _PostType = PostType;
        }

        #endregion Constructors
    }
}