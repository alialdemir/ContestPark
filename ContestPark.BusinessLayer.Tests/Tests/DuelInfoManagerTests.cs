using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.Entities.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class DuelInfoManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IDuelInfoRepository> _repository;
        private IDuelInfoService _duelInfoService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IDuelInfoRepository>();
        }

        private void CreateManager()
        {
            _duelInfoService = new DuelInfoService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer ICoverPictureRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_ICoverPictureRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IDuelInfoService repository = new DuelInfoService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IDuelInfoService repository = new DuelInfoService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region AcceptsDuelWithNotification method tests

        /// <summary>
        /// Eğer competitorUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AcceptsDuelWithNotification_CompetitorUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullCompetitorUserId = String.Empty;
            // Act
            _duelInfoService.AcceptsDuelWithNotification(nullCompetitorUserId, 1);
        }

        /// <summary>
        /// Eğer notificationId null gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AcceptsDuelWithNotification_NotificationId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroNotificationId = 0;
            // Act
            _duelInfoService.AcceptsDuelWithNotification("deneme-user-id", zeroNotificationId);
        }

        #endregion AcceptsDuelWithNotification method tests

        #region AudienceAnswers method tests

        /// <summary>
        /// Eğer questionId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AudienceAnswers_QuestionId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroQuestionId = 0;
            // Act
            _duelInfoService.AudienceAnswers(zeroQuestionId);
        }

        #endregion AudienceAnswers method tests

        #region ContestMostPlay method tests

        /// <summary>
        /// Eğer userName null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContestMostPlay_UserName_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserName = String.Empty;
            // Act
            _duelInfoService.ContestMostPlay(nullUserName, new Entities.Models.PagingModel());
        }

        #endregion ContestMostPlay method tests

        #region DuelEnterScreen method tests

        /// <summary>
        /// Eğer founderUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuelEnterScreen_FounderUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFounderUserId = String.Empty;
            // Act
            _duelInfoService.DuelEnterScreen(nullFounderUserId, "deneme-user-id", 1, true, 1);
        }

        /// <summary>
        /// Eğer competitorUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuelEnterScreen_CompetitorUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullCompetitorUserId = String.Empty;
            // Act
            _duelInfoService.DuelEnterScreen("deneme-user-id", nullCompetitorUserId, 1, true, 1);
        }

        /// <summary>
        /// Eğer duelId null gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DuelEnterScreen_DuelId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Act
            _duelInfoService.DuelEnterScreen(zeroDuelId);
        }

        #endregion DuelEnterScreen method tests

        #region DuelQuestionControl method tests

        /// <summary>
        /// Eğer duelId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DuelQuestionControl_DuelId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Act
            _duelInfoService.DuelQuestionControl(zeroDuelId, "deneme-user-id");
        } /// <summary>

        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuelQuestionControl_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelInfoService.DuelQuestionControl(1, nullUserId);
        }

        #endregion DuelQuestionControl method tests

        #region DuelResult method tests

        /// <summary>
        /// Eğer duelId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DuelResult_DuelId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Act
            _duelInfoService.DuelResult(zeroDuelId);
        }

        #endregion DuelResult method tests

        #region DuelStartRandom method tests

        /// <summary>
        /// Eğer founderUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuelStartRandom_FounderUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFounderUserId = String.Empty;
            // Act
            _duelInfoService.DuelStartRandom(nullFounderUserId, 1);
        }

        #endregion DuelStartRandom method tests

        #region GlobalStatisticsInfo method tests

        /// <summary>
        /// Eğer userName null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GlobalStatisticsInfo_UserName_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserName = String.Empty;
            // Act
            _duelInfoService.GlobalStatisticsInfo(nullUserName);
        }

        #endregion GlobalStatisticsInfo method tests

        #region NextDuelStart method tests

        /// <summary>
        /// Eğer duelId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NextDuelStart_DuelId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Act
            _duelInfoService.NextDuelStart(zeroDuelId, "deneme-user-id");
        }

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NextDuelStart_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelInfoService.NextDuelStart(1, nullUserId);
        }

        #endregion NextDuelStart method tests

        #region RandomCompetitorUserId method tests

        /// <summary>
        /// Eğer founderUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RandomCompetitorUserId_FounderUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullFounderUserId = String.Empty;
            // Act
            _duelInfoService.RandomCompetitorUserId(nullFounderUserId);
        }

        #endregion RandomCompetitorUserId method tests

        #region SelectedContestStatistics method tests

        /// <summary>
        /// Eğer userName null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelectedContestStatistics_UserName_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserName = String.Empty;
            // Act
            _duelInfoService.SelectedContestStatistics(nullUserName, 1);
        }

        #endregion SelectedContestStatistics method tests

        #region SmotherDuel method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SmotherDuel_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelInfoService.SmotherDuel(nullUserId, 1);
        }

        /// <summary>
        /// Eğer duelId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SmotherDuel_DuelId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Act
            _duelInfoService.SmotherDuel("deneme-user-id", zeroDuelId);
        }

        #endregion SmotherDuel method tests

        #region SolvedQuestions method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SolvedQuestions_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _duelInfoService.SolvedQuestions(nullUserId, 0);
        }

        #endregion SolvedQuestions method tests

        #region TrueAnswerControl method tests

        /// <summary>
        /// Eğer DuelInfoId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TrueAnswerControl_DuelInfoId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullDuelInfoId = String.Empty;
            // Act
            _duelInfoService.TrueAnswerControl(nullDuelInfoId, 1, 1, Stylish.A, "deneme-user-id");
        }

        /// <summary>
        /// Eğer questionId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TrueAnswerControl_QuestionId_0_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int nullQuestionId = 0;
            // Act
            _duelInfoService.TrueAnswerControl("deneme-duel-info-id", 1, nullQuestionId, Stylish.A, "deneme-user-id");
        }

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TrueAnswerControl_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act;
            _duelInfoService.TrueAnswerControl("deneme-user-id", 1, 1, Stylish.A, nullUserId);
        }

        #endregion TrueAnswerControl method tests
    }
}