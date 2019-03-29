﻿using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission6 : EfEntityRepositoryBase<Duel>, IMission
    {
        public Mission6(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission6;
            }
        }

        /// <summary>
        /// 500 düello tamamlamalısın.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            int completeDuelCount = (from d in DbSet
                                     join di in DbContext.Set<DuelInfo>() on d.DuelId equals di.DuelId
                                     where (d.FounderUserId == userId && di.FounderUserAnswer != Stylish.Cevaplamadi) || (d.CompetitorUserId == userId && di.CompetitorUserAnswer != Stylish.Cevaplamadi)
                                     select di.DuelInfoId).Count();
            return completeDuelCount >= 500;
        }
    }
}