using ContestPark.WebApi.Hubs;

namespace ContestPark.WebApi.Controllers
{
    public class DuelInfosController : BaseHubController<ContestParkHub>
    {
        #region Private Variables

        private readonly IDuelInfoService _duelInfoService;
        private readonly IDuelService _duelService;
        private readonly ICpService _cpService;
        private readonly IMissionService _missionService;
        private readonly IPostService _postService;
        private readonly IDuelUserService _duelUserService;
        private readonly IGameDuelService _gameDuelService;

        #endregion Private Variables

        #region Constructors

        public DuelInfosController(IDuelInfoService duelInfoService,
            IDuelService duelService,
            ICpService cpService,
            IMissionService missionService,
            IPostService postService,
            IDuelUserService duelUserService,
            IGameDuelService gameDuelService,
            IConnectionManager signalRConnectionManager) : base(signalRConnectionManager)
        {
            _duelInfoService = duelInfoService ?? throw new ArgumentNullException(nameof(duelInfoService));
            _duelService = duelService ?? throw new ArgumentNullException(nameof(duelService));
            _cpService = cpService ?? throw new ArgumentNullException(nameof(cpService));
            _missionService = missionService ?? throw new ArgumentNullException(nameof(missionService));
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
            _duelUserService = duelUserService ?? throw new ArgumentNullException(nameof(duelUserService));
            _gameDuelService = gameDuelService ?? throw new ArgumentNullException(nameof(gameDuelService));
        }

        #endregion Constructors

        #region SignalR

        /// <summary>
        /// Bekleme modunda olduğunu bildirir
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <param name="Cp"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OpenDuel/{subCategoryId}")]
        public async Task<IActionResult> OpenDuel(int subCategoryId, [FromQuery]int Cp)
        {
            if (subCategoryId <= 0)
                return BadRequest();
            await _duelUserService.AddDuelUser(UserId, subCategoryId, Cp);
            return Ok();
        }

        /// <summary>
        /// Bekleme modundan çıkar
        /// </summary>
        [HttpDelete]
        [Route("CloseDuel")]
        public async Task<IActionResult> CloseDuel()
        {
            await _duelUserService.RemoveDuelUser(UserId);
            return Ok();
        }

        /// <summary>
        /// Bekleme modunda olduğunu bildirir
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <param name="Cp"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DuelPlay")]
        public async Task<IActionResult> DuelPlay()
        {
            List<DuelUser> duelUsers = await _duelUserService.GetDuelUsers();
            DuelUser founderUser = duelUsers.Find(p => p.UserId == UserId);
            if (founderUser == null) return BadRequest();

            DuelUser competitorUser = duelUsers.Find(p => p.UserId != founderUser.UserId && p.SubCategoryId == founderUser.SubCategoryId && p.BetAmount == founderUser.BetAmount);// rakip bulduk

            if (competitorUser == null) competitorUser = founderUser;// Eğer rakip bulamazsa bot devreye girer

            if (founderUser.BetAmount > 0)
            {
                bool chipEquals = _cpService.UserChipEquals(UserId, founderUser.BetAmount);//kullanıcının chip istenilen chipden fazla mı? fazla ise true değilse false
                if (!chipEquals) Check.BadStatus("ServerMessage_youDoNotHaveEnoughGoldToPlay");
            }

            if (founderUser.UserId == UserId) founderUser.UserId = _duelInfoService.RandomCompetitorUserId(UserId);// Bota karşı oynamadan önce o saniyelerde canlı rakip var mı ona baktık
            DuelEnterScreenModel duelEnterScreen = _duelInfoService.DuelEnterScreen(UserId, founderUser.UserId, founderUser.SubCategoryId, false, founderUser.BetAmount);
            if (founderUser.BetAmount >= 50000)// Görevlerde en az 50 binlik bahis için görev olduğu için Cp'nin 50 binden fazla ise görevi kontrol etsin böylece her düelloda kontrol etmek zorunda kalmasın
            {
                _missionService.MissionComplete(UserId,
                                                    Missions.Mission17,
                                                    Missions.Mission18,
                                                    Missions.Mission19,
                                                    Missions.Mission20);// Görev yapıldı mı? kontrol ettik
                _missionService.MissionComplete(founderUser.UserId,
                                                    Missions.Mission17,
                                                    Missions.Mission18,
                                                    Missions.Mission19,
                                                    Missions.Mission20);// Görev yapıldı mı? kontrol ettik
            }

            _postService.Update(UserId, founderUser.UserId, duelEnterScreen.DuelId);//Rakibin kime yapıyor listesine bu duelloyu görünür yaptık

            if (founderUser.UserId != competitorUser.UserId)// Rakip varsa
            {
                string founderConnectionId = ContestParkHub.ActiveUsers.Where(p => p.UserId == founderUser.UserId).Select(p => p.ConnectionId).First();
                string competitorConnectionId = ContestParkHub.ActiveUsers.Where(p => p.UserId == competitorUser.UserId).Select(p => p.ConnectionId).First();
                await _gameDuelService.AddGameDuel(new GameDuel
                {
                    FounderConnectionId = founderConnectionId,
                    FounderUserId = UserId,
                    CompetitorConnectionId = competitorConnectionId,
                    CompetitorUserId = founderUser.UserId,
                    DuelId = duelEnterScreen.DuelId
                });
                Clients.Client(competitorConnectionId).GoToDuelScreen(duelEnterScreen);
            }
            await _duelUserService.RemoveDuelUser(founderUser.UserId);

            return Ok(duelEnterScreen);
        }

        ///// <summary>
        ///// Düello başlatma varsa bekleyen kullanıcı yoksa random rakip(bot)
        ///// </summary>
        ///// <param name="subCategoryId">Alt kategori Id</param>
        ///// <returns>Duello giriş ekranı modeli</returns>
        //[HttpPost]
        //[Route("DuelStart/{subCategoryId}")]
        //public IActionResult DuelStart(int subCategoryId, int Cp)//Rasgele rakip bulma
        //{
        //    return Ok(_duelInfoService.DuelStartRandom(UserId, subCategoryId));
        //}

        #endregion SignalR

        #region Services

        /// <summary>
        /// Düello başlatma varsa bekleyen kullanıcı yoksa random rakip(bot)
        /// </summary>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Duello giriş ekranı modeli</returns>
        //////[HttpPost]
        //////[Route("DuelStart/{subCategoryId}")]
        //////public IActionResult DuelStart(int subCategoryId, int Cp)//Rasgele rakip bulma
        //////{
        //////    var user = _duelUserService.get

        //////    return Ok(_duelInfoService.DuelStartRandom(UserId, subCategoryId));
        //////}
        /*  /// <summary>
          /// Düello id'ye göre düello başlıyor ekranı
          /// </summary>
          /// <param name="duelId">Düello id</param>
          /// <returns>Duello giriş ekranı modeli</returns>
          [HttpPost]
          [Route("DuelEnterScreenGetByDuelId/{duelId}/{subCategoryId}")]
          public IActionResult DuelEnterScreenGetByDuelId(int duelId, int subCategoryId)//Düello başlıyor ekranı
          {
              //var _IDuelInfo = new DuelInfoManager(subCategoryId);
              _duelInfoService.SubCategoryId = subCategoryId;
              return Ok(_duelInfoService.DuelEnterScreen(duelId));
          }*/
        /*   /// <summary>
           /// Kullanıcıları belli olan düellonun düello giriş ekranını döndürür
           /// </summary>
           /// <param name="competitorUserId">Rakip kullanıcı id</param>
           /// <returns>Duello giriş ekranı modeli</returns>
           [HttpPost]
           [Route("DuelEnterScreen/{subCategoryId}")]
           public IActionResult DuelEnterScreen([FromBody]string competitorUserId, int subCategoryId)//Düello başlıyor ekranı
           {
               _duelInfoService.SubCategoryId = subCategoryId;
               return Ok(_duelInfoService.DuelEnterScreen(UserId, competitorUserId, true, 0));
           }*/
        /* //// <summary>
         /// Gelen bildirim id'sine göre duellonun kabul edilip başlatılmasını sağlar
         /// </summary>
         /// <param name="notificationId">Bildirim id</param>
         /// <param name="competitorUserId">Rakip kullanıcı id</param>
         [HttpPost]
         [Route("AcceptsDuelWithNotification/{notificationId}/{subCategoryId}")]
         public IActionResult AcceptsDuelWithNotification(int notificationId/*, int subCategoryId*)
         {
             //_duelInfoService.SubCategoryId = subCategoryId;
             _duelInfoService.AcceptsDuelWithNotification(UserId, notificationId);
             return Ok();
         }*/
        /*    /// <summary>
            /// Parametreden gelen düello id'sini yenildi yapar
            /// </summary>
            /// <param name="duelId">Düello id</param>
            /// <param name="userId">Kullanıcı id</param>
            [HttpPost]
            [Route("SmotherDuel/{duelId}/{subCategoryId}")]
            public IActionResult SmotherDuel(int duelId/*, int subCategoryId*)
            {
            //    _duelInfoService.SubCategoryId = subCategoryId;
                _duelInfoService.SmotherDuel(UserId, duelId);
                return Ok();
            }*/

        /// <summary>
        /// Gelen question Id göre diğer yarışmacıların bu soruya verdiği cevaplar
        /// </summary>
        /// <param name="questionId">Soru id</param>
        /// <returns>Verdiği cevaplar</returns>
        [HttpPost]
        [Route("AudienceAnswers/{questionId}")]
        public IActionResult AudienceAnswers(int questionId)
        {
            return Ok(_duelInfoService.AudienceAnswers(questionId));
        }

        /// <summary>
        /// Kullanıcının ilgili kategorideki tamamlanmış düello sayısı eğer _subCategoryId 0 ise tüm kategorilerdeki oyun sayısını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Tamamlanmış toplam düello sayısı</returns>
        [HttpGet]
        [Route("GameCount/{userId}/{subCategoryId}")]
        public IActionResult GameCount(string userId, int subCategoryId)
        {
            return Ok(_duelService.GameCount(userId, subCategoryId));
        }

        /// <summary>
        /// Alt kategori Id'ye göre kullanıcının o kategorideki çözmüş olduğu soruları yüzdesel olarak döndürür
        /// Alt kategori id nesne oluştururken verildi !!!
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Yüzdesel çözülen sorular</returns>
        [HttpPost]
        [Route("SolvedQuestions/{subCategoryId}")]
        public IActionResult SolvedQuestions(int subCategoryId)
        {
            return Ok(_duelInfoService.SolvedQuestions(UserId, subCategoryId));
        }

        /// <summary>
        /// Kullanıcının düello bilgisini verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Düello bilgisi</returns>
        [HttpPost]
        [Route("DuelUserInfo/{duelId}")]
        public IActionResult DuelUserInfo(int duelId)
        {
            return Ok(_duelService.DuelUserInfo(UserId, duelId));
        }

        /// <summary>
        /// Düello id'sine göre düelloyu kim kazandı onun bilgisini geri döndürür
        /// </summary>
        /// <param name="duelId">Düello id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Düello sonucu modeli</returns>
        [HttpPost]
        [Route("DuelResult/{duelId}/{subCategoryId}")]
        public IActionResult DuelResult(int duelId, int subCategoryId)
        {
            return Ok(_duelInfoService.DuelResult(duelId));
        }

        /// <summary>
        /// Kullanıcı adına göre yarışmalardaki kazanma kaybetme istatislik bilgisini verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        /// <returns>Kategorilerdeki istatiksel bilgi modeli</returns>
        [HttpPost]
        [Route("SelectedContestStatistics/{subCategoryId}")]
        public IActionResult SelectedContestStatistics([FromBody]string userName, int subCategoryId)
        {
            return Ok(_duelInfoService.SelectedContestStatistics(userName, subCategoryId));
        }

        /// <summary>
        /// Her yarışma için kaç oyun oynadı onu verir
        /// Kazandığı kaybettiği berabere yada beklemede olanların hepsi
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>Oynadığı yarışmaların istasiksel bilgisi</returns>
        [HttpPost]
        [Route("ContestMostPlay")]
        public IActionResult ContestMostPlay([FromBody]string userName, [FromQuery]PagingModel pagingModel)
        {
            return Ok(_duelInfoService.ContestMostPlay(userName, pagingModel));
        }

        /// <summary>
        /// Oynadığı tüm kategorilerde kazanma kaybetme beraberlik durumunu verir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>İstatiksel bilgi</returns>
        [HttpPost]
        [Route("GlobalStatisticsInfo")]
        public IActionResult GlobalStatisticsInfo([FromBody]string userName)
        {
            return Ok(_duelInfoService.GlobalStatisticsInfo(userName));
        }

        #endregion Services
    }
}