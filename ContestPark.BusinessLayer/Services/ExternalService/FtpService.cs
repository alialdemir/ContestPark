using FluentFTP;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Net;

namespace ContestPark.BusinessLayer.Services
{
    public class FtpService : IFtpService
    {
        #region Private Variables

        private readonly IHostingEnvironment _env;

        #endregion Private Variables

        #region Constructors

        public FtpService(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion Constructors

        #region Private varebles

        /// <summary>
        /// Ftp kullanıcı adı
        /// </summary>
        private string userName = "contestp";

        /// <summary>
        /// Ftp şifre
        /// </summary>
        private string password = "c2Nx22cOf0";

        /// <summary>
        /// Ftp ip
        /// </summary>
        private string uploadurl = "37.230.108.26";

        #endregion Private varebles

        #region Methods

        /// <summary>
        /// Gelen dosya yoluna göre Ftpye dosya ekler
        /// </summary>
        /// <param name="uploadfile">Dosya stream</param>
        /// <param name="pictureName">Ftp yolu</param>
        public void UploadFileOnServerAsync(Stream uploadfile, string pictureName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FtpService.UploadFileOnServerAsync\"");
            if (_env.IsDevelopment()) return;

            using (FtpClient client = new FtpClient()
            {
                Host = uploadurl,
                SslProtocols = System.Security.Authentication.SslProtocols.None,
                Credentials = new NetworkCredential(userName, password)
            })
            {
                try
                {
                    client.Connect();
                    client.Upload(uploadfile, pictureName);
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    LoggingService.LogError($"UploadFileOnServerAsync() hata olutuştu. Hata mesajı: {ex.Message}");
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Ftp üzerinden dosya siler
        /// </summary>
        /// <param name="pictureName">Dosya yolu</param>
        /// <returns>Başarılı ise true değilse false</returns>
        public bool DeleteFileOnServerAsync(string pictureName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.FtpService.DeleteFileOnServerAsync\"");
            if (_env.IsDevelopment()) return true;

            using (FtpClient client = new FtpClient()
            {
                Host = uploadurl,
                SslProtocols = System.Security.Authentication.SslProtocols.None,
                Credentials = new NetworkCredential(userName, password)
            })
            {
                try
                {
                    client.Connect();
                    client.DeleteFile(pictureName);
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    LoggingService.LogError($"DeleteFileOnServerAsync() hata olutuştu. Hata mesajı: {ex.Message}");
                    return false;
                }
                finally
                {
                    client.Dispose();
                }
            }
            return true;
        }

        #endregion Methods
    }

    public interface IFtpService
    {
        bool DeleteFileOnServerAsync(string pictureName);

        void UploadFileOnServerAsync(Stream uploadfile, string pictureName);
    }
}