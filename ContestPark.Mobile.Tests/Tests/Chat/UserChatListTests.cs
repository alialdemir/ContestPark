namespace ContestPark.Mobile.Tests.Tests.Chat
{
    [Category("User Chat List Page")]
    [TestFixture(Platform.Android)]
    public class UserChatListTests
    {
        private IApp app;
        private Platform platform;

        public UserChatListTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
            app.WaitForElement("lblSubCategoryName");
            app.SwipeRightToLeft();// Mesajlar tab geçtik
            app.WaitForElement("fabUsers");
            app.Tap("fabUsers");
        }

        /// <summary>
        /// Title alanı doğru verilmiş mi?
        /// </summary>
        [Test]
        public void Title_Control()
        {
            app.WaitForElement(ContestParkResources.NewChat);
        }

        /// <summary>
        /// Kullanıcı listesindeki profil fotoğraflarına tıklayınca profile gidiyor mu
        /// </summary>
        [Test]
        public void Click_Profile_Photo()
        {
            app.WaitForElement("imgProfilePhoto");
            app.Tap("imgProfilePhoto");
        }

        /// <summary>
        /// Kullanıcıya tıklayınca mesaj detayına gidiyor mu
        /// </summary>
        [Test]
        public void Click_Message()
        {
            app.WaitForElement("LstItemView");
            app.Tap("LstItemView");
            app.WaitForElement("btnBtnSendMessage");
        }

        /// <summary>
        /// Search bar da ali aldemir diye aratınca çıkıyor mu
        /// </summary>
        [Test]
        public void SearchBar_Test()
        {
            app.WaitForElement("SearchBar");
            app.EnterText("SearchBar", "meral ");
            app.EnterText("SearchBar", "belinç");
            app.WaitForElement(x => x.Text("meral belinç"));
        }

        /// <summary>
        /// Takip ettiklerim segmentine tıklayınca gidiyor mu
        /// </summary>
        [Test]
        public void Following_Segment_Click()
        {
            app.WaitForElement(x => x.Text(ContestParkResources.Following));
            app.Tap(x => x.Text(ContestParkResources.Following));
            app.WaitForElement("LstItemView");
        }

        /// <summary>
        /// Diğer kullanıcılar segmentine tıklayınca gidiyor mu
        /// </summary>
        [Test]
        public void OtherUsers_Segment_Click()
        {
            app.WaitForElement(x => x.Text(ContestParkResources.Following));
            app.Tap(x => x.Text(ContestParkResources.Following));

            app.WaitForElement(x => x.Text(ContestParkResources.OtherUsers));
            app.Tap(x => x.Text(ContestParkResources.OtherUsers));
            app.WaitForElement("LstItemView");
        }
    }
}