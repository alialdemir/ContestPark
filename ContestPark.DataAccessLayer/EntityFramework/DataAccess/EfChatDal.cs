using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfChatDal : EfEntityRepositoryBase<Chat>, IChatRepository
    {
        #region Constructors

        public EfChatDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// İki kullanıcı arasında mesaj gönderilmiş mi kontrol eder
        /// </summary>
        /// <param name="receiverId">Gönderen kullanıcı Id</param>
        /// <param name="senderId">Alıcı kullanıcı adı</param>
        /// <returns>İki kullanıcı arasındaki mesaj Id</returns>
        public int Conversations(string receiverId, string senderId)
        {
            return DbSet
                    .Where(c => (c.ReceiverId == receiverId && c.SenderId == senderId) || (c.ReceiverId == senderId && c.SenderId == receiverId))
                    .Select(c => c.ChatId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcılar arasındaki mesaj geçmişi
        /// </summary>
        /// <param name="receiverId">Gönderem Kullanıcı Id</param>
        /// <param name="senderId">Alıcı Kullanıcı Id</param>
        /// <returns>Mesaj geçmişi listesi</returns>
        public ServiceModel<ChatHistoryModel> ChatHistory(string receiverId, string senderId, PagingModel pagingModel)//Kullanıcı mesajlarının geçmişi(detayı) son 10
        {
            return (from c in DbSet
                    where (c.ReceiverId == receiverId && c.SenderId == senderId) || (c.ReceiverId == senderId && c.SenderId == receiverId)
                    join cr in DbContext.Set<ChatReply>() on c.ChatId equals cr.ChatId
                    where (cr.UserId == receiverId && cr.ReceiverDeletingStatus) || (cr.UserId == senderId && cr.SenderDeletingStatus)//Sililen mesajlar gelmesin
                    join u in DbContext.Set<User>() on cr.UserId equals u.Id
                    orderby cr.CreatedDate descending
                    select new ChatHistoryModel
                    {
                        Date = cr.CreatedDate,
                        Message = cr.Message,
                        SenderId = cr.UserId,
                        PicturePath = u.ProfilePicturePath,
                        UserName = u.UserName
                    })
                    // .OrderBy(p => p.Date)
                    .ToServiceModel(pagingModel);
            //.Take(10)
            //.ToList()
            //.OrderBy(p => p.Date)
            //.ToList();
        }

        /// <summary>
        /// Kullanıcının mesaj listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kullanıcıya ait mesajlar</returns>
        public ServiceModel<ChatListModel> UserChatList(string userId, PagingModel pagingModel)//Kullanıcı mesaj listesi
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from c in DbSet
                    where (c.ReceiverId == userId || c.SenderId == userId)
                    && ((c.ChatReplys.Where(cr => cr.UserId == userId && cr.ReceiverDeletingStatus)).Any()  //Kullanıcı gönderen ise SenderDeletingStatus kontrol edilir
                    || (c.ChatReplys.Where(cr => cr.UserId != userId && cr.SenderDeletingStatus)).Any())//Kullanıcı alıcı ise ReceiverDeletingStatus e göre mesajı silir silmediği kontrol edilir
                    join u in DbContext.Set<User>() on (c.SenderId == userId ? c.ReceiverId : c.SenderId) equals u.Id
                    let cr = c.ChatReplys.OrderByDescending(p => p.ChatReplyId).FirstOrDefault()
                    let senderUserId = c.SenderId == userId ? c.ReceiverId : c.SenderId
                    select new ChatListModel
                    {
                        UserName = u.UserName,
                        Date = cr.CreatedDate,
                        Message = cr.UserId == userId ? Languages.Turkish.HasFlag(language) ? "Sen : " + cr.Message : "You :" + cr.Message : cr.Message,
                        SenderUserId = senderUserId,
                        UserFullName = u.FullName,
                        UserProfilePicturePath = DbContext
                        .Set<ChatBlock>()
                        .Where(Cp => (Cp.WhoId == userId && Cp.WhonId == senderUserId) || (Cp.WhoId == senderUserId && Cp.WhonId == userId))
                        .Any()
                        ? DefaultImages.DefaultProfilePicture
                        : u.ProfilePicturePath,
                        VisibilityStatus = cr.UserId != userId ? cr.VisibilityStatus : false
                    }).OrderByDescending(p => p.Date)
                      .ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Online olma sırasına göre kullanıcı listesi login olan kullanıcı hariç
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Kullanıcı listesi</returns>
        public ServiceModel<ChatPeopleModel> ChatPeople(string userId, string search, PagingModel pagingModel)
        {
            return (from u in DbContext.Set<User>()
                    let searching = search
                    where u.Status && u.Id != userId && !u.Whons.Any() && !u.Whos.Any() && (u.FullName.ToLower().Contains(searching) || u.UserName.ToLower().Contains(searching) || String.IsNullOrEmpty(searching))//Chat engel listesinde KİM ve KİME olanlardakileri getirdik
                    orderby u.LastActiveDate descending
                    select new ChatPeopleModel
                    {
                        FullName = u.FullName,
                        UserName = u.UserName,
                        LastActiveDate = u.LastActiveDate,
                        ProfilePicturePath = u.ProfilePicturePath,
                        UserId = u.Id
                    }).ToServiceModel(pagingModel);
        }

        #endregion Methods
    }
}