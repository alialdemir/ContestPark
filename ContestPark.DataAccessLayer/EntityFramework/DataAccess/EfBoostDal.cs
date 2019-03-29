using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfBoostDal : EfEntityRepositoryBase<Boost>, IBoostRepository
    {
        #region Constructors

        public EfBoostDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Joker listesi
        /// </summary>
        /// <returns>Joker listesi döndürür</returns>
        public List<BoostModel> BoostList()
        {
            return DbSet
                        .Where(p => p.Visibility)
                        .Select(p => new BoostModel
                        {
                            BoostId = p.BoostId,
                            BoostName = p.Name,
                            BoostPicturePath = p.PicturePath,
                            Gold = p.Gold
                        })
                        .ToList();
        }

        /// <summary>
        /// Jokerin altın miktarı
        /// </summary>
        /// <param name="boostId">Joker Id</param>
        /// <returns>Altın miktarı</returns>
        public byte FindBoostGold(int boostId)
        {
            return DbSet
                     .Where(p => p.BoostId == boostId)
                     .Select(p => p.Gold)
                     .FirstOrDefault();
        }

        /// <summary>
        /// Parametreden gelen boost id var mı
        /// </summary>
        /// <param name="boostId">Joker id</param>
        /// <returns>Joker varsa true yoksa false</returns>
        public bool IsBoostIdControl(int boostId)
        {
            return DbSet
                     .Where(p => p.BoostId == boostId)
                     .Any();
        }

        #endregion Methods
    }
}