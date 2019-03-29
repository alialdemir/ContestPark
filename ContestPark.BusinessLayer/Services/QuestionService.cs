using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ContestPark.BusinessLayer.Services
{
    public class QuestionService : ServiceBase<Question>, IQuestionService
    {
        #region Private Variables

        private IQuestionRepository _questionRepository;
        private IUnitOfWork _unitOfWork;
        private IScoreService _scoreService;

        #endregion Private Variables

        #region Constructors

        public QuestionService(
            IQuestionRepository questionRepository,
            IScoreService scoreService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Cevaplar içerisinde doğru  cevabın bulunduğu indexi bulur
        /// </summary>
        /// <param name="answer">Doğru cevap</param>
        /// <param name="answers">Cevaplar dizisi</param>
        /// <returns>Doğru cevabın bulunduğu index numarası</returns>
        private byte TrueAnswerEncodingId(string answer, string[] answers)
        {
            int i = 0;
            for (; i < answers.Length; i++)
                if (answers[i] == answer) break;//Burada cevaplar dizisinde doğru cevabın index numarasını bulduk
            return (byte)(i + 1);//index numarasını geri döndürdük  Not i + 1 yaptık çünkü Stylish enumu 1 den başlıyor dizi indexi 0 dan başlıyor doğru cevabı yakalamıyorduk
        }

        /// <summary>
        /// Kullanıcının soruya doğru cevap verip  vermediğini kontrol eder
        /// </summary>
        /// <param name="questionId">Soru Id</param>
        /// <param name="stylish">Soru şıkkı</param>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelInfoId">Düello bilgi Id</param>
        /// <param name="scorePoint">Puanı</param>
        /// <returns>Doru cevapladımı, kaç puan aldı ve doğru cevabı döndürür</returns>
        public TrueAnswerControlModel TrueAnswerControl(int questionId, Stylish stylish, string userId, int duelInfoId, byte scorePoint, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.QuestionManager.TrueAnswerControl\"");
            Check.IsLessThanZero(questionId, nameof(questionId));
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(duelInfoId, nameof(duelInfoId));

            bool isTrueAnswer = questionId == (int)stylish;
            if (isTrueAnswer)//Cevap doğru ise puan ekliyoruz
            {
                _scoreService.Insert(new Score
                {
                    SubCategoryId = subCategoryId,
                    Point = scorePoint,//Puan durumu
                    ScoreDate = DateTime.Now,
                    UserId = userId,
                    DuelInfoId = duelInfoId,
                });
            }
            return new TrueAnswerControlModel
            {
                IsTrueAnswer = isTrueAnswer,
                TrueAnswer = (Stylish)questionId,
                ScorePoint = scorePoint
            };
        }

        /// <summary>
        /// Daha önce sorulmamış rastgele soru
        /// </summary>
        /// <returns>Rasgele soru</returns>
        public List<RandomQuestioInfoModel> RandomQuestion(string userId, string competitorUserId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.QuestionManager.RandomQuestion\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsNullOrEmpty(competitorUserId, nameof(competitorUserId));

            var randomQuestion = _questionRepository.RandomQuestion(userId, competitorUserId, subCategoryId);
            Check.IsNull(randomQuestion, nameof(randomQuestion));

            randomQuestion.ForEach(p =>
            {
                _unitOfWork
                           .Repository<AskedQuestion>()
                           .Insert(new AskedQuestion//Soru id'si ve kullanıcı id'sini saklıyoruz ki aynı soru aynı kişiye tekrar tekrar sorulmasın tüm sorular bitince sorulsun
                           {
                               UserId = userId,
                               QuestionId = p.QuestionId,
                               SubCategoryId = subCategoryId
                           });
                Thread.Sleep(333);
                p.TrueAnswer = (Stylish)new Random().Next(1, 4);
            });
            return randomQuestion;
        }

        /// <summary>
        /// Düellodaki cevaplanmayan soruyu getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="questionId">Soru ıD</param>
        /// <param name="trueAnswerIndex">Doğru cevabın bulunduğu index numarası</param>
        /// <returns>Soruyu döndürür</returns>
        public RandomQuestionModel UnansweredQuestions(string userId, int questionId, Stylish? trueAnswerIndex)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.QuestionManager.UnansweredQuestions\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(questionId, nameof(questionId));

            RandomQuestionModel randomQuestion = _questionRepository.UnansweredQuestions(questionId, trueAnswerIndex);

            Check.IsNull(randomQuestion, nameof(randomQuestion));

            // Doğru şık ile yanlış her hangi bir şıkkın yerlerini değiştirdik
            string answerTrue = randomQuestion.Answers[0];
            string answerFalseintStylish = randomQuestion.Answers[(byte)trueAnswerIndex - 1];
            randomQuestion.Answers[0] = answerFalseintStylish;// sıfırıncı indexe rasgele üzetilen yanlış şık eklendi
            randomQuestion.Answers[(byte)trueAnswerIndex - 1] = answerTrue;// Rasgele üretilen yeni indexe doğru şık eklendi

            randomQuestion.TrueAnswer = (Stylish)TrueAnswerEncodingId(answerTrue, randomQuestion.Answers);// Burada Questionid'yi dizi içinde bulup index numarasını geri aldık böylece artık sayfaya basabiliriz...
            return randomQuestion;
        }

        /// <summary>
        /// Duellodaki soruları getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>Düelloda sorulan soruların listesi</returns>
        public List<DuelQuestionModel> DuelQuestions(string userId, int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.QuestionManager.DuelQuestions\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(duelId, nameof(duelId));

            return _questionRepository.DuelQuestions(duelId);
        }

        /// <summary>
        /// Alt kategori Id'ye göre kullanıcının o kategorideki çözmüş olduğu soruları yüzdesel olarak döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Yüzdesel çözülen sorular</returns>
        public decimal SolvedQuestions(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.QuestionManager.SolvedQuestions\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            return _questionRepository.SolvedQuestions(userId, subCategoryId);
        }

        #endregion Methods
    }
}