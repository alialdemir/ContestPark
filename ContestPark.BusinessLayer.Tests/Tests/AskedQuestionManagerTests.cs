using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class AskedQuestionManagerTests// : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IAskedQuestionRepository> _askedQuestionRepository;
        private Mock<DataAccessLayer.Interfaces.IRepository<AskedQuestion>> _entityRepository;
        private Mock<IDbFactory> _dDbFactory;
        private IAskedQuestionService _askedQuestionService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _askedQuestionRepository = new Mock<IAskedQuestionRepository>();
            _entityRepository = new Mock<DataAccessLayer.Interfaces.IRepository<AskedQuestion>>();
            _dDbFactory = new Mock<IDbFactory>();
            _askedQuestionService = new AskedQuestionService(_askedQuestionRepository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IAskedQuestionRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void When_IAskedQuestionRepository_Null_Expect_ArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IAskedQuestionService _boost = new AskedQuestionService(null, _unitOfWork.Object);
            });
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IAskedQuestionService _boost = new AskedQuestionService(_askedQuestionRepository.Object, null);
            });
        }

        #endregion Constructors methods tests

        #region Delete method tests

        /// <summary>
        /// User Id değeri null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task When_UserId_Null_Expect_Exception()
        {
            // Arrange
            string userId = String.Empty;
            // Act
            await _askedQuestionService.DeleteAsync(userId, 1);
        }

        /// <summary>
        /// User Id değeri null giderse exception fırlatmalı
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task When_SubcategoryId_0_Expect_Exception()
        {
            //Arrange
            int subCategoryId = 0;
            // Assert
            await _askedQuestionService.DeleteAsync("deneme-user-id", subCategoryId);
        }

        #endregion Delete method tests
    }
}