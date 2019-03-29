using ContestPark.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ContestPark.Entities.ExceptionHandling
{
    public class NotificationException : Exception
    {
        #region Private Variables

        private string _Message;

        #endregion Private Variables

        #region Constructors

        public NotificationException(string exceptionMessage, Dictionary<string, string[]> modelState = null, Exception exception = null)
           : base(exceptionMessage, exception)
        {
            _Message = new MessageModel(exceptionMessage, modelState).ToString();
        }

        #endregion Constructors

        #region Properties

        [JsonProperty(Order = 0)]
        public override string Message
        {
            get { return _Message; }
        }

        #endregion Properties
    }
}