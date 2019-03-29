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
    public class EfDuelInfoDal : EfEntityRepositoryBase<DuelInfo>, IDuelInfoRepository
    {
        #region Readonly Variables

        private readonly EfDuelDal _efDuelDal;

        #endregion Readonly Variables

        #region Internal Variables

        internal byte _maxTime = 30;//Yarışmadaki sürenin server tarafındaki değeri

        #endregion Internal Variables

        #region Properties

        public EfDuelDal _EfDuelDal
        {
            get { return _efDuelDal; }
        }

        #endregion Properties

        #region Private Variables

        private readonly NotificationTypes _contestNotication;
        private readonly NotificationTypes _contestTakeABeating;
        private readonly NotificationTypes _contestResist;
        private readonly PostTypes _contestPostType;
        private readonly ICpRepository _Cp;
        private readonly IQuestionRepository _question;
        private readonly IUserRepository _user;
        private readonly IPostRepository _Post;
        private readonly INotificationRepository _notification;

        #endregion Private Variables

        #region Constructors

        public EfDuelInfoDal(
            ICpRepository Cp,
            IQuestionRepository question,
            IUserRepository user,
            IPostRepository Post,
            INotificationRepository notification,
            IDbFactory dbFactory)
            : base(dbFactory)
        {
            _efDuelDal = new EfDuelDal(dbFactory);// TODO: ctor dan parametre olarak gelmeli

            _contestNotication = NotificationTypes.Contest;
            _contestTakeABeating = NotificationTypes.ContestTakeABeating;
            _contestResist = NotificationTypes.ContestResist;
            _contestPostType = PostTypes.ContestDuel;

            _Cp = Cp;
            _question = question;
            //_question.SubCategoryId = SubCategoryId;
            _user = user;
            _Post = Post;
            _notification = notification;
        }

        #endregion Constructors

        #region Public methods

        /// <summary>
        /// Parametereden gelen founderUserId haricinde bir kullanıcının id'sini döndürür
        /// </summary>
        /// <param name="founderUserId">Kurucu kullanıcı id</param>
        /// <returns>Rastgele kullanıcı id</returns>
        public string RandomCompetitorUserId(string founderUserId)
        {
            //DateTime systemDate = new GlobalSettingsManager().SystemDate(SelectDate.Both);
            return DbContext
                 .Set<User>()
                 //.AsEnumerable()
                 .Where(u => u.Status == true && u.Id != founderUserId /*&& (systemDate - u.LastActiveDate).Days >= 90*/)//3 ay içerisinde giren kullanıcılardan seçiyoruz ki adam girince hesabımla oynanmış demesin
                 .Select(p => p.Id)
                 .OrderBy(p => Guid.NewGuid())
                 .FirstOrDefault();
        }

        /// <summary>
        /// Rastgete  rakip bulma parametreden gelen kullanıcı haricinde bir kullanıcı bulur
        /// </summary>
        /// <param name="founderUserId">Kurucu kullanıcı id</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        public DuelEnterScreenModel DuelStartRandom(string founderUserId, int subCategoryId)//Rasgele rakip bul
        {
            string competitorUserId = RandomCompetitorUserId(founderUserId);
            return DuelEnterScreen(founderUserId, competitorUserId, subCategoryId, true, 0);
        }

        /// <summary>
        /// Kullanıcıları belli olan düellonun düello giriş ekranını döndürür
        /// </summary>
        /// <param name="founderUserId">Kurucu kullanıcı id</param>
        /// <param name="competitorUserId">Rakip kullanıcı id</param>
        /// <param name="notificationStatus">Düello yapıldığının bildirimi gönderilsin mi? true ise gönderir false ise göndermez</param>
        /// <param name="Cp">Ne kadarlık düello yapıldığı</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        public DuelEnterScreenModel DuelEnterScreen(string founderUserId, string competitorUserId, int subCategoryId, bool notificationStatus, int Cp)//Düello başlıyor ekranı
        {
            Duel duel = new Duel
            {
                FounderUserId = founderUserId,
                CompetitorUserId = competitorUserId,
                SubCategoryId = subCategoryId,
                Cp = Cp * 2//İki kullanıcılı olduğu için chip miktarının 2 katını alıyoruz
            };
            _efDuelDal.Insert(duel);

            //Chipleri iki kullanıcıdanda düşüyoruz
            _Cp.RemoveChip(founderUserId, Cp, ChipProcessNames.Game);//kurucudan chip azaltıldı
            _Cp.RemoveChip(competitorUserId, Cp, ChipProcessNames.Game);//rakip oyuncudan chip azaltıldı

            var randomQuestions = _question.RandomQuestion(founderUserId, competitorUserId, subCategoryId);
            foreach (var randomQuestion in randomQuestions)//Duello başlar başlamaz 7 soru ekledik
            {
                // TODO: ekleme işlemini optimize edilmeli tek seferde save yapılsın
                base.Insert(new DuelInfo//Yeni soru oluşturuldu
                {
                    DuelId = duel.DuelId,
                    QuestionId = randomQuestion.QuestionId,////DuelInfoId içinde QuestionId var sonradan duello id geliyor DuelInfoId içine
                    TrueAnswer = randomQuestion.TrueAnswer,//doğru cevabın bulunduğu şıkkı da veri tabanına kaydettik ki soruyu bildi mi? görelim
                    FounderTime = _maxTime,
                    CompetitorTime = _maxTime,
                    CompetitorUserAnswer = Stylish.Cevaplamadi,
                    FounderUserAnswer = Stylish.Cevaplamadi
                });//Duello bilgisi eklendi
            }
            if (notificationStatus)
            {
                _notification
                    .Insert(new Notification//Bildirim eklendi..
                    {
                        NotificationTypeId = (int)_contestNotication,
                        WhoId = founderUserId,
                        WhonId = competitorUserId,
                        Link = duel.DuelId.ToString(),
                        SubCategoryId = subCategoryId
                    });
            }

            _Post
                .Insert(new Post//Kim ne yapıyor eklendi..
                {
                    ContestantId = competitorUserId,
                    UserId = founderUserId,
                    PostTypeId = (int)_contestPostType,
                    DuelId = duel.DuelId,
                    ContestantContestStatus = false,
                    SubCategoryId = subCategoryId
                });
            return DuelEnterScreen(duel.DuelId);
        }

        /// <summary>
        /// Düello id'ye göre düello başlıyor ekranı
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        public DuelEnterScreenModel DuelEnterScreen(int duelId)
        {
            return (from d in DbContext.Set<Duel>()
                    where d.DuelId == duelId
                    //Kurucu
                    join founderUser in DbContext.Set<User>() on d.FounderUserId equals founderUser.Id
                    //Rakip
                    join competitorUser in DbContext.Set<User>() on d.CompetitorUserId equals competitorUser.Id
                    select new DuelEnterScreenModel
                    {
                        //Duello bilgisi
                        DuelId = duelId,
                        //Kurucu
                        ////////////////////////////////////////FounderDegree = _degree.UserDegreeName(founderUser.Id, d.SubCategoryId),
                        FounderFullName = founderUser.FullName,
                        FounderProfilePicturePath = founderUser.ProfilePicturePath,
                        FounderCoverPicturePath = founderUser.CoverPicturePath,
                        //Rakip
                        ////////////////////////////////////////CompetitorDegree = _degree.UserDegreeName(competitorUser.Id, d.SubCategoryId),
                        CompetitorFullName = competitorUser.FullName,
                        CompetitorProfilePicturePath = competitorUser.ProfilePicturePath,
                        CompetitorCoverPicturePath = competitorUser.CoverPicturePath,
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Gelen bildirim id'sine göre duellonun kabul edilip başlatılmasını sağlar
        /// </summary>
        /// <param name="competitorUserId">Rakip kullanıcı id</param>
        /// <param name="notificationId">Bildirim id</param>
        public void AcceptsDuelWithNotification(string competitorUserId, int notificationId)
        {
            var founderInfo = DbContext //Bildirim Id kontrol ettik
                .Set<Notification>()
                .Where(n => n.NotificationId == notificationId && n.NotificationTypeId == (int)_contestNotication)
                .Select(n => new
                {
                    FounderUserId = n.WhoId,
                    DuelId = n.Link,
                    SubCategoryId = n.SubCategoryId//Linkin içinde duello Id var
                }).First();

            if (founderInfo == null) BadStatus("serverMessages.notificationIdWasNotYou");//kurucu id boş ise yanlış bildirimdir..

            _notification
                .Insert(new Notification//Duello kurucusuna düellona karşılık verdi diye bildirim gitti..
                {
                    NotificationTypeId = (int)_contestResist,
                    WhoId = competitorUserId,
                    WhonId = founderInfo.FounderUserId,
                    Link = founderInfo.DuelId,
                    SubCategoryId = founderInfo.SubCategoryId
                });

            _Post
                .Update(competitorUserId, founderInfo.FounderUserId, Convert.ToInt32(founderInfo.DuelId));//Rakibin kime yapıyor listesine bu duelloyu görünür yaptık
        }

        /// <summary>
        /// Parametreden gelen düello id'sini yenildi yapar
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="duelId">Düello id</param>
        public void SmotherDuel(string userId, int duelId)
        {
            bool isDuelIdControl = DbContext//Duello ID'yi bulduk..
                .Set<Duel>()
                .Where(d => d.DuelId == duelId && ((d.FounderUserId == userId) || (d.CompetitorUserId == userId)))
                .Any();
            if (isDuelIdControl)
            {
                List<DuelInfo> duelInfoList = DbSet
                                                .Where(dqi => dqi.DuelId == duelId)
                                                .Select(dqi => dqi)
                                                .ToList();

                bool isFounder = _efDuelDal.IsFounder(userId, duelId);

                if (isFounder) duelInfoList.ForEach(p => p.FounderUserAnswer = Stylish.Escape);
                else duelInfoList.ForEach(p => p.CompetitorUserAnswer = Stylish.Escape);
                int row = SaveChanges();// TODO: bug olabilir

                if (row <= 0) BadStatus("serverMessages.duelRenewalProcessDidNotHappen");

                var founderUserId = DbContext
                    .Set<Duel>()
                    .Where(p => p.DuelId == duelId)
                    .Select(p => new
                    {
                        p.FounderUserId,
                        p.SubCategoryId
                    })
                    .FirstOrDefault();
                _notification
                    .Insert(new Notification//Duello kurucusuna düellona karşılık verdi diye bildirim gitti..
                    {
                        NotificationTypeId = (int)_contestTakeABeating,
                        WhoId = userId,
                        WhonId = founderUserId.FounderUserId,
                        Link = duelId.ToString(),
                        SubCategoryId = founderUserId.SubCategoryId
                    });
                _Post
                    .Update(userId, founderUserId.FounderUserId, duelId);//Rakibin kime yapıyor listesine bu duelloyu görünür yaptık
            }
        }

        /// <summary>
        /// Parametreden gelen düello id'sine ait soruyu getirir
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <param name="userId"><Kullanıcı id/param>
        /// <returns>Düelloya ait soru moedeli</returns>
        public RandomQuestionModel NextDuelStart(int duelId, string userId)//Sonraki duelloya geçer
        {
            bool isFounder = _efDuelDal.IsFounder(userId, duelId);
            int duelCount = DbSet//Kaç tane duello id'si olduğunu kontrol etmek için duello idlerini listeledik..
                .Where(d => (d.DuelId == duelId && isFounder && d.FounderUserAnswer != Stylish.Cevaplamadi) || (d.DuelId == duelId && !isFounder && d.CompetitorUserAnswer != Stylish.Cevaplamadi))
                .Count();

            if (duelCount >= 7) return null;//Burada bir duello maksimum 7 sorudan oluşur 7 soru sorulmuş ise null döner..

            RandomQuestionModel randomQuestion = new RandomQuestionModel();
            randomQuestion = DuelQuestionControl(duelId, userId);//Kullanıcının o düelloda çözmediği soru var mı
                                                                 //Önemli not : burada kurucuda çıkan sorularda doğru cevap hangi şıkta ise rakipde de o şıkta olmasını sağladık böylece iki tarafında doğru cevap veremediklerini kontrol ettirebilcez..
            byte trueAnswer = (byte)randomQuestion.TrueAnswer;//doğru cevabın bulunduğu şıkkı da veri tabanına kaydettik ki soruyu bildi mi? görelim
            Stylish? trueAnswerDatabase = DbSet
                                            .Where(dqi => dqi.DuelInfoId == randomQuestion.DuelInfoId)
                                            .Select(p => p.TrueAnswer)
                                            .FirstOrDefault();//Doğru cevabın bulunması gereken şık indexi

            string answer1 = randomQuestion.Answers[(byte)trueAnswerDatabase - 1];
            string answerTrue = randomQuestion.Answers[trueAnswer - 1];

            randomQuestion.Answers[(byte)trueAnswerDatabase - 1] = answerTrue;

            randomQuestion.Answers[trueAnswer - 1] = answer1;
            randomQuestion.TrueAnswer = trueAnswerDatabase;
            return randomQuestion;
        }

        /// <summary>
        /// Kullanıcı soruyu cevapladımı
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <param name="userId"><Kullanıcı id/param>
        /// <returns>Düelloya ait soru moedeli</returns>
        /// <returns></returns>
        public RandomQuestionModel DuelQuestionControl(int duelId, string userId)
        {
            bool isFounder = _efDuelDal.IsFounder(userId, duelId);//Kurucu mu rakip mi true ise kuucu false ise rakip
            RandomQuestionModel result = null;
            var dqiinfo = DbSet
                            .Where(dqi => dqi.DuelId == duelId && ((isFounder && dqi.FounderUserAnswer == Stylish.Cevaplamadi) || (!isFounder && dqi.CompetitorUserAnswer == Stylish.Cevaplamadi)))//FounderUserAnswer null ise soruyu cevaplamamıştır.. CompetitorUserAnswer null ise soruyu cevaplamamıştır..)
                            .Select(dqi => new
                            {
                                dqi.TrueAnswer,
                                dqi.QuestionId,
                                dqi.DuelInfoId
                            })
                            .FirstOrDefault();
            if (dqiinfo != null)
            {
                result = _question.UnansweredQuestions(dqiinfo.QuestionId, dqiinfo.TrueAnswer);
                result.DuelInfoId = dqiinfo.DuelInfoId;
            }
            return result;
        }

        /// <summary>
        /// Kullanıcının cevapladığı soru doğru cevap mı kontrol eder
        /// </summary>
        /// <param name="DuelInfoId">Düello bilgisi id</param>
        /// <param name="time">Soruyu kaçıncı saniyede cevapladı</param>
        /// <param name="questionId">Soru id</param>
        /// <param name="stylish">Hangi şıkkı seçti(soru şıkkı)</param>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Cevap doğrumu modeli</returns>
        public TrueAnswerControlModel TrueAnswerControl(string DuelInfoId, byte time, int questionId, Stylish stylish, string userId)
        {
            byte scorePoint = time;//Aldığı puan kaçıncı sürede cevapladığı
            int intDuelInfoIdi = Convert.ToInt32(DuelInfoId);//Şifreli idyi çözdük
            DuelInfo duelQuizInfo = GetById(intDuelInfoIdi);
            bool isFounder = _efDuelDal.IsFounder(userId, duelQuizInfo.DuelId);
            if (isFounder)//kurucu ise
            {
                duelQuizInfo.FounderTime = time;
                duelQuizInfo.FounderUserAnswer = stylish;
                if (duelQuizInfo.CompetitorUserAnswer == duelQuizInfo.TrueAnswer && duelQuizInfo.CompetitorTime > time) scorePoint -= 2;//Rakibinden sonra cevap verirse -2 puan alır
            }
            else//rakip ise
            {
                duelQuizInfo.CompetitorTime = time;
                duelQuizInfo.CompetitorUserAnswer = stylish;
                if (duelQuizInfo.FounderUserAnswer == duelQuizInfo.TrueAnswer && duelQuizInfo.FounderTime > time) scorePoint -= 2;//Rakibinden sonra cevap verirse -2 puan alır
            }
            base.Update(duelQuizInfo);
            // TODO: Burası iş katmanındaki methodu kullanmalı
            return _question.TrueAnswerControl(questionId, stylish, userId, duelQuizInfo.DuelInfoId, scorePoint);
        }

        /// <summary>
        /// Düello id'sine göre düelloyu kim kazandı onun bilgisini geri döndürür
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <returns>Düello sonucu modeli</returns>
        public DuelResultModel DuelResult(int duelId)
        {
            DuelResultModel duelResult = (from d in DbContext.Set<Duel>()
                                          where d.DuelId == duelId
                                          join founderUser in DbContext.Set<User>() on d.FounderUserId equals founderUser.Id

                                          join competitorUser in DbContext.Set<User>() on d.CompetitorUserId equals competitorUser.Id
                                          select new DuelResultModel
                                          {
                                              //Soru bilgisi
                                              Bet = d.Cp,
                                              //Kurucu
                                              FounderUserId = founderUser.Id,
                                              FounderUserName = founderUser.UserName,
                                              FounderFullName = founderUser.FullName,
                                              FounderProfilePicturePath = founderUser.ProfilePicturePath,
                                              FounderTrueAnswerCount = DbSet
                                              .Where(dqi => dqi.DuelId == duelId && dqi.FounderUserAnswer == dqi.TrueAnswer)
                                              .Count(),
                                              FounderNullAnswerCount = DbSet//cevaplanmayan soru sayısını aldık böylece düellodaki karşı tarafında cevap verip vermediğini öğrenmiş olduk.
                                              .Where(dqi => dqi.DuelId == duelId && dqi.FounderUserAnswer == Stylish.Cevaplamadi)
                                              .Any(),
                                              FounderEscapeStatus = DbSet
                                              .Where(dqi => dqi.DuelId == duelId && dqi.FounderUserAnswer == Stylish.Escape)
                                              .Any(),
                                              FounderScorePoint = (byte)((from di in DbSet//Toplam puanı aldık
                                                                          where di.DuelId == duelId
                                                                          join s in DbContext.Set<Score>().Where(p => p.UserId == founderUser.Id) on di.DuelInfoId equals s.DuelInfoId into data
                                                                          from data1 in data.DefaultIfEmpty()
                                                                          select (int?)data1.Point).Sum() ?? 0),
                                              //Rakip
                                              CompetitorUserId = competitorUser.Id,
                                              CompetitorUserName = competitorUser.UserName,
                                              CompetitorFullName = competitorUser.FullName,
                                              CompetitorProfilePicturePath = competitorUser.ProfilePicturePath,
                                              CompetitorTrueAnswerCount = DbSet
                                              .Where(dqi => dqi.DuelId == duelId && dqi.CompetitorUserAnswer == dqi.TrueAnswer)
                                              .Count(),
                                              CompetitorNullAnswerCount = DbSet//cevaplanmayan soru sayısını aldık böylece düellodaki karşı tarafında cevap verip vermediğini öğrenmiş olduk.
                                              .Where(dqi => dqi.DuelId == duelId && dqi.CompetitorUserAnswer == Stylish.Cevaplamadi)
                                              .Any(),
                                              CompetitorEscapeStatus = DbSet
                                              .Where(dqi => dqi.DuelId == duelId && dqi.CompetitorUserAnswer == Stylish.Escape)
                                              .Any(),//cevaplanmayan soru sayısını aldık böylece düellodaki karşı tarafında cevap verip vermediğini öğrenmiş olduk.

                                              CompetitorScorePoint = (byte)((from di in DbSet//Toplam puanı aldık
                                                                             where di.DuelId == duelId
                                                                             join s in DbContext.Set<Score>().Where(p => p.UserId == competitorUser.Id) on di.DuelInfoId equals s.DuelInfoId into data
                                                                             from data1 in data.DefaultIfEmpty()
                                                                             select (int?)data1.Point).Sum() ?? 0),
                                          }).FirstOrDefault();
            return duelResult;
        }

        /// <summary>
        /// Kullanıcı adına göre yarışmalardaki kazanma kaybetme istatislik bilgisini verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kategorilerdeki istatiksel bilgi modeli</returns>
        public StatisticsInfoModel SelectedContestStatistics(string userName, int subCateogryId)
        {
            string userId = _user.UserId(userName);
            var s = (from d in DbContext.Set<Duel>()
                     where (d.FounderUserId == userId || d.CompetitorUserId == userId) && d.SubCategoryId == subCateogryId
                     select new
                     {
                         //Kurucu
                         FounderTrueAnswerCount = (from dqi in DbSet
                                                   where dqi.DuelId == d.DuelId && dqi.FounderUserAnswer == dqi.TrueAnswer && (dqi.FounderUserAnswer != Stylish.Cevaplamadi || dqi.CompetitorUserAnswer != Stylish.Cevaplamadi)
                                                   select dqi.DuelInfoId).Count(),
                         //Rakip
                         CompetitorTrueAnswerCount = (from dqi in DbSet
                                                      where dqi.DuelId == d.DuelId && dqi.CompetitorUserAnswer == dqi.TrueAnswer && (dqi.FounderUserAnswer != Stylish.Cevaplamadi || dqi.CompetitorUserAnswer != Stylish.Cevaplamadi)
                                                      select dqi.DuelInfoId).Count()
                     }).ToList();
            return new StatisticsInfoModel()
            {
                Draws = s.Where(p => p.CompetitorTrueAnswerCount == p.FounderTrueAnswerCount).Count(),//berabere
                Won = s.Where(p => p.CompetitorTrueAnswerCount < p.FounderTrueAnswerCount).Count(),//kazandığı
                Lose = s.Where(p => p.CompetitorTrueAnswerCount > p.FounderTrueAnswerCount).Count()//kaybettiği
            };
        }

        /// <summary>
        /// Her yarışma için kaç oyun oynadı onu verir
        /// Kazandığı kaybettiği berabere yada beklemede olanların hepsi
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Oynadığı yarışmaların istasiksel bilgisi</returns>
        public ServiceModel<ContestMostPlayModel> ContestMostPlay(string userName, PagingModel pagingModel)
        {
            string userId = _user.UserId(userName);
            byte langId = (byte)Thread.CurrentThread.CurrentCulture.Name.ToLanguagesEnum();
            return (from d in DbContext.Set<Duel>()
                    where d.FounderUserId == userId || d.CompetitorUserId == userId
                    from di in DbSet
                    where di.DuelId == d.DuelId && (di.FounderUserAnswer != Stylish.Cevaplamadi || di.CompetitorUserAnswer != Stylish.Cevaplamadi)
                    group d by d.SubCategoryId into dCategory
                    join sc in DbContext.Set<SubCategoryLang>() on dCategory.Key equals sc.SubCategoryId
                    where sc.LanguageId == langId
                    select new ContestMostPlayModel
                    {
                        PlayNumber = dCategory.Count(),
                        CategoryName = sc.SubCategoryName,
                        CategoryId = sc.SubCategoryId
                    })
                    .OrderByDescending(p => p.PlayNumber)
                    .ToServiceModel(pagingModel);
        }

        /// <summary>
        /// Oynadığı tüm kategorilerde kazanma kaybetme beraberlik durumunu verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>İstatiksel bilgi</returns>
        public StatisticsInfoModel GlobalStatisticsInfo(string userName)
        {
            string userId = _user.UserId(userName);
            var s = (from d in DbContext.Set<Duel>()
                     where d.FounderUserId == userId || d.CompetitorUserId == userId
                     select new
                     {
                         //Soru bilgisi
                         //Kurucu
                         FounderTrueAnswerCount = (from dqi in DbSet
                                                   where dqi.DuelId == d.DuelId && dqi.FounderUserAnswer == dqi.TrueAnswer && (dqi.FounderUserAnswer != Stylish.Cevaplamadi || dqi.CompetitorUserAnswer != Stylish.Cevaplamadi)
                                                   select dqi.DuelInfoId).Count(),
                         //Rakip
                         CompetitorTrueAnswerCount = (from dqi in DbSet
                                                      where dqi.DuelId == d.DuelId && dqi.CompetitorUserAnswer == dqi.TrueAnswer && (dqi.FounderUserAnswer != Stylish.Cevaplamadi || dqi.CompetitorUserAnswer != Stylish.Cevaplamadi)
                                                      select dqi.DuelInfoId).Count()
                     }).ToList();
            return new StatisticsInfoModel()
            {
                TotalGames = s.Count,//Toplam oyun sayısı
                Draws = s.Where(p => p.CompetitorTrueAnswerCount == p.FounderTrueAnswerCount).Count(),//berabere
                Won = s.Where(p => p.CompetitorTrueAnswerCount < p.FounderTrueAnswerCount).Count(),//kazandığı
                Lose = s.Where(p => p.CompetitorTrueAnswerCount > p.FounderTrueAnswerCount).Count()//kaybettiği
            };
        }

        /// <summary>
        /// Alt kategori Id'ye göre kullanıcının o kategorideki çözmüş olduğu soruları yüzdesel olarak döndürür
        /// Alt kategori id nesne oluştururken verildi !!!
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Yüzdesel çözülen sorular</returns>
        public decimal SolvedQuestions(string userId, int subCategoryId)
        {
            return _question.SolvedQuestions(userId, subCategoryId);
        }

        /// <summary>
        /// Gelen question Id göre diğer yarışmacıların bu soruya verdiği cevaplar
        /// </summary>
        /// <param name="questionId">Soru id</param>
        /// <returns>Verdiği cevaplar</returns>
        public AudienceAnswersModel AudienceAnswers(int questionId)
        {
            return new AudienceAnswersModel
            {// (askedQuestion * 100 / QuestionsCount)
                Stylish1 = DbSet
                .Where(p => p.QuestionId == questionId && (p.FounderUserAnswer == Stylish.A || p.CompetitorUserAnswer == Stylish.A))
                .Count(),
                Stylish2 = DbSet
                .Where(p => p.QuestionId == questionId && (p.FounderUserAnswer == Stylish.B || p.CompetitorUserAnswer == Stylish.B))
                .Count(),
                Stylish3 = DbSet
                .Where(p => p.QuestionId == questionId && (p.FounderUserAnswer == Stylish.C || p.CompetitorUserAnswer == Stylish.C))
                .Count(),
                Stylish4 = DbSet
                .Where(p => p.QuestionId == questionId && (p.FounderUserAnswer == Stylish.D || p.CompetitorUserAnswer == Stylish.D))
                .Count()
            };
        }

        #endregion Public methods
    }
}