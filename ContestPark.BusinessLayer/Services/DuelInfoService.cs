using ContestPark.BusinessLayer.Interfaces;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.BusinessLayer.Services
{
    public class DuelInfoService : ServiceBase<DuelInfo>, IDuelInfoService
    {
        #region Private Variables

        private IDuelInfoRepository _duelInfoRepository;

        #endregion Private Variables

        #region Constructors

        public DuelInfoService(IDuelInfoRepository duelInfoRepository, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _duelInfoRepository = duelInfoRepository ?? throw new ArgumentNullException(nameof(duelInfoRepository));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gelen bildirim id'sine göre duellonun kabul edilip başlatılmasını sağlar
        /// </summary>
        /// <param name="competitorUserId">Rakip kullanıcı id</param>
        /// <param name="notificationId">Bildirim id</param>
        public void AcceptsDuelWithNotification(string competitorUserId, int notificationId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.AcceptsDuelWithNotification\"");
            Check.IsNullOrEmpty(competitorUserId, nameof(competitorUserId));
            Check.IsLessThanZero(notificationId, nameof(notificationId));

            _duelInfoRepository.AcceptsDuelWithNotification(competitorUserId, notificationId);
        }

        /// <summary>
        /// Gelen question Id göre diğer yarışmacıların bu soruya verdiği cevaplar
        /// </summary>
        /// <param name="questionId">Soru id</param>
        /// <returns>Verdiği cevaplar</returns>
        public AudienceAnswersModel AudienceAnswers(int questionId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.AudienceAnswers\"");
            Check.IsLessThanZero(questionId, nameof(questionId));

            return _duelInfoRepository.AudienceAnswers(questionId);
        }

        /// <summary>
        /// Her yarışma için kaç oyun oynadı onu verir
        /// Kazandığı kaybettiği berabere yada beklemede olanların hepsi
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Oynadığı yarışmaların istasiksel bilgisi</returns>
        public ServiceModel<ContestMostPlayModel> ContestMostPlay(string userName, PagingModel pagingModel)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.ContestMostPlay\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _duelInfoRepository.ContestMostPlay(userName, pagingModel);
        }

        /// <summary>
        /// Kullanıcıları belli olan düellonun düello giriş ekranını döndürür
        /// </summary>
        /// <param name="founderUserId">Kurucu kullanıcı id</param>
        /// <param name="competitorUserId">Rakip kullanıcı id</param>
        /// <param name="notificationStatus">Düello yapıldığının bildirimi gönderilsin mi? true ise gönderir false ise göndermez</param>
        /// <param name="Cp">Ne kadarlık düello yapıldığı</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        public DuelEnterScreenModel DuelEnterScreen(string founderUserId, string competitorUserId, int subCategoryId, bool notificationStatus, int Cp)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.DuelEnterScreen\"");
            Check.IsNullOrEmpty(founderUserId, nameof(founderUserId));
            Check.IsNullOrEmpty(competitorUserId, nameof(competitorUserId));

            return _duelInfoRepository.DuelEnterScreen(founderUserId, competitorUserId, subCategoryId, notificationStatus, Cp);
        }

        /// <summary>
        /// Düello id'ye göre düello başlıyor ekranı
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        public DuelEnterScreenModel DuelEnterScreen(int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.DuelEnterScreen\"");
            Check.IsLessThanZero(duelId, nameof(duelId));

            return _duelInfoRepository.DuelEnterScreen(duelId);
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
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.DuelQuestionControl\"");
            Check.IsLessThanZero(duelId, nameof(duelId));
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _duelInfoRepository.DuelQuestionControl(duelId, userId);
        }

        /// <summary>
        /// Düello id'sine göre düelloyu kim kazandı onun bilgisini geri döndürür
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <returns>Düello sonucu modeli</returns>
        public DuelResultModel DuelResult(int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.DuelResult\"");
            Check.IsLessThanZero(duelId, nameof(duelId));

            return _duelInfoRepository.DuelResult(duelId);
        }

        /// <summary>
        /// Rastgete  rakip bulma parametreden gelen kullanıcı haricinde bir kullanıcı bulur
        /// </summary>
        /// <param name="founderUserId">Kurucu kullanıcı id</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        public DuelEnterScreenModel DuelStartRandom(string founderUserId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.DuelStartRandom\"");
            Check.IsNullOrEmpty(founderUserId, nameof(founderUserId));

            return _duelInfoRepository.DuelStartRandom(founderUserId, subCategoryId);
        }

        /// <summary>
        /// Oynadığı tüm kategorilerde kazanma kaybetme beraberlik durumunu verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>İstatiksel bilgi</returns>
        public StatisticsInfoModel GlobalStatisticsInfo(string userName)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.GlobalStatisticsInfo\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _duelInfoRepository.GlobalStatisticsInfo(userName);
        }

        /// <summary>
        /// Parametreden gelen düello id'sine ait soruyu getirir
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <param name="userId"><Kullanıcı id/param>
        /// <returns>Düelloya ait soru moedeli</returns>
        public RandomQuestionModel NextDuelStart(int duelId, string userId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.NextDuelStart\"");
            Check.IsLessThanZero(duelId, nameof(duelId));
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _duelInfoRepository.NextDuelStart(duelId, userId);
        }

        /// <summary>
        /// Parametereden gelen founderUserId haricinde bir kullanıcının id'sini döndürür
        /// </summary>
        /// <param name="founderUserId">Kurucu kullanıcı id</param>
        /// <returns>Rastgele kullanıcı id</returns>
        public string RandomCompetitorUserId(string founderUserId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.RandomCompetitorUserId\"");
            Check.IsNullOrEmpty(founderUserId, nameof(founderUserId));

            return _duelInfoRepository.RandomCompetitorUserId(founderUserId);
        }

        /// <summary>
        /// Kullanıcı adına göre yarışmalardaki kazanma kaybetme istatislik bilgisini verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Kategorilerdeki istatiksel bilgi modeli</returns>
        public StatisticsInfoModel SelectedContestStatistics(string userName, int subCateogryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.SelectedContestStatistics\"");
            Check.IsNullOrEmpty(userName, nameof(userName));

            return _duelInfoRepository.SelectedContestStatistics(userName, subCateogryId);
        }

        /// <summary>
        /// Parametreden gelen düello id'sini yenildi yapar
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <param name="duelId">Düello id</param>
        public void SmotherDuel(string userId, int duelId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.SmotherDuel\"");
            Check.IsNullOrEmpty(userId, nameof(userId));
            Check.IsLessThanZero(duelId, nameof(duelId));

            _duelInfoRepository.SmotherDuel(userId, duelId);
        }

        /// <summary>
        /// Alt kategori Id'ye göre kullanıcının o kategorideki çözmüş olduğu soruları yüzdesel olarak döndürür
        /// Alt kategori id nesne oluştururken verildi !!!
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Yüzdesel çözülen sorular</returns>
        public decimal SolvedQuestions(string userId, int subCategoryId)
        {
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.SolvedQuestions\"");
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _duelInfoRepository.SolvedQuestions(userId, subCategoryId);
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
            LoggingService.LogInformation($"Executed action \"ContestPark.BusinessLayer.Services.DuelInfoManager.TrueAnswerControl\"");
            Check.IsNullOrEmpty(DuelInfoId, nameof(DuelInfoId));
            Check.IsLessThanZero(questionId, nameof(questionId));
            Check.IsNullOrEmpty(userId, nameof(userId));

            return _duelInfoRepository.TrueAnswerControl(DuelInfoId, time, questionId, stylish, userId);
        }

        #endregion Methods
    }
}