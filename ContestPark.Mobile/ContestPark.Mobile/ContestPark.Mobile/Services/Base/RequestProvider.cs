using ContestPark.Entities.Models;
using ContestPark.Extensions;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.Modules;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Prism.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services
{
    public class RequestProvider : IRequestProvider
    {
        #region Local variable

        /// <summary>
        /// Servis Api Url
        /// </summary>
        private string apiUrl;

        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ILanguageService _languageService;
        private readonly IPageDialogService _dialogService;
        private readonly IUserDataModule _userDataModule;

        #endregion Local variable

        #region Properties

        /// <summary>
        /// Servis Url
        /// </summary>
        public string WebApiDomain
        {
            get
            {
                return "http://169.254.80.80:5003/"; // "http://localhost:50788/";//  "http://api.contestpark.com/"; //"http://api1.contestpark.com/";// "http://169.254.80.80/ContestPark/"; //  "http://api2.contestpark.com/"; // "http://api.contestpark.com/";// En son bunu yayımladın.
            }
        }

        #endregion Properties

        #region Constructors

        public RequestProvider(ILanguageService languageService,
                               IPageDialogService dialogService,
                               IUserDataModule userDataModule)
        {
            _languageService = languageService;
            _dialogService = dialogService;
            _userDataModule = userDataModule;

            apiUrl = WebApiDomain + "v1/";
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// HttpClient nesnesi verir
        /// </summary>
        /// <returns>new HttpClient</returns>
        public HttpClient GetHttpClient()
        {
            HttpClient httpClient = new HttpClient(new NativeMessageHandler());
            if (_userDataModule.UserModel != null) httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _userDataModule.UserModel.AccessToken);
            if (_languageService != null) httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(_languageService.Language.ToLanguageCode()));
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return httpClient;
        }

        /// <summary>
        /// Servisten get işlemi yapar
        /// </summary>
        /// <typeparam name="TResult">İstenilen tip de değeri döndürür</typeparam>
        /// <param name="url">Servis get Url</param>
        /// <returns>TResult tipinde veri</returns>
        public async Task<ServiceResult<TResult>> GetDataAsync<TResult>(string url)
        {
            return await SendAsync<TResult>(HttpMethod.Get, url);
        }

        /// <summary>
        /// Servisten post işlemi yapar
        /// </summary>
        /// <typeparam name="TResult">İstenilen tip de değeri döndürür</typeparam>
        /// <param name="url">Servis post Url</param>
        /// <param name="data">Content data</param>
        /// <returns>TResult tipinde veri</returns>
        public async Task<ServiceResult<TResult>> PostDataAsync<TResult>(string url, object data = null)
        {
            return await SendAsync<TResult>(HttpMethod.Post, url, data);
        }

        /// <summary>
        /// Servisten delete işlemi yapar
        /// </summary>
        /// <typeparam name="TResult">İstenilen tip de değeri döndürür</typeparam>
        /// <param name="url">Servis delete Url</param>
        /// <param name="data">Content data</param>
        /// <returns>TResult tipinde veri</returns>
        public async Task<ServiceResult<TResult>> DeleteDataAsync<TResult>(string url, object data = null)
        {
            return await SendAsync<TResult>(HttpMethod.Delete, url, data);
        }

        /// <summary>
        /// Servisten httpMethod tipine göre request yapar
        /// </summary>
        /// <typeparam name="TResult">İstenilen tip de değeri döndürür</typeparam>
        /// <param name="url">Servis Url</param>
        /// <param name="data">Content data</param>
        /// <returns>TResult tipinde veri</returns>
        private async Task<ServiceResult<TResult>> SendAsync<TResult>(HttpMethod httpMethod, string url, object data = null)
        {
            ServiceResult<TResult> result = new ServiceResult<TResult>();
            var httpClient = GetHttpClient();
            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, apiUrl + url);
                if (data != null)
                {
                    string content = JsonConvert.SerializeObject(data);
                    httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    string serialized = await response.Content.ReadAsStringAsync();
                    result.Data = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                    result.IsSuccess = true;
                }
                else if (response.StatusCode.HasFlag(HttpStatusCode.Unauthorized))
                {
                    //bool isLogin = await RefleshToken();// Access token süresi dolmuş ise yeniden login yapıyoruz
                    //if (isLogin)
                    //    return await SendAsync<TResult>(httpMethod, url, data);
                    //else result.IsSuccess = false;
                }
                else result.IsSuccess = await ErrorMessageAsync(response);
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached) ShowErrorAlert(ex.Message);
                else ShowErrorAlert(ContestParkResources.GlobalErrorMessage);
            }
            return result;
        }

        /// <summary>
        /// Unauthorized olduğu zaman token yenilek için tekrar login olur
        /// </summary>
        /// <returns></returns>
        private async Task<bool> RefleshToken()
        {
            ////////LoginModel loginModel = new LoginModel
            ////////{
            ////////    UserName = UserDataModule.UserModel.UserName,
            ////////    Password = UserDataModule.UserModel.Password
            ////////};
            ////////if (String.IsNullOrEmpty(loginModel.UserName) || String.IsNullOrEmpty(loginModel.Password)) ContestParkApp.PushMainPage(new SignInPage() { BindingContext = new SignInViewModel() });
            ////////else
            ////////{
            ////////    UserDataModule userDataModule = new UserDataModule();
            ////////    TokenResponseModel userLoginModel = await new ApiServices(this).GetToken(loginModel);
            ////////    if (userLoginModel != null) userDataModule.SetDefaultValues(userLoginModel, loginModel);
            ////////    else
            ////////    {
            ////////        userDataModule.Unauthorized();
            ////////        ContestParkApp.PushMainPage(new SignInPage() { BindingContext = new SignInViewModel() });
            ////////        return false;
            ////////    }
            ////////}
            return true;
        }

        /// <summary>
        /// Hata mesajını deserialize edip alert basar
        /// </summary>
        /// <param name="response">Http response message</param>
        /// <returns>false</returns>
        private async Task<bool> ErrorMessageAsync(HttpResponseMessage response)
        {
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                MessageModel messageModel = JsonConvert.DeserializeObject<MessageModel>(reader.ReadToEnd());
                string message = ContestParkResources.ResourceManager.GetString(messageModel.Message) ?? messageModel.Message;//Çekebiliyorsa resources'den çeker çekemezse mesajı direk basar
                if (!String.IsNullOrEmpty(message)) ShowErrorAlert(message);
            }
            return false;
        }

        private void ShowErrorAlert(string message)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.DisplayAlertAsync(
                                  ContestParkResources.Error,
                                  message,
                                  ContestParkResources.Okay);
            });
        }

        #endregion Methods
    }

    public interface IRequestProvider
    {
        string WebApiDomain { get; }

        HttpClient GetHttpClient();

        Task<ServiceResult<TResult>> GetDataAsync<TResult>(string url);

        Task<ServiceResult<TResult>> PostDataAsync<TResult>(string url, object data = null);

        Task<ServiceResult<TResult>> DeleteDataAsync<TResult>(string url, object data = null);
    }
}