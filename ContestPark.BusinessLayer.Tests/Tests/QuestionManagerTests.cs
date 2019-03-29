using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class QuestionManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IQuestionRepository> _repository;
        private Mock<IScoreService> _scoreService;
        private IQuestionService _questionService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IQuestionRepository>();
            _scoreService = new Mock<IScoreService>();
        }

        private void CreateManager()
        {
            _questionService = new QuestionService(_repository.Object, _scoreService.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IQuestionService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IQuestionService_Null_Expect_ArgumentNullException()
        {
            // Act
            IQuestionService repository = new QuestionService(null, _scoreService.Object, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IScoreService null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IScoreService_Null_Expect_ArgumentNullException()
        {
            // Act
            IQuestionService repository = new QuestionService(_repository.Object, null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IQuestionService repository = new QuestionService(_repository.Object, _scoreService.Object, null);
        }

        #endregion Constructors methods tests

        #region TrueAnswerControl method tests

        /// <summary>
        /// Eğer questionId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void TrueAnswerControl_QuestionId_Zero_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroQuestionId = 0;
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // Act
                _questionService.TrueAnswerControl(zeroQuestionId, Stylish.A, "sample-user-id", 0, 0, 0);
            });
        }

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void TrueAnswerControl_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.TrueAnswerControl(1, Stylish.A, null, 0, 0, 0);
            });
        }

        /// <summary>
        /// Eğer duelInfoId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void TrueAnswerControl_DuelInfoId_Zero_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelInfoId = 0;
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // Act
                _questionService.TrueAnswerControl(1, Stylish.A, "sample-user-id", zeroDuelInfoId, 0, 0);
            });
        }

        /// <summary>
        /// Soruya doğru cevap verdiyse IsTrueAnswer true dönmeli
        /// </summary>
        [TestMethod]
        public void TrueAnswerControl_IsTrueAnswer_Expect_Result_True()
        {
            // Arrange
            _scoreService.Setup(p => p.Insert(It.IsAny<Score>()));
            CreateManager();
            // Act
            var result = _questionService.TrueAnswerControl(1, Stylish.A, "sample-user-id", 1, 0, 111);
            // Assert
            Assert.IsTrue(result.IsTrueAnswer);
        }

        #endregion TrueAnswerControl method tests

        #region RandomQuestion method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void RandomQuestion_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.RandomQuestion(nullUserId, "sample-user-id", 0);
            });
        }

        /// <summary>
        /// Eğer competitorUserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void RandomQuestion_CompetitorUserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullCompetitorUserId = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.RandomQuestion("sample-user-id", nullCompetitorUserId, 0);
            });
        }

        /// <summary>
        /// Dal katmanından RandomQuestion null dönerse Business katmanıda null döndürmeli
        /// </summary>
        [TestMethod]
        public void RandomQuestion_DalResult_Null_Expect_Result_Null()
        {
            // Arrange
            _repository.Setup(x => x.RandomQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns((List<RandomQuestioInfoModel>)null);
            CreateManager();
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
          {
              // Act
              var result = _questionService.RandomQuestion("sample-user-id", "sample-user-id", 0);
          });
        }

        #endregion RandomQuestion method tests

        #region UnansweredQuestions method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void UnansweredQuestions_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.UnansweredQuestions(nullUserId, 1, Stylish.A);
            });
        }

        /// <summary>
        /// Eğer questionId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void UnansweredQuestions_QuestionId_Zero_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroQuestionId = 0;
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // Act
                _questionService.UnansweredQuestions("sample-user-id", zeroQuestionId, Stylish.A);
            });
        }

        /// <summary>
        /// Dal katmanından UnansweredQuestions null dönerse Business katmanıda null döndürmeli
        /// </summary>
        [TestMethod]
        public void UnansweredQuestions_DalResult_Null_Expect_Result_Null()
        {
            // Arrange
            _repository.Setup(x => x.UnansweredQuestions(It.IsAny<int>(), It.IsAny<Stylish>())).Returns((RandomQuestionModel)null);
            CreateManager();
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.UnansweredQuestions("sample-user-id", 1, Stylish.A);
            });
        }

        #endregion UnansweredQuestions method tests

        #region DuelQuestions method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void DuelQuestions_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.DuelQuestions(nullUserId, 1);
            });
        }

        /// <summary>
        /// Eğer DuelId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void DuelQuestions_DuelId_Zero_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // Act
                _questionService.DuelQuestions("sample-user-id", zeroDuelId);
            });
        }

        #endregion DuelQuestions method tests

        #region SolvedQuestions method tests

        /// <summary>
        /// Eğer userId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void SolvedQuestions_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                _questionService.SolvedQuestions(nullUserId, 0);
            });
        }

        #endregion SolvedQuestions method tests
    }
}