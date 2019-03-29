using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class PostTypeManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IPostTypeService repository = new PostTypeService(null);
            });
        }

        #endregion Constructors methods tests
    }
}