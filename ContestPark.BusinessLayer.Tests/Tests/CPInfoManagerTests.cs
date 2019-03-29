using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class CpInfoManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICpInfoRepository> _repository;
        private ICpInfoService _CpInfoService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ICpInfoRepository>();
        }

        private void CreateManager()
        {
            _CpInfoService = new CpInfoService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICpInfoService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICpInfoService_Null_Expect_ArgumentNullException()
        {
            // Act
            ICpInfoService repository = new CpInfoService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            ICpInfoService repository = new CpInfoService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region LastDailyChipDateTime method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LastDailyChipDateTime_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _CpInfoService.LastDailyChipDateTime(nullUserId);
        }

        #endregion LastDailyChipDateTime method tests
    }
}