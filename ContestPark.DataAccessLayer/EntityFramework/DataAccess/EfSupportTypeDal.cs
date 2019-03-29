using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfSupportTypeDal : EfEntityRepositoryBase<SupportType>, ISupportTypeRepository
    {
        #region Constructors

        public EfSupportTypeDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///  Kullanıcı diline göre destek tipleri listesi getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Destek tipi isimleri</returns>
        public List<GetSupportTypeModel> GetSupportTypes()
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return DbContext.Set<SupportTypeLang>()
                  .Where(p => p.LanguageId == (byte)language)
                  .Select(stl => new GetSupportTypeModel
                  {
                      Description = stl.Description,
                      SupportTypeId = stl.SupportTypeId
                  })
                  .ToList();
        }

        #endregion Methods
    }
}