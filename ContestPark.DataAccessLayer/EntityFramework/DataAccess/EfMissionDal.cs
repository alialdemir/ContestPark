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
    public class EfMissionDal : EfEntityRepositoryBase<Mission>, IMissionRepository
    {
        #region Constructors

        public EfMissionDal(IDbFactory dbFacotry)
            : base(dbFacotry)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcının görevlerini listeler
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görev listesi</returns>
        public MissionListModel MissionList(string userId, PagingModel pagingModel)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            var query = (from t in DbSet
                         where t.Visibility
                         join tl in DbContext.Set<MissionLang>() on t.MissionId equals tl.MissionId
                         where tl.LanguageId == (byte)language
                         join ct in DbContext.Set<CompletedMission>().Where(p => p.UserId == userId) on t.MissionId equals ct.MissionId into ctData
                         from ct in ctData.DefaultIfEmpty()
                         orderby t.MissionId ascending
                         select new MissionModel
                         {
                             MissionId = t.MissionId,
                             Gold = t.Gold,
                             MissionName = tl.MissionName,
                             MissionDescription = tl.MissionDescription,
                             MissionPicturePath = ct != null ? t.MissionOpeningImage : t.MissionCloseingImage,
                             MissionStatus = ct != null ? ct.MissionComplate : true,//Görev toplandımı görev yapılmış ama toplanmamış olabilir
                             IsCompleteMission = ct != null//Görev yapıldımı
                         });
            MissionListModel missionListModel = new MissionListModel();
            missionListModel.CompleteMissionCount = (byte)query.Where(p => p.IsCompleteMission).Count();

            var serviceModel = query.ToServiceModel(pagingModel);
            missionListModel.Count = serviceModel.Count;
            missionListModel.Items = serviceModel.Items;
            missionListModel.PageNumber = serviceModel.PageNumber;
            missionListModel.PageSize = serviceModel.PageSize;
            return missionListModel;
        }

        /// <summary>
        /// Parametreden gelen görevin verdiği altın miktirı
        /// </summary>
        /// <param name="mission">Görev id</param>
        /// <returns>Görevin altın miktarı</returns>
        public int MissionGold(Entities.Enums.Missions mission)
        {
            return DbSet
                       .Where(p => p.MissionId == (int)mission)
                       .Select(p => p.Gold)
                       .FirstOrDefault();
        }

        #endregion Methods
    }
}