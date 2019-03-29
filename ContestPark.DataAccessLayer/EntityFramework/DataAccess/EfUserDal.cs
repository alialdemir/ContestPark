using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfUserDal : EfEntityRepositoryBase<User>, IUserRepository
    {
        #region Constructors

        public EfUserDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Facebook Id'sinin başka kullanıcı tarafından kullanıldımı kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="facebookId">Facebook Id</param>
        /// <returns>Facebook Id başka kullanıcı tarafından kullanılıyor mu? kullanılıyor ise true kullanılmıyor ise false döner</returns>
        public bool IsFacebookRegister(string userId, string facebookId)
        {
            return DbSet
                    .Where(u => u.Id != userId && u.FaceBookId == facebookId)
                    .Any();
        }

        /// <summary>
        /// Kullanıcı adına göre ve kullanıcı aktif ise kullanıcının ad ve soyadını verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Ad soyad</returns>
        public string UserFullName(string userName)
        {
            return DbContext.Set<User>()
                        .Where(p => p.Status && p.UserName == userName)
                        .Select(u => u.FullName)
                        .FirstOrDefault();
        }

        /// <summary>
        /// Facebook bilgisine göre kullanıcı kayıt eder
        /// </summary>
        /// <param name="facebookLogin">Kullanıcının facebook bilgisi</param>
        /// <returns>User entity</returns>
        public User FacebookRegister(FacebookUserViewModel facebookLogin)
        {
            DateTime systemDate = DateTime.Now;
            var user = new User
            {
                UserName = facebookLogin.name,
                FullName = facebookLogin.name,
                Email = facebookLogin.email,
                FaceBookId = facebookLogin.id,
                RegistryDate = systemDate,
                LastActiveDate = systemDate,
                Status = true,
                LanguageCode = facebookLogin.LanguageCode
            };
            this.Insert(user);
            return user;
        }

        /// <summary>
        /// Kullanıcı adına göre en son giriş yaptığı tarihi verir
        /// </summary>
        /// <param name="userName">Kullanıcı Adı</param>
        /// <returns>Son giriş yapma tarihi</returns>
        public DateTime UserLastActiveDate(string userName)
        {
            return DbSet
                .Where(p => p.UserName == userName)
                .Select(p => p.LastActiveDate)
                .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcı adına göre kullanıcı Id'sini verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı Id</returns>
        public string UserId(string userName)
        {
            return DbContext.Set<User>()
                .Where(p => p.UserName == userName)
                .Select(p => p.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Parametreden gelen kullanıcı adı başka kullanıcı tarafından kullanıldı mı kontrol eder
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı adı kullanılmış ise true kullanılmamış ise false döner</returns>
        public bool IsUserNameControl(string userName)
        {
            return DbSet
                .Where(p => p.UserName == userName)
                .Any();
        }

        /// <summary>
        /// Parametreden gelen eposta adresi başka kullanıcı tarafından kullanıldı mı kontrol eder
        /// </summary>
        /// <param name="email">Email adresi</param>
        /// <returns>Eposta adresi başka kullanıcı tarafından kullanılmış ise true kullanılmamış ise false döner</returns>
        public bool IsEmailControl(string email)
        {
            return DbSet
                    .Where(p => p.Email == email)
                    .Any();
        }

        /// <summary>
        /// Kullanıcı Id'sine göre kullanıcıya ait bilgileri getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Kullanıcı bilgileri</returns>
        public UpdateUserInfoModel GetUpdateUserInfo(string userId)
        {
            return DbSet
                .AsEnumerable()
                .Where(p => p.Id == userId)
                .Select(p => new UpdateUserInfoModel
                {
                    Email = p.Email,
                    FullName = p.FullName,
                    UserName = p.UserName,
                    UserCoverPicturePath = p.CoverPicturePath,
                    UserProfilePicturePath = p.ProfilePicturePath
                })
                .FirstOrDefault();
        }

        /// <summary>
        /// Facebook Id taşıma işlemi gerçekleştiril..
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="facebookId">Facebook Id</param>
        public void FacebookRegister(string userId, string facebookId)
        {
            //Facebook Id yeni kullanıcıya eklendi iki kullanıcıda da olmuş oldu giriş yaparken ilk kayıt olanı alır
            User updateUserNewFacebookId = Find(p => p.Id == userId).FirstOrDefault();
            updateUserNewFacebookId.FaceBookId = facebookId;
            base.Update(updateUserNewFacebookId);
        }

        /// <summary>
        /// Kullanıcı aktif olma durumu
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullacı aktif ise true pasif ise false döner</returns>
        public bool IsUserStatus(string userName)
        {
            return DbSet
                    .Where(u => u.UserName == userName)
                    .Select(u => u.Status)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının facebook Id'sine göre kullanıcı entity getirir
        /// </summary>
        /// <param name="facebookId">Facebook Id</param>
        /// <returns>Kullanıcı enity</returns>
        public User FacebookLogin(string facebookId)
        {
            return DbSet
                    .Where(u => u.FaceBookId == facebookId)
                    .Select(u => u)
                    .FirstOrDefault();
        }

        public bool IsUserIdControl(string userId)
        {
            return DbSet
                    .Where(u => u.Id == userId)
                    .Any();
        }

        /// <summary>
        /// Kullanıcı adına ait kullanıcı bilgilerini döndürür
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı</returns>
        public User GetUserByUserName(string userName)
        {
            return DbSet
                  .Where(p => p.UserName == userName)
                  .Select(p => p)
                  .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcı id'ye ait profil resmi
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Profil resim url</returns>
        public string GetProfilePictureByUserId(string userId)
        {
            return DbSet
                        .Where(p => p.Id == userId)
                        .Select(p => p.ProfilePicturePath)
                        .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcı adına göre profilde kullanılan bilgiler
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>User profileP page model </returns>
        public UserProfilePageModel GetUserProfileInfo(string userName)
        {
            return DbSet
                .Where(p => p.NormalizedUserName == userName.ToUpper())
                .Select(p => new UserProfilePageModel
                {
                    CoverPicture = p.CoverPicturePath,
                    UserProfilePicturePath = p.ProfilePicturePath,
                    FullName = p.FullName,
                    UserId = p.Id
                }).FirstOrDefault();
        }

        #endregion Methods
    }
}