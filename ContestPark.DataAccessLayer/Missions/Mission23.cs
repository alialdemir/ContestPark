using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission23 : EfEntityRepositoryBase<Duel>, IMission// Oynadığın herhangi bir kategori sıralamasında 1. 2. veya 3. olmalısın
    {
        public Mission23(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission23;
            }
        }

        /// <summary>
        /// Herhangi bir kategori sıralamasında 1. olmalısın.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            var _contestStartAndEndDate = DbContext
                                              .Set<ContestDate>()
                                              .OrderByDescending(cd => cd.ContestDateId)
                                              .Select(cd => new
                                              {
                                                  StartDate = cd.StartDate,
                                                  FinishDate = cd.FinishDate
                                              })
                                              .Take(1)
                                              .FirstOrDefault();
            List<int> subCategoryIdList = DbSet//Kullanıcının oynadığı kategoriler
                .Where(p => p.FounderUserId == userId || p.CompetitorUserId == userId)
                .GroupBy(p => p.SubCategoryId)
                .Select(p => p.Key)
                .ToList();
            foreach (int subCategoryId in subCategoryIdList)
            {
                bool is1st2ng3rd = (from s in DbContext.Set<Score>()
                                    where s.SubCategoryId == subCategoryId && s.ScoreDate >= _contestStartAndEndDate.StartDate && s.ScoreDate <= _contestStartAndEndDate.FinishDate
                                    group s by s.UserId into sData
                                    join u in DbContext.Set<User>() on sData.Key equals u.Id
                                    orderby sData.Sum(p => p.Point) descending
                                    select u.Id)
                                            .Skip(0)
                                            .Take(3)
                                            .ToList()
                                            .Where(p => p == userId)
                                            .Any();
                if (is1st2ng3rd) return is1st2ng3rd;
            }
            return false;
        }
    }
}