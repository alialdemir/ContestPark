using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class SupportService : ServiceBase<Support>, ISupportService
    {
        #region Private Variables

        private ISupportRepository _supportRepository;
        private IEmailSender _emailSender;

        #endregion Private Variables

        #region Constructors

        public SupportService(ISupportRepository SupportRepository, IEmailSender emailSender, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _supportRepository = SupportRepository ?? throw new ArgumentNullException(nameof(SupportRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Destek ekle
        /// </summary>
        /// <param name="support">Destek entity</param>
        public override void Insert(Support support)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.SupportManager.Insert\"");
            Check.IsNull(support, nameof(support));

            base.Insert(support);
            _emailSender.EmailSend("aldemirali93@gmail.com", "ContestPark destek talebi var", "Destek talebini kontrol et. " + support.Message);
        }

        #endregion Methods
    }
}