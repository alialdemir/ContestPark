using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Enums;
using System;
using System.Linq;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfCpDal : EfEntityRepositoryBase<Cp>, ICpRepository
    {
        #region Constructors

        public EfCpDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Cp tablosunda üyenin hiç altın kayıdı yoksa o üyeyi Cp tablosuna 0 altın ile ekler
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="date">SystemTarihi</param>
        /// <returns>Eklenen üyenin Cp nesnesi</returns>
        public Cp AddNonGoldUser(string userId, DateTime date)
        {
            Cp Cp = new Cp() { UserId = userId, CpAmount = 0 };
            Insert(Cp);
            return Cp;
        }

        /// <summary>
        /// Altın ekleme
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="addedChips">Eklenecek altın miktarı</param>
        /// <param name="chipProcessName">Altın ekleme işlemi nereden gerçekleşiyor</param>
        public int AddChip(string userId, int addedChips, ChipProcessNames chipProcessName)
        {
            DateTime systemDate = DateTime.Now;
            Cp Cp = DbSet.Where(p => p.UserId == userId).FirstOrDefault();
            if (Cp == null)
            {
                Cp = AddNonGoldUser(userId, systemDate);
            }
            Cp.CpAmount = Cp.CpAmount + addedChips;
            Update(Cp);
            return Cp.CpId;
        }

        /// <summary>
        /// 10000-20000 arası rasgele chip ekler
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Eklenen altın miktarı</returns>
        public int AddRandomChip(string userId, DateTime lastDailyChipDateTime)
        {
            TimeSpan ts = DateTime.Now - lastDailyChipDateTime;
            //if kısmında 100 bin gün sonra giren kullanıcıya günlük altın vermez bug oluşuyor
            if (ts.Days < 100000 && ts.Hours < 12) return 0;//On iki saate bir kere chip vermek için kontrol koyduk
            int randomChip = new Random().Next(10000, 20000);
            AddChip(userId, randomChip, ChipProcessNames.DailyChip);//Günlük chip eklendi
            return randomChip;
        }

        /// <summary>
        /// Store'dan altın satın alınınca alınan altını ekliyor
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="productId">Product Id</param>
        public void BuyChip(string userId, string productId)
        {
            switch (productId)
            {
                case "com.contestparkapp.app.55bin": AddChip(userId, (int)Products.bin55, ChipProcessNames.Buy); break;
                case "com.contestparkapp.app.230bin": AddChip(userId, (int)Products.bin230, ChipProcessNames.Buy); break;
                case "com.contestparkapp.app.680bin": AddChip(userId, (int)Products.bin680, ChipProcessNames.Buy); break;
                case "com.contestparkapp.app.1.6milyon": AddChip(userId, (int)Products.milyon1_6, ChipProcessNames.Buy); break;
                case "com.contestparkapp.app.6milyon": AddChip(userId, (int)Products.milyon6, ChipProcessNames.Buy); break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Duello Id'sine göre duellodaki bahis miktarı kadar kullanıcılardan bahisi düşer
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        public void ChipDistribution(int duelId)
        {
            Duel duel = DbContext
                .Set<Duel>()
                .Where(p => p.DuelId == duelId)
                .FirstOrDefault();
            if (duel.Cp > 0)
            {
                int duelChips = duel.Cp;
                duel.Cp = -duel.Cp;
                SaveChanges();
                var duelUserInfo = DbContext
                    .Set<DuelInfo>()
                    .Where(p => p.DuelId == duelId)
                    .Select(di => new
                    {
                        TrueAnswer = di.TrueAnswer,
                        FounderTrueAnswerCount = di.FounderUserAnswer,//Kurucunun doğru cevap sayısı
                        CompetitorUserAnswerCount = di.CompetitorUserAnswer//Rakibin doğru cevap sayısı
                    }).ToList();
                int founderTrueAnswerCount = duelUserInfo.Where(p => p.FounderTrueAnswerCount == (Stylish)p.TrueAnswer).Count();
                int competitorUserAnswerCount = duelUserInfo.Where(p => p.CompetitorUserAnswerCount == (Stylish)p.TrueAnswer).Count();
                if (founderTrueAnswerCount > competitorUserAnswerCount) this.AddChip(duel.FounderUserId, duelChips, ChipProcessNames.Win);//Kurucu kazanırsa
                else if (competitorUserAnswerCount > founderTrueAnswerCount) this.AddChip(duel.CompetitorUserId, duelChips, ChipProcessNames.Win);//Rakib kazanırsa
                else if (competitorUserAnswerCount == founderTrueAnswerCount)//Berabera biterse chipler geri iade ediliyor
                {
                    int chips = duelChips / 2;//düellodaki chipleri ikiye bölüp başlaştırdık
                    this.AddChip(duel.CompetitorUserId, chips, ChipProcessNames.Win);
                    this.AddChip(duel.FounderUserId, chips, ChipProcessNames.Win);
                }
            }
        }

        /// <summary>
        /// Bu method bot ile oynanan duellolarda düello sonunda kurucu kazanırsa bahisini alması için
        /// </summary>
        /// <param name="duelId">Düello Id</param>
        /// <param name="FounderScore">Düello kurucusu puanı</param>
        /// <param name="CompetitorScore">Rakip oyuncu puanı</param>
        public void ChipDistribution(int duelId, byte FounderScore, byte CompetitorScore)
        {
            Duel duel = DbContext
                 .Set<Duel>()
                 .Where(p => p.DuelId == duelId)
                 .FirstOrDefault();
            if (duel.Cp > 0)
            {
                int duelChips = duel.Cp;
                duel.Cp = -duel.Cp;
                SaveChanges();
                var duelUserInfo = DbContext
                    .Set<DuelInfo>()
                    .Where(p => p.DuelId == duelId)
                    .Select(di => new
                    {
                        TrueAnswer = di.TrueAnswer,
                        FounderTrueAnswerCount = di.FounderUserAnswer,//Kurucunun doğru cevap sayısı
                        CompetitorUserAnswerCount = di.CompetitorUserAnswer//Rakibin doğru cevap sayısı
                    }).ToList();
                if (FounderScore > CompetitorScore) this.AddChip(duel.FounderUserId, duelChips, ChipProcessNames.Win);//Kurucu kazanırsa
                else if (CompetitorScore == FounderScore)//Berabera biterse chipler geri iade ediliyor
                {
                    int chips = duelChips / 2;//düellodaki chipleri ikiye bölüp başlaştırdık
                    this.AddChip(duel.FounderUserId, chips, ChipProcessNames.Win);
                }
            }
        }

        /// <summary>
        /// Kullanıcı Id'sine göre toplam altın miktarını verir
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Toplam altın miktarı</returns>
        public int UserTotalCp(string userId)
        {
            return DbContext.Set<Cp>()
                            .Where(p => p.UserId == userId)
                            .Select(p => p.CpAmount)
                            .FirstOrDefault();
        }

        /// <summary>
        /// Kullanıcının altınını azaltma
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="diminishingChips">Azaltılcak altın miktarı</param>
        /// <param name="chipProcessName">Nereden azaltıldığı bilgisi</param>
        public int RemoveChip(string userId, int diminishingChips, ChipProcessNames chipProcessName)
        {
            if (diminishingChips <= 0)
                return 0;

            DateTime systemDate = DateTime.Now;
            Cp Cp = DbSet.Where(p => p.UserId == userId).FirstOrDefault();
            if (Cp == null)
            {
                Cp = AddNonGoldUser(userId, systemDate);
            }

            if (Cp.CpAmount <= 0) BadStatus("ServerMessage_insufficientGold");

            Cp.CpAmount = Cp.CpAmount - diminishingChips;
            if (Cp.CpAmount < 0) Cp.CpAmount = 0;
            Update(Cp);
            return Cp.CpId;
        }

        /// <summary>
        /// kullanıcının chip istenilen chipden fazla mı? fazla ise true değilse false
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="amountRequired">Karşılaştırılacak altın miktarı</param>
        /// <returns>kullanıcının chip istenilen chipden fazla mı? fazla ise true değilse false</returns>
        public bool UserChipEquals(string userId, int amountRequired)
        {
            return DbSet
                    .Where(Cp => Cp.UserId == userId && Cp.CpAmount >= amountRequired)
                    .Any();
        }

        #endregion Methods
    }
}