using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace ContestPark.BusinessLayer.Services
{
    // TODO: singletion olmamalı login kimin yazdığı belli olmuyor
    public sealed class LoggingService
    {
        private static ILoggerFactory _Factory = null;

        /// <summary>
        /// Log dosyasına erişimi sağlar
        /// </summary>
        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                }
                return _Factory;
            }
            set { _Factory = value; }
        }

        /// <summary>
        /// Loglamada bilgi verme amaçlı logları bu kısım ile loglanır
        /// </summary>
        /// <param name="logMessage">Loga yazılacak mesaj</param>
        /// <param name="filePath">
        /// Çağrılan dosyanın tam yolu
        /// O yola göre CreateLogger oluşturulur
        /// </param>
        /// <returns>Logger</returns>
        public static void LogInformation(string logMessage, [CallerFilePath]string filePath = "")
        {
            CreateLogger(filePath).LogInformation(logMessage);
        }

        /// <summary>
        /// Loglamada önemli olan hataları bu kısım ile loglanır
        /// </summary>
        /// <param name="logMessage">Loga yazılacak mesaj</param>
        /// <param name="filePath">
        /// Çağrılan dosyanın tam yolu
        /// O yola göre CreateLogger oluşturulur
        /// </param>
        /// <returns>Logger</returns>
        public static void LogWarning(string logMessage, [CallerFilePath]string filePath = "")
        {
            CreateLogger(filePath).LogWarning(logMessage);
        }

        /// <summary>
        /// Loglamada önemli olan hataları bu kısım ile loglanır
        /// </summary>
        /// <param name="logMessage">Loga yazılacak mesaj</param>
        /// <param name="filePath">
        /// Çağrılan dosyanın tam yolu
        /// O yola göre CreateLogger oluşturulur
        /// </param>
        /// <returns>Logger</returns>
        public static void LogError(string logMessage, [CallerFilePath]string filePath = "")
        {
            CreateLogger(filePath).LogError(logMessage);
        }

        /// <summary>
        /// Logger oluşturur
        /// </summary>
        /// <param name="filePath">Çağrılan dosyanın tam yolu</param>
        private static ILogger CreateLogger(string filePath)
        {
            string[] split = filePath.Split(new string[] { @"\" }, StringSplitOptions.None);
            string className = split[split.Length - 1].Replace(".cs", "");// Çağrılan classın adı alındı
            return LoggerFactory.CreateLogger(className);
        }
    }
}