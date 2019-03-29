using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfFollowDal : EfEntityRepositoryBase<Follow>, IFollowRepository
    {
        #region Constructors

        public EfFollowDal(IDbFactory dbFacotry)
            : base(dbFacotry)
        {
        }

        #endregion Constructors

        #region Methods

        //string followUpUserId = takip eden , string followedUserId  = takip edilen
        /// <summary>
        /// İki kullanıcı arasındaki takip etme durumunu kaldırır
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        public void Delete(string followUpUserId, string followedUserId)
        {
            int executeRowCount = DbContext.Database.ExecuteSqlCommand($"delete from Follows where FollowUpUserId={followUpUserId} and FollowedUserId={followedUserId}");
            if (executeRowCount < 0) BadStatus("ServerMessages_thisUserFollowing");
        }

        /// summary>
        /// Parametreden gelen kullanıcı id'ye ait takip eden kullanıcı sayısını dönrürür
        /// </summary>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <returns>Takip eden kullanıcı sayısı</returns>
        public int FollowersCount(string followedUserId)
        {
            return DbSet
                    .Where(p => p.FollowedUserId == followedUserId)
                    .Count();
        }

        /// <summary>
        /// Parametreden gelen kullanıcı id'ye ait takip ettiği kullanıcıların sayısını döndürür
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <returns>Takip ettiği kullanıcı sayısı</returns>
        public int FollowUpCount(string followUpUserId)//takip ettiklerinin sayısı
        {
            return DbSet
                    .Where(p => p.FollowUpUserId == followUpUserId)
                    .Count();
        }

        /// <summary>
        /// Parametreden gelen kullanıcının takip ettiği kullanıcı listesi
        /// </summary>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>kullanıcı listesi</returns>
        public ServiceModel<ChatPeopleModel> FollowingChatList(string followedUserId, string search, PagingModel pagingModel)
        {
            return (from f in DbSet
                    where f.FollowUpUserId == followedUserId
                    join followUpUser in DbContext.Set<User>() on f.FollowedUserId equals followUpUser.Id
                    let searching = search
                    where (followUpUser.FullName.ToLower().Contains(searching) || followUpUser.UserName.ToLower().Contains(searching) || String.IsNullOrEmpty(searching))
                    orderby f.FollowId descending
                    select new ChatPeopleModel
                    {
                        LastActiveDate = followUpUser.LastActiveDate,
                        ProfilePicturePath = followUpUser.ProfilePicturePath,
                        UserId = followUpUser.Id,
                        FullName = followUpUser.FullName,
                        UserName = followUpUser.UserName
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Kullanıcının Takip edenler(Takipçileri)
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takipçi listesi</returns>
        public ServiceModel<ListOfFollowerModel> Followers(string followedUserId, string followUpUserId, PagingModel pagingModel)
        {
            return (from f in DbSet
                    from followUpUser in DbContext.Set<User>()
                    where f.FollowedUserId == followedUserId && followUpUser.Id == f.FollowUpUserId
                    orderby f.FollowId descending
                    select new ListOfFollowerModel
                    {
                        FollowUpUserId = f.FollowUpUserId,
                        FullName = followUpUser.FullName,
                        UserName = followUpUser.UserName,
                        ProfilePicturePath = followUpUser.ProfilePicturePath,
                        IsFollowUpStatus = DbSet
                                          .Where(p => p.FollowUpUserId == followUpUserId && p.FollowedUserId == f.FollowUpUserId)
                                          .Any()//Karşı tarafı takip etme durmunu kontrol ettik
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Kullanıcının takip ettikleri
        /// </summary>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="paging">Sayfalama 10 ve katları olmalı</param>
        /// <returns>Takip ettiklerinin listesi</returns>
        public ServiceModel<ListOfFollowerModel> Following(string followedUserId, string followUpUserId, PagingModel pagingModel)
        {
            return (from f in DbSet
                    where f.FollowUpUserId == followedUserId
                    join followUpUser in DbContext.Set<User>() on f.FollowedUserId equals followUpUser.Id
                    orderby f.FollowId descending
                    select new ListOfFollowerModel
                    {
                        ProfilePicturePath = followUpUser.ProfilePicturePath,
                        FollowUpUserId = followUpUser.Id,
                        FullName = followUpUser.FullName,
                        UserName = followUpUser.UserName,
                        IsFollowUpStatus = DbSet
                                          .Where(p => p.FollowUpUserId == followUpUserId && p.FollowedUserId == f.FollowedUserId)
                                          .Any(),//Karşı tarafı takip etme durmunu kontrol ettik
                    }).ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Takip etme durumu kontrol eder takip ediyorsa true etmiyorsa false döner
        /// </summary>
        /// <param name="followUpUserId">Takip eden kullanıcı id</param>
        /// <param name="followedUserId">Takip edilen kullanıcı id</param>
        /// <returns>Takip ediyorsa true etmiyorsa false</returns>
        public bool IsFollowUpStatus(string followUpUserId, string followedUserId)
        {
            return DbSet
                    .Where(p => p.FollowUpUserId == followUpUserId && p.FollowedUserId == followedUserId)
                    .Any();
        }

        #endregion Methods
    }
}