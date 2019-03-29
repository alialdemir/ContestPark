using ContestPark.Mobile.Events;
using ContestPark.Mobile.Modules;
using Microsoft.AspNet.SignalR.Client;
using Prism.Events;
using System.Threading.Tasks;

namespace ContestPark.Mobile.Services.Base
{
    public class SignalRServiceBase : ISignalRServiceBase
    {
        #region Private variable

        private HubConnection connection;
        private readonly IUserDataModule _userDataModule;

        #endregion Private variable

        #region Constructors

        public SignalRServiceBase(IRequestProvider requestProvider,
                                  IEventAggregator eventAggregator,
                                  IUserDataModule userDataModule)
        {
            _userDataModule = userDataModule;
            Init(requestProvider);
            SignalRConnectEventSubscribe(eventAggregator);
        }

        #endregion Constructors

        #region Properties

        public IHubProxy HubProxy { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// SignalR servisini oluştur
        /// </summary>
        /// <param name="requestProvider"></param>
        private void Init(IRequestProvider requestProvider)
        {
            connection = new HubConnection(requestProvider.WebApiDomain);
            if (_userDataModule.UserModel != null)
            {
                connection.Headers.Add("Authorization", $"bearer {_userDataModule.UserModel.AccessToken}");
                connection.Headers.Add("UserId", _userDataModule.UserModel.UserId);
                connection.Headers.Add("UserName", _userDataModule.UserModel.UserName);
            }
            HubProxy = connection.CreateHubProxy("ContestParkHub");
        }

        /// <summary>
        /// Signalr connection event subscribe
        /// </summary>
        /// <param name="eventAggregator"></param>
        private void SignalRConnectEventSubscribe(IEventAggregator eventAggregator)
        {
            eventAggregator
                          .GetEvent<SignalRConnectEvent>()
                          .Subscribe((isConnect) =>
                          {
                              if (isConnect) Connect();
                              else Disconnect();
                          });
        }

        /// <summary>
        /// SignalR bağlantı aç
        /// </summary>
        public void Connect()
        {
            if (_userDataModule.IsAuthenticated) Task.Run(async () => await connection.Start());
        }

        /// <summary>
        /// SignalR bağlantı kapat
        /// </summary>
        public void Disconnect()
        {
            if (_userDataModule.IsAuthenticated) connection.Stop();
        }

        #endregion Methods
    }

    public interface ISignalRServiceBase
    {
        IHubProxy HubProxy { get; }

        void Connect();

        void Disconnect();
    }
}