using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class ContestDateManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IContestDateRepository> _repository;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IContestDateRepository>();
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICategoryRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICategoryRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IContestDateService repository = new ContestDateService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IContestDateService repository = new ContestDateService(_repository.Object, null);
        }

        #endregion Constructors methods tests
    }
}