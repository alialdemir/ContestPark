using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfQuestionDal : EfEntityRepositoryBase<Question>, IQuestionRepository
    {
        #region Private Variables

        private readonly IAskedQuestionRepository _askedQuestion;

        #endregion Private Variables

        #region Constructors

        public EfQuestionDal(IAskedQuestionRepository askedQuestion, IDbFactory dbFactory)
            : base(dbFactory)
        {
            _askedQuestion = askedQuestion;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Doğru şık ile yanlış her hangi bir şıkkın yerlerini değiştirdik
        /// </summary>
        /// <param name="answers">Doğru cevap</param>
        /// <param name="Stylish1">Cevap 1</param>
        /// <param name="Stylish2">Cevap 2</param>
        /// <param name="Stylish3">Cevap 3</param>
        /// <param name="trueAnswer">Doğru cevap Index numarası</param>
        /// <returns>Karıştırılmış cevap dizisi</returns>
        private string[] AnswerOrder(string answers, string Stylish1, string Stylish2, string Stylish3, Stylish? trueAnswer)
        {
            string[] result = new string[] { answers, Stylish1, Stylish2, Stylish3 };
            result[0] = result[(byte)trueAnswer - 1];//sıfırıncı indexe rasgele üzetilen yanlış şık eklendi
            result[(byte)trueAnswer - 1] = answers;//Rasgele üretilen yeni indexe doğru şık eklendi
            return result;
        }

        /// <summary>
        /// Duellodaki soruları getirir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="duelId">Düello Id</param>
        /// <returns>Düelloda sorulan soruların listesi</returns>
        public List<DuelQuestionModel> DuelQuestions(int duelId)
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from dqi in DbContext.Set<DuelInfo>().AsEnumerable()
                    where dqi.DuelId == duelId
                    join q in DbContext.Set<Question>() on dqi.QuestionId equals q.QuestionId
                    join ql in DbContext.Set<QuestionLang>() on q.QuestionId equals ql.QuestionId
                    where ql.LanguageId == (byte)language
                    orderby dqi.DuelInfoId ascending
                    select new DuelQuestionModel
                    {
                        Question = ql.Questions,
                        Answers = AnswerOrder(ql.Answer, ql.Stylish1, ql.Stylish2, ql.Stylish3, dqi.TrueAnswer),
                        CompetitorTime = dqi.CompetitorTime,
                        CompetitorUserAnswer = dqi.CompetitorUserAnswer,
                        FounderTime = dqi.FounderTime,
                        FounderUserAnswer = dqi.FounderUserAnswer,
                        TrueAnswer = dqi.TrueAnswer,
                        Link = q.Link,
                        AnswerType = q.AnswerType,
                        QuestionType = q.QuestionType
                    }).ToList();
        }

        /// <summary>
        /// Alt kategori Id'ye göre kullanıcının o kategorideki çözmüş olduğu soruları yüzdesel olarak döndürür
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Yüzdesel çözülen sorular</returns>
        public decimal SolvedQuestions(string userId, int subCategoryId)
        {
            decimal QuestionsCount = (from sc in DbContext.Set<SubCategory>()
                                      where sc.SubCategoryId == subCategoryId
                                      join q in DbSet on sc.SubCategoryId equals q.SubCategoryId
                                      select q.QuestionId).Count();
            decimal askedQuestion = (from d in DbContext.Set<Duel>()
                                     where d.FounderUserId == userId || d.CompetitorUserId == userId
                                     join di in DbContext.Set<DuelInfo>() on d.DuelId equals di.DuelId
                                     where (d.FounderUserId == userId && di.FounderUserAnswer != Stylish.Cevaplamadi) || (d.CompetitorUserId == userId && di.CompetitorUserAnswer != Stylish.Cevaplamadi)
                                     join q in DbSet on subCategoryId equals q.SubCategoryId
                                     where q.QuestionId == di.QuestionId
                                     select di.QuestionId).Count();
            decimal yuzde = (askedQuestion * 100 / QuestionsCount);
            return yuzde <= 100 ? yuzde : 100;
        }

        /// <summary>
        /// Soru Id'ye göre soruyu getirir
        /// </summary>
        /// <param name="language">Kullanıcı dili</param>
        /// <param name="questionId">Soru Id</param>
        /// <param name="trueAnswerIndex">Doğru cevap index numarası</param>
        /// <returns>Soru bilgisi</returns>
        public RandomQuestionModel UnansweredQuestions(int questionId, Stylish? trueAnswerIndex)//Kullanıcının duelloda cevaplamadığı soruyu getirir
        {
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            var randomQuestion = (from q in DbSet
                                  join ql in DbContext.Set<QuestionLang>() on q.QuestionId equals ql.QuestionId
                                  where q.QuestionId == questionId && ql.LanguageId == (byte)language
                                  select new
                                  {
                                      ql.Questions,
                                      ql.Answer,
                                      ql.Stylish1,
                                      ql.Stylish2,
                                      ql.Stylish3,
                                      q.QuestionId,
                                      q.QuestionType,
                                      q.AnswerType,
                                      q.Link
                                  }).OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            if (randomQuestion == null) BadStatus("serverMessages.requestedQuestionNotFound");
            RandomQuestionModel rndRandomQuestion = new RandomQuestionModel()
            {
                //soru ve soru id
                AnswerType = (byte)randomQuestion.AnswerType,
                QuestionType = (byte)randomQuestion.QuestionType,
                Question = randomQuestion.Questions,
                QuestionId = randomQuestion.QuestionId,
                DuelInfoId = randomQuestion.QuestionId,
                TrueAnswer = Stylish.A,
                Link = randomQuestion.Link
            };
            //şıklar 0. index doğru cevap
            rndRandomQuestion.Answers[0] = randomQuestion.Answer;
            rndRandomQuestion.Answers[1] = randomQuestion.Stylish1;
            rndRandomQuestion.Answers[2] = randomQuestion.Stylish2;
            rndRandomQuestion.Answers[3] = randomQuestion.Stylish3;
            return rndRandomQuestion;
        }

        /// <summary>
        /// Rasgele soru getirir detaylı açıklama QuestionManager kısmında...
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="competitorUserId">Rakip yarışmacı kullanıcı Id</param>
        /// <param name="language">Kullanıcının dil bilgisi</param>
        /// <returns>Rasgele soru Id'si</returns>
        public List<RandomQuestioInfoModel> RandomQuestion(string userId, string competitorUserId, int subCategoryId)
        {//Not Soru sayısı çoğalınca burada hem kurucu hemde rakibin çözmediği sorular gelsin
            Languages language = Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            var randomQuestion = (from q in DbSet
                                  from ql in DbContext.Set<QuestionLang>().Where(p => p.LanguageId == (byte)language)
                                  where q.IsActive && q.SubCategoryId == subCategoryId && !DbContext.Set<AskedQuestion>().Where(p => p.QuestionId == q.QuestionId && (p.UserId == userId)).Any()
                                  && q.QuestionId == ql.QuestionId
                                  select new RandomQuestioInfoModel
                                  {
                                      QuestionId = q.QuestionId
                                  })
                                  .OrderBy(x => Guid.NewGuid())
                                  .Take(7)
                                  .ToList();
            if (randomQuestion == null)
            {
                _askedQuestion.DeleteAsync(userId, subCategoryId);
                return this.RandomQuestion(userId, competitorUserId, subCategoryId);
            }
            return randomQuestion;
        }

        public TrueAnswerControlModel TrueAnswerControl(int questionId, Stylish stylish, string userId, int duelInfoId, byte scorePoint)
        {//Business tarafında kullanındı
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}