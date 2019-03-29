using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services.Base;
using Newtonsoft.Json;
using Prism.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        #region Private

        private readonly IPageDialogService _dialogService;
        private readonly IUserDataModule _userDataModule;
        private string _errorMessage;

        #endregion Private

        #region Constructors

        public AccountService(IRequestProvider requestProvider,
                              IPageDialogService dialogService,
                              IUserDataModule userDataModule) : base(requestProvider)
        {
            _dialogService = dialogService;
            _userDataModule = userDataModule;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Kullanıcı adına göre profil bilgileri getirir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kullanıcı profil bilgisi</returns>
        public async Task<UserProfilePageModel> UserProfileAsync(string userName)
        {
            var result = await RequestProvider.PostDataAsync<UserProfilePageModel>($"Account/UserProfile?userName={userName}");
            return result.Data;
        }

        /// <summary>
        /// Token süresi hala geçirlimi kontrol etmek kullanılır
        /// </summary>
        /// <returns>Token geçerli ise true değilse false</returns>
        public async Task<bool> TokenControl()
        {
            var result = await RequestProvider.GetDataAsync<string>("Account/TokenControl");
            return result.IsSuccess;
        }

        /// <summary>
        /// Kullanıcının bilgilerini güncellenemesini sağlar
        /// </summary>
        /// <param name="updateUserInfo">Kullanıcı bilgisi</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> SetUpdateUserInfoAsync(UpdateUserInfoModel updateUserInfo)
        {
            var result = await RequestProvider.PostDataAsync<object>("Account/SetUpdateUserInfo", updateUserInfo);
            return result.IsSuccess;
        }

        /// <summary>
        /// Kullanıcı ayarlarları sayfasında kullanıcı bilgisini getirir
        /// </summary>
        /// <returns>Kullanıcı bilgisi</returns>
        public async Task<UpdateUserInfoModel> GetUpdateUserInfoAsync()
        {
            var result = await RequestProvider.GetDataAsync<UpdateUserInfoModel>("Account/GetUpdateUserInfo");
            return result.Data;
        }

        /// <summary>
        /// Üye olma methodu eğer üye işlemi gerçekleşirse servisten Ok mesajı geliyor değilse hata mesağı döner
        /// </summary>
        /// <param name="registerBindingModel">Üye bilgisi</param>
        /// <returns>Hata mesajı</returns>
        public async Task<bool> RegisterAsync(RegisterBindingModel registerBindingModel)
        {
            var result = await RequestProvider.PostDataAsync<string>("Account/Register", registerBindingModel);
            return result.IsSuccess;
        }

        /// <summary>
        /// Şifremi unuttum
        /// </summary>
        /// <param name="userInfo">Kullanıcı adı veya eposta adresi bilgisi</param>
        /// <returns></returns>
        public async Task<bool> ForgotYourPasswordAsync(string userInfo)
        {
            var result = await RequestProvider.PostDataAsync<string>($"Account/ForgotYourPassword?userInfo={userInfo}");
            return result.IsSuccess;
        }

        /// <summary>
        /// Servisden token alır ve döndürür
        /// </summary>
        /// <param name="loginModel">Kullanıcı adı ve şifre</param>
        /// <returns>Token bilgisi</returns>
        public async Task<TokenResponseModel> GetToken(LoginModel loginModel)
        {
            using (var httpClient = RequestProvider.GetHttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                var from = new Dictionary<string, string>
                {
                    {"username",loginModel.UserName },
                    {"password",loginModel.Password },
                };
                var response = await httpClient.PostAsync(
                    RequestProvider.WebApiDomain + "token",
                    new FormUrlEncodedContent(from));

                string resultContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    TokenResponseModel userLoginModel = JsonConvert.DeserializeObject<TokenResponseModel>(resultContent);
                    return userLoginModel;
                }
                else _errorMessage = resultContent;
            }
            return null;
        }

        /// <summary>
        /// Kullanıcı login olma
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="password">Şifre</param>
        /// <returns></returns>
        public async Task<bool> LoginProgcessAsync(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
            {
                await _dialogService.DisplayAlertAsync(
                      ContestParkResources.Error,
                      ContestParkResources.enterYourUsernamEAndPassword,
                      ContestParkResources.Okay);
                return false;
            }
            try
            {
                var userLoginModel = await GetToken(loginModel);
                if (userLoginModel != null)
                {
                    _userDataModule.SetDefaultValues(userLoginModel, loginModel);
                    return true;
                }
                else
                {
                    MessageModel messageModel = JsonConvert.DeserializeObject<MessageModel>(_errorMessage);
                    await _dialogService.DisplayAlertAsync(
                             ContestParkResources.Error,
                             ContestParkResources.ResourceManager.GetString(messageModel.Message),
                             ContestParkResources.Okay);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                await _dialogService.DisplayAlertAsync(
                      ContestParkResources.Error,
                      ex.Message,
                      //ContestParkResources.GlobalErrorMessage,
                      ContestParkResources.Okay);
            }
            return false;
        }

        #endregion Methods
    }

    public interface IAccountService
    {
        Task<bool> LoginProgcessAsync(LoginModel loginModel);

        Task<TokenResponseModel> GetToken(LoginModel loginModel);

        Task<bool> ForgotYourPasswordAsync(string userInfo);

        Task<bool> RegisterAsync(RegisterBindingModel registerBindingModel);

        Task<UpdateUserInfoModel> GetUpdateUserInfoAsync();

        Task<bool> SetUpdateUserInfoAsync(UpdateUserInfoModel updateUserInfo);

        Task<bool> TokenControl();

        Task<UserProfilePageModel> UserProfileAsync(string userName);
    }
}