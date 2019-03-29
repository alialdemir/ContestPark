using System;
using System.Linq;

namespace ContestPark.Mobile.Tests.Tests.Contest
{
    [Category("Categories Page")]
    [TestFixture(Platform.Android)]
    public class CategoriesPageTests
    {
        private IApp app;
        private Platform platform;

        public CategoriesPageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        /// <summary>
        /// Title alanı doğru verilmiş mi?
        /// </summary>
        [Test]
        public void Title_Control()
        {
            app.WaitForElement(ContestParkResources.Categories);
        }

        /// <summary>
        /// Kategoriye uzun basınca çıkan menüden kategoriyi paylaş tıklanıyor mu
        /// </summary>
        [Test]
        public void SubCategory_Share()
        {
            app.WaitForElement("lblSubCategoryName");
            app.TouchAndHold(p => p.Marked("lblSubCategoryName"));

            string text = ContestParkResources.Share;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            Assert.IsTrue(alertMessaTextControl);
            app.Tap(p => p.Text(text));
        }

        /// <summary>
        /// Kategoriye uzun basınca çıkan menüden kategoriyi takip et çalışıyor mu
        /// </summary>
        [Test]
        public void SubCategory_Long_Press_And_Click_Follow()
        {
            app.WaitForElement("lblSubCategoryName");
            app.TouchAndHold(p => p.Marked("lblSubCategoryName"));

            string text = ContestParkResources.Follow;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            Assert.IsTrue(alertMessaTextControl);
            app.Tap(p => p.Text(text));
        }

        /// <summary>
        /// Takip ettiği kategoriyi takipten çıkarıyor mu
        /// Eğer takip ettiği hiç kategori yoksa fail verir
        /// </summary>
        [Test]
        public void SubCategory_Long_Press_And_Click_UnFollow()
        {
            app.WaitForElement("lblSubCategoryName");
            string followingSubCategoryName = app.Query(p => p.Text(ContestParkResources.FollowingCategories))?.FirstOrDefault().Text;

            if (String.IsNullOrEmpty(followingSubCategoryName)) Assert.Fail("Takip ettiği kategori yok");

            app.TouchAndHold(p => p.Marked("lblSubCategoryName"));
            string text = ContestParkResources.UnFollow;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            Assert.IsTrue(alertMessaTextControl);
            app.Tap(p => p.Text(text));
        }

        /// <summary>
        /// Alt kategoriye uzun basınca çıkan seçeneklerden 'Sıralama' tıklama
        /// </summary>
        [Test]
        public void SubCategory_Long_Press_And_Click_Ranking()
        {
            app.TouchAndHold(p => p.Marked("Takımlar"));
            string text = ContestParkResources.Ranking;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            app.Tap(p => p.Text(text));
            Assert.IsTrue(alertMessaTextControl);
        }

        /// <summary>
        /// Alt kategoriye uzun basınca çıkan seçeneklerden 'Başka rakip' tıklama
        /// </summary>
        [Test]
        public void SubCategory_Long_Press_Click_And_Other_Opponent()
        {
            app.TouchAndHold(p => p.Marked("Takımlar"));
            string text = ContestParkResources.OtherOpponent;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            app.Tap(p => p.Text(text));
            Assert.IsTrue(alertMessaTextControl);
        }

        /// <summary>
        /// Kilitli kategorinin kilidini açma
        /// Kilitli Aç? alert için Kilidi Aç seçeneğine basınca kategori açılıyor mu
        /// Eğer kategori açılırken hata oluşursa displayalert çıkar eğer çıkmazsa kategori açılmıştır
        /// </summary>
        [Test]
        public void Unlock_SubCategory_Open()
        {
            app.Tap(x => x.Marked("imgUnlock"));
            app.WaitForElement("message");
            app.Tap("button1");
            app.WaitForElement("message");
            bool alertErrorMessaTextControl = ContestParkResources.YouDoNotHaveASufficientAmountOfGoldToOpenThisCategory == app.Query("message").First().Text;
            Assert.IsFalse(alertErrorMessaTextControl);
        }

        /// <summary>
        /// Kilitli kategoriye basınca bu kategori kitli mesajı çıkıyor mu
        /// </summary>
        [Test]
        public void Unlock_SubCategory_Message_Control()
        {
            app.Tap(x => x.Marked("imgUnlock"));
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.CategoryLocked, app.Query("message").First().Text);
        }

        /// <summary>
        /// Takımlar alt kategorisine tıklayınca EnterPage açılıyor mu
        /// </summary>
        [Test]
        public void Tap_SubCategory_For_Football()
        {
            app.Tap(x => x.Text("Takımlar"));
            var Football = app.Query(p => p.Text("Takımlar")).SingleOrDefault();
            Assert.IsNotNull(Football);
        }

        /// <summary>
        /// Tümünü göre basınca gidiyor mu?
        /// </summary>
        [Test]
        public void Tap_SeeAll()
        {
            app.Tap("SeeAll");
            var seeAll = app.Query(p => p.Marked("lstSearchCategories")).FirstOrDefault();
            Assert.IsNotNull(seeAll);
        }

        /// <summary>
        /// Toolbar'daki kategori ara iconuna tıklıyor mu
        /// </summary>
        //////[Test]
        //////public void Tap_Search()
        //////{
        //////    app.Tap("tbiCategorySearch");
        //////    var seeAll = app.Query(p => p.Marked("lstSearchCategories")).FirstOrDefault();
        //////    Assert.IsNotNull(seeAll);
        //////}
    }
}