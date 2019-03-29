using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContestPark.BusinessLayer.Services
{
    public class PostService : ServiceBase<Post>, IPostService
    {
        #region Private Variables

        private IPostRepository _Post;

        #endregion Private Variables

        #region Constructors

        public PostService(IPostRepository Post, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _Post = Post ?? throw new ArgumentNullException(nameof(Post));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kim ne yapıyor güncelle
        /// </summary>
        /// <param name="entity">Post entity</param>
        public override void Update(Post entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.Update\"");
            Check.IsNull(entity, nameof(entity));

            Post PostUpdate = Find(x => x.PostId == entity.PostId).FirstOrDefault();
            Check.IsNull(PostUpdate, nameof(PostUpdate));

            PostUpdate.PostTypeId = entity.PostTypeId;
            PostUpdate.UserId = entity.UserId;
            PostUpdate.ContestantId = entity.ContestantId;
            PostUpdate.Date = entity.Date;
            PostUpdate.ContestantContestStatus = true;
            base.Update(entity);
        }

        /// <summary>
        /// Kullanıcının kim ne yapıyor listesi
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="paging">Sayfalama</param>
        /// <returns>Kim ne yapıyor listesi</returns>
        public ServiceModel<PostListModel> PostList(string userId, string userName, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.PostList\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _Post.PostList(userId, userName, pagingModel);
        }

        /// <summary>
        /// Kim ne yapıyor ekle
        /// </summary>
        /// <param name="entity">Kim ne yapıyor entity</param>
        public override void Insert(Post entity)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.Insert\"");
            Check.IsNull(entity, nameof(entity));

            entity.Date = DateTime.Now;
            base.Insert(entity);
        }

        /// <summary>
        /// Kim ne yapıyoru tek post olarak göstermek için
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="PostId">kim ne yapıyor Id</param>
        /// <returns>Kim ne yapıyor modeli</returns>
        public PostListModel Post(string userId, int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.Post\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _Post.Post(userId, PostId);
        }

        /// <summary>
        /// kim ne yapıyor Id'sine göre kullanıı Id'sini verir
        /// </summary>
        /// <param name="PostId">kim ne yapıyor Id</param>
        /// <returns>Kim ne yapıyor bilgi modeli</returns>
        public PostInfoModel GetUserId(int PostId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.GetUserId\"");
            Check.IsLessThanZero(PostId, nameof(PostId));

            return _Post.GetUserId(PostId);
        }

        /// <summary>
        /// Kullanıcı yarışmacıyı takip ediyor mu kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="contestantId">Rakip kullanıcı id</param>
        /// <returns>Takip ediyor ise true etmiyor ise false</returns>
        public bool IsFollowControl(string userId, string contestantId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.IsFollowControl\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(contestantId, nameof(contestantId));

            return _Post.IsFollowControl(userId, contestantId);
        }

        /// <summary>
        /// Yarışmalarının giriş ekranında kim ne yapıyordan gelen datalardan yarışma sonuçlarını listelemek için kullandım
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="subCategoryId">Alt kategori id</param>
        /// <param name="paging">Sayfalama için 4 ve katları olmalı</param>
        /// <returns>Yarışma kategorisine ait kim yapıyor listesi</returns>
        public ServiceModel<PostListModel> ContestEnterScreen(string userId, int subCategoryId, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.ContestEnterScreen\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(subCategoryId, nameof(subCategoryId));

            return _Post.ContestEnterScreen(userId, subCategoryId, pagingModel);
        }

        /// <summary>
        /// Kim yapıyor güncelle
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="contestantId">Rakip kullanıcı id</param>
        /// <param name="duelId">Düello id</param>
        public void Update(string userId, string contestantId, int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.Update\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(contestantId, nameof(contestantId));
            Check.IsLessThanZero(duelId, nameof(duelId));

            _Post.Update(userId, contestantId, duelId);
        }

        /// <summary>
        /// PostTypeId'ye göre profil veya kapak resmimlerinin PostId'lerini verir
        /// </summary>
        /// <param name="pictuteId">Resim Id</param>
        /// <returns>PostTypeId list</returns>
        public void DeleteAllPostByPictureId(int pictuteId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.PostManager.DeleteAllPostByPictureId\"");
            Check.IsLessThanZero(pictuteId, nameof(pictuteId));
            // TODO: optimize edilmeli resim id'leri tek seferde gönderilip silinmeli
            IEnumerable<int> PostIdList = _Post.GetPostsByPictureId(pictuteId);
            if (PostIdList != null)
            {
                foreach (int PostId in PostIdList) Delete(PostId);
            }
        }

        #endregion Methods

        //#region Dispose
        //protected override void DisposeCore()
        //{
        //    if (_Post != null)
        //    {
        //        _Post.Dispose();
        //        _Post = null;
        //    }
        //    base.DisposeCore();
        //}
        //#endregion
    }
}