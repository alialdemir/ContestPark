using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using Microsoft.AspNet.SignalR.Client;
using System;

namespace ContestPark.Mobile.Services
{
    public class DuelSignalRService : IDuelSignalRService
    {
        #region Private variables

        private readonly ISignalRServiceBase _signalRServiceBase;

        #endregion Private variables

        #region Constructors

        public DuelSignalRService(ISignalRServiceBase signalRServiceBase)
        {
            _signalRServiceBase = signalRServiceBase;
        }

        #endregion Constructors

        #region Event

        public event EventHandler<DuelEnterScreenModel> OnDataReceived;

        #endregion Event

        #region Methods

        public void GoToDuelScreenProxy()
        {
            _signalRServiceBase
                            .HubProxy
                            .On("goToDuelScreen", (DuelEnterScreenModel data) => OnDataReceived?.Invoke(this, data));
        }

        #endregion Methods
    }

    public interface IDuelSignalRService
    {
        void GoToDuelScreenProxy();

        event EventHandler<DuelEnterScreenModel> OnDataReceived;
    }
}