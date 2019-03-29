using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System;
using System.IO;

namespace ContestPark.BusinessLayer.Services
{
    public class EmailService : IEmailSender
    {
        #region Private Variables

        private readonly IHostingEnvironment _env;

        #endregion Private Variables

        #region Constructors

        public EmailService(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Eposta gönderme
        /// </summary>
        /// <param name="whom">Alıcı eposta adresi</param>
        /// <param name="subject">Konu</param>
        /// <param name="message">Mesaj</param>
        /// <returns>İşlem başarılı ise true değilse false</returns>
        public bool EmailSend(string whom, string subject, string message)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.EmailService.EmailSend\"");
            if (_env.IsDevelopment()) return true;

            var mimeMessage = new MimeMessage()
            {
                Subject = subject,
                From =
                {
                    new MailboxAddress("", "info@contestpark.com")
                },
                To =
                {
                    new MailboxAddress("", whom)
                }
            };
            // Html template
            var builder = new BodyBuilder()
            {
                HtmlBody = File.ReadAllText("EmailTheme.html").Replace("{Message}", message)
            };
            mimeMessage.Body = builder.ToMessageBody();
            using (SmtpClient client = new SmtpClient())
            {
                try
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("mail.contestpark.com", 587, false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("info@contestpark.com", "6lrZPs8V");// TODO: şifreyi şifreleyip yaz

                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    LoggingService.LogError($"Error ContestPark.BusinessLayer.Services.EmailService.EmailSend exception:{ex.Message}");
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

    public interface IEmailSender
    {
        bool EmailSend(string whom, string subject, string message);
    }
}