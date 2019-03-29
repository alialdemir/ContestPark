using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.ExceptionHandling;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class BoostManagerTests// : TestBase
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IBoostRepository> _boostRepository;
        private Mock<ICpService> _CpService;
        private Mock<DataAccessLayer.Interfaces.IRepository<Boost>> _entityRepository;
        private Mock<IDbFactory> _dDbFactory;
        private IBoostService _boostService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _boostRepository = new Mock<IBoostRepository>();
            _entityRepository = new Mock<DataAccessLayer.Interfaces.IRepository<Boost>>();
            _dDbFactory = new Mock<IDbFactory>();
            _CpService = new Mock<ICpService>();
        }

        private void CreateManager()
        {
            _boostService = new BoostService(_boostRepository.Object, _CpService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IBoostRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void When_IBoostRepository_Null_Expect_ArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IBoostService _boost = new BoostService(null, _CpService.Object, _unitOfWork.Object);
            });
        }

        /// <summary>
        /// Eğer ICpService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void When_ICpService_Null_Expect_ArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IBoostService _boost = new BoostService(_boostRepository.Object, null, _unitOfWork.Object);
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
                IBoostService _boost = new BoostService(_boostRepository.Object, _CpService.Object, null);
            });
        }

        #endregion Constructors methods tests

        #region Insert method tests

        /// <summary>
        /// Tüm değerler doğru ise joker kayıt ekleme durmumu
        /// </summary>
        [TestMethod]
        public void Given_Create_Boost_Expect_Registration()
        {
            // Arrange
            _boostRepository.Setup(p => p.Insert(It.IsAny<Boost>()));
            _entityRepository.Setup(p => p.Insert(It.IsAny<Boost>()));
            _entityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<Boost, bool>>>())).Returns(new List<Boost> { new Boost { BoostId = 1 } }.AsQueryable());
            _unitOfWork.Setup(p => p.Repository<Boost>()).Returns(_entityRepository.Object);

            CreateManager();
            var boost = new Boost { BoostId = 1, Name = "Joker1", PicturePath = "#", Gold = 233, Visibility = true };
            // Act
            _boostService.Insert(boost);
            // Assert
            Assert.IsNotNull(_boostService.Find(x => x.BoostId == boost.BoostId));
        }

        /// <summary>
        /// Tüm değerler null ise joker kayıt ekleme durmumu
        /// </summary>
        [TestMethod]
        public void Insert_Given_Boost_When_Null_Then_ArgumentNullException()
        {
            // Arrange
            CreateManager();
            // Act
            Boost boost = null;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => _boostService.Insert(boost));
        }

        #endregion Insert method tests

        #region GetById method tests

        /// <summary>
        /// Id değeri sıfır giderse null gelmeli
        /// </summary>
        [TestMethod]
        public void When_Id__0_Expect_Null_Model()
        {
            //Arrange
            _entityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<Boost, bool>>>())).Returns(new List<Boost>().AsQueryable());
            _unitOfWork.Setup(p => p.Repository<Boost>()).Returns(_entityRepository.Object);

            CreateManager();
            // Act
            var boost = _boostService.Find(x => x.BoostId == 0).FirstOrDefault();
            // Assert
            Assert.IsNull(boost);
        }

        #endregion GetById method tests

        #region BoostList method tests

        /// <summary>
        /// BoostsList sıfırdan fazla değer döndürmeli
        /// </summary>
        [TestMethod]
        public void BoostList_Count_Not_Equals_0()
        {
            // Arrange
            _boostRepository
                .Setup(p => p.BoostList())
                .Returns(new List<BoostModel>()
            {
                new BoostModel{ BoostId=1, BoostName="test", BoostPicturePath="#", Gold=1}
            });
            CreateManager();
            // Act
            var boostList = _boostService.BoostList();
            // Assert
            Assert.IsTrue(boostList.Count > 0);
        }

        #endregion BoostList method tests

        #region IsBoostIdControl method tests

        /// <summary>
        /// Eğer yanlış bir Boosts id değeri giderse false döndürmeli
        /// </summary>
        [TestMethod]
        public void IsBoostIdControl_Wrong_BoostId_False()
        {
            // Arrange
            _boostRepository.Setup(p => p.IsBoostIdControl(It.IsAny<int>())).Returns(false);
            CreateManager();

            int wrongBoost = 42123;
            // Act
            bool isFalse = _boostService.IsBoostIdControl(wrongBoost);
            // Assert
            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Eğer Doğru bir Boosts id değeri giderse false döndürmeli
        /// </summary>
        [TestMethod]
        public void IsBoostIdControl_Truth_BoostId_True()
        {
            // Arrange
            _boostRepository.Setup(p => p.IsBoostIdControl(It.IsAny<int>())).Returns(true);
            CreateManager();
            // Act
            bool isTrue = _boostService.IsBoostIdControl(1);
            // Assert
            Assert.IsTrue(isTrue);
        }

        #endregion IsBoostIdControl method tests

        #region FindBoostGold method tests

        /// <summary>
        /// Boost id varsa o id'ye ait altın miktarını döndürmesi lazım
        /// </summary>
        [TestMethod]
        public void Given_AddBoost_When_BoostId_True_Then_Gold_Price()
        {
            // Arrange
            byte sampeGold = 222;
            _boostRepository.Setup(p => p.IsBoostIdControl(It.IsAny<int>())).Returns(true);
            _boostRepository.Setup(p => p.FindBoostGold(It.IsAny<int>())).Returns(sampeGold);
            CreateManager();
            // Act
            byte gold = _boostService.FindBoostGold(1);
            // Assert
            Assert.AreEqual(sampeGold, gold);
        }

        /// <summary>
        /// Boost id yoksa o NotificationException fırlatması lazım ve exception mesajı ServerMessage_theJokerIsNotFound olmalı
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"ServerMessage_theJokerIsNotFound\"}")]
        public void FindBoostGold_Wrong_BoostId_False()
        {
            // Arrange
            _boostRepository.Setup(p => p.IsBoostIdControl(It.IsAny<int>())).Returns(false);
            CreateManager();
            // Act
            _boostService.FindBoostGold(312452);
        }

        #endregion FindBoostGold method tests

        #region BuyBoost method tests

        /// <summary>
        /// Joker alınırken yanlış joker id giderse exception fırlatmaıs lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"ServerMessage_theJokerIsNotFound\"}")]
        public void BuyBoost_Wrong_UserId_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string wrongUserId = "2ed-dsp-csd";
            // Act
            _boostService.BuyBoost(wrongUserId, 1234);
        }

        /// <summary>
        /// Joker alınırken null user id giderse exception fırlatmaıs lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: userId")]
        public void BuyBoost_When_UserId_Null_Expect_NotificationException()
        {
            // Arrange
            CreateManager();
            string wrongUserId = String.Empty;
            // Act
            _boostService.BuyBoost(wrongUserId, 1);
        }

        /// <summary>
        /// Joker alınırken yanlış boosts id giderse exception fırlatmaıs lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuyBoost_When_boostId_0_Expect_NotificationException()
        {
            // Arrange
            CreateManager();
            // Act
            _boostService.BuyBoost("dsd-ds-", 0);
        }

        /// <summary>
        /// Joker alınırken her şey doğru ise sorunsuz çalışması lazım
        /// </summary>
        [TestMethod]
        public void BuyBoost_Expect_Success()
        {
            // Arrange
            _boostRepository.Setup(p => p.IsBoostIdControl(It.IsAny<int>())).Returns(true);
            CreateManager();
            // Act
            _boostService.BuyBoost("doğru-kullanıcı-id-var", 10);
        }

        #endregion BuyBoost method tests
    }
}