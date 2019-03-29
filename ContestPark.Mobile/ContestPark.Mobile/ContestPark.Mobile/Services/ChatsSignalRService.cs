using ContestPark.Entities.Models;
using ContestPark.Mobile.Services.Base;
using Microsoft.AspNet.SignalR.Client;
using System;

namespace ContestPark.Mobile.Services
{
    public class ChatsSignalRService : IChatsSignalRService
    {
        #region Private variables

        private readonly ISignalRServiceBase _signalRServiceBase;

        #endregion Private variables

        #region Constructors

        public ChatsSignalRService(ISignalRServiceBase signalRServiceBase)
        {
            _signalRServiceBase = signalRServiceBase;
        }

        #endregion Constructors

        #region Event

        public event EventHandler<ChatHistoryModel> OnDataReceived;

        #endregion Event

        #region Methods

        public void ChatProxy()
        {
            _signalRServiceBase
                .HubProxy
                .On("send", (ChatHistoryModel data) =>
                {
                    if (OnDataReceived != null)
                    {
                        OnDataReceived(this, data);
                    }
                });
        }

        #endregion Methods
    }

    public interface IChatsSignalRService
    {
        void ChatProxy();

        event EventHandler<ChatHistoryModel> OnDataReceived;
    }
}