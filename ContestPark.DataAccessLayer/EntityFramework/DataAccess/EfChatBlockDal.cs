using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfChatBlockDal : EfEntityRepositoryBase<ChatBlock>, IChatBlockRepository
    {
        #region Constructors

        public EfChatBlockDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Sohbet karşılıklı engelleme durumu
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        /// <returns>İki tarafdan biri engellemiş mi true ise engellemiş false ise engellememiş</returns>
        public bool BlockingStatus(string whoId, string whonId)//Kullanıcılar arası sohbet engelleme durumunu kontrol eder
        {
            return DbSet
                        .Where(Cp => (Cp.WhoId == whoId && Cp.WhonId == whonId) || (Cp.WhoId == whonId && Cp.WhonId == whoId))
                        .Any();//Engellemiş ise true engellememiş ise false
        }

        /// <summary>
        /// Tek taraflı engelleme kontrol eder
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        /// <returns>Engellemiş ise true engellemiş false ise engellememiş</returns>
        public bool UserBlockingStatus(string whoId, string whonId)//Kullanıcılar arası sohbet engelleme durumunu kontrol eder
        {
            return DbSet
                        .Where(Cp => Cp.WhoId == whoId && Cp.WhonId == whonId)
                        .Any();//Engellemiş ise true engellememiş ise false
        }

        /// <summary>
        /// İki kullanıcı arasındaki engellemenin ChatBlockId'sini verir
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public int GetChatBlockIdByWhonIdAndWhoId(string whoId, string whonId)
        {
            return DbSet
                        .Where(p => p.WhonId == whonId && p.WhoId == whoId)
                        .Select(p => p.ChatBlockId)
                        .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcı Engelle
        /// </summary>
        /// <param name="whoId">Engelleyen kullanıcı Id</param>
        /// <param name="whonId">Engellenen kullanıcı Id</param>
        public void UserBlocking(string userId, string whonId)
        {
            Insert(new ChatBlock
            {
                WhoId = userId,
                WhonId = whonId
            });
        }

        /// <summary>
        /// Kullanıcının engellediği kullanıcılar
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Engellenenler listesi</returns>
        public ServiceModel<UserBlockListModel> UserBlockList(string userId, PagingModel pagingModel)
        {
            return (from cb in DbSet
                    where cb.WhoId == userId
                    join u in DbContext.Set<User>() on cb.WhonId equals u.Id
                    select new UserBlockListModel
                    {
                        UserId = u.Id,
                        FullName = u.FullName,
                        BlockDate = cb.CreatedDate,
                    }).ToServiceModel(pagingModel);
        }

        #endregion Methods
    }
}