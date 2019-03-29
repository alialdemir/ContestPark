using ContestPark.Entities.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ContestPark.Entities.Helpers
{
    public sealed class Check
    {
        /// <summary>
        /// <param name="entity"></param> null gelirse log atar ve ArgumentNullException fırlatır
        /// </summary>
        /// <param name="entity">Entity nesnesi</param>
        /// <param name="parameterName">Exception da yazılacak parametre adı</param>
        /// <param name="methodName">Çağıran methodun adı</param>
        public static void IsNull(object entity, string parameterName, [CallerMemberName]string methodName = "")
        {
            if (entity == null)
            {
                //        LoggingService.LogError($"{methodName}() {parameterName} null geldi");
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// <param name="id"></param> null yada empty gelirse log atar ve ArgumentNullException fırlatır
        /// </summary>
        /// <param name="id">Entity nesnesi</param>
        /// <param name="parameterName">Exception da yazılacak parametre adı</param>
        /// <param name="methodName">Çağıran methodun adı</param>
        public static void IsNullOrEmpty(string id, string parameterName, [CallerMemberName]string methodName = "")
        {
            if (String.IsNullOrEmpty(id))
            {
                //     LoggingService.LogError($"{methodName}() {parameterName} null geldi");
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// <param name="id"></param> sıfırdan küçük eşit gelirse log atar ve ArgumentNullException fırlatır
        /// </summary>
        /// <param name="id">Entity nesnesi</param>
        /// <param name="parameterName">Exception da yazılacak parametre adı</param>
        /// <param name="methodName">Çağıran methodun adı</param>
        public static void IsLessThanZero(int id, string parameterName, [CallerMemberName]string methodName = "")
        {
            if (id <= 0)
            {
                //   LoggingService.LogError($"{methodName}() {parameterName} sıfırdan küçük geldi");
                throw new InvalidOperationException(parameterName);
            }
        }

        /// <summary>
        /// Kullanıcıya verilecek mesajlar için fırlatılan methodlar
        /// </summary>
        /// <param name="exceptionMessage">Exception da yazılacak mesaj</param>
        public static void BadStatus(string exceptionMessage, string logMessage = "", Dictionary<string, string[]> modelState = null)
        {
            //    if (!String.IsNullOrEmpty(logMessage))
            //LoggingService.LogInformation(logMessage);
            throw new NotificationException(exceptionMessage, modelState);
        }
    }
}