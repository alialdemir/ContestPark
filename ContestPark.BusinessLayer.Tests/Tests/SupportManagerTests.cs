using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class SupportManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISupportRepository> _repository;
        private Mock<IEmailSender> _emailSender;
        private ISupportService _supportService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ISupportRepository>();
            _emailSender = new Mock<IEmailSender>();
        }

        private void CreateManager()
        {
            _supportService = new SupportService(_repository.Object, _emailSender.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ISupportRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ISupportRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            ISupportService repository = new SupportService(null, _emailSender.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IEmailSender null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IEmailSender_Null_Expect_ArgumentNullException()
        {
            // Act
            ISupportService repository = new SupportService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ISupportService repository = new SupportService(_repository.Object, _emailSender.Object, null);
        }

        #endregion Constructors methods tests

        #region Insert method tests

        /// <summary>
        /// Eğer entity null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_Entity_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            Support nullSupportEntity = null;
            // Act
            _supportService.Insert(nullSupportEntity);
        }

        #endregion Insert method tests
    }
}