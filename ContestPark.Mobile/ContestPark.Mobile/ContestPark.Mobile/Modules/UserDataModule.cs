using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using ContestPark.Mobile.Events;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.Services;
using Prism.Events;

namespace ContestPark.Mobile.Modules
{
    public class UserDataModule : IUserDataModule
    {
        #region Private

        private ISQLiteService<UserModel> _sQLiteService;
        private UserModel userModel;
        private readonly ILanguageService _languageService;
        private readonly IEventAggregator _eventAggregator;

        #endregion Private

        #region Constructors

        public UserDataModule(ISQLiteService<UserModel> sQLiteService, ILanguageService languageService, IEventAggregator eventAggregator)
        {
            _sQLiteService = sQLiteService;
            _languageService = languageService;
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Methods

        public void SetDefaultValues(TokenResponseModel userLoginModel, LoginModel loginModel)
        {
            SetDefaultValues(new UserModel
            {
                Password = loginModel.Password,
                UserName = loginModel.UserName,
                AccessToken = userLoginModel.access_token,
                UserCoverPicturePath = userLoginModel.UserCoverPicturePath,
                UserProfilePicturePath = userLoginModel.UserProfilePicturePath,
                FullName = userLoginModel.fullName,
                UserId = userLoginModel.userId
            }, userLoginModel.language.ToLanguagesEnum());
        }

        /// <summary>
        /// Kullanıcı giriş yapınca yapılacak tüm işlemler
        /// </summary>
        public void SetDefaultValues(UserModel userModule, Languages language)
        {
            userModel = userModule;
            _languageService.SetUserLanguage(language.ToLanguageCode());
            EntityInsert(userModel);
            SignalrConnectStatus(true);
        }

        /// <summary>
        /// Kullanıcı çıkış yapınca yapılacak tüm işlemler
        /// </summary>
        public void Unauthorized()
        {
            if (_sQLiteService.Count > 0)
            {
                SignalrConnectStatus(false);
                _sQLiteService.Delete(_sQLiteService.First().UserName);
                userModel = null;
            }
        }

        /// <summary>
        /// Signalr connection açıp kapatmaya yarar
        /// </summary>
        /// <param name="isConnect"></param>
        private void SignalrConnectStatus(bool isConnect)
        {
            _eventAggregator.GetEvent<SignalRConnectEvent>().Publish(isConnect);
        }

        #endregion Methods

        #region Properties

        public static string SQLiteDbName { get; set; } = "ContestPark.db3";

        public bool IsLoaded
        {
            get
            {
                if (_sQLiteService.Count <= 0)
                    return false;
                SetDefaultValues(_sQLiteService.First(), _languageService.Language);
                // TODO: 401 olup olmadığını kontrol etmek için api'ye request attıyoruz fakat daha kolay yolla yapılmalı token express süresini kontrol etmek gibi
                var isUnauthorized = true;// new AccountService(new RequestProvider()).TokenControl().Result;
                return isUnauthorized;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this != null;
            }
        }

        public UserModel UserModel { get { return userModel; } }

        #endregion Properties

        #region SqlLite Methods

        /// <summary>
        /// Eğer kayıt ekli ise o kayıtı günceller değilse yeni kayıt ekler
        /// </summary>
        /// <param name="userModule">Kullanıcı login bilgileri</param>
        public void EntityInsert(UserModel userModule)
        {
            if (userModule == null) return;
            var userModuleLocal = _sQLiteService.First();
            if (userModuleLocal != null) _sQLiteService.Update(userModule);
            else _sQLiteService.Insert(userModule);
        }

        #endregion SqlLite Methods
    }

    public interface IUserDataModule
    {
        void SetDefaultValues(TokenResponseModel userLoginModel, LoginModel loginModel);

        void SetDefaultValues(UserModel userModule, Languages language);

        void Unauthorized();

        void EntityInsert(UserModel userModule);

        bool IsLoaded { get; }
        bool IsAuthenticated { get; }
        UserModel UserModel { get; }
    }
}