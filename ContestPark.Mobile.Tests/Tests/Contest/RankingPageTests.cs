namespace ContestPark.Mobile.Tests.Tests.Contest
{
    [Category("Ranking Page")]
    [TestFixture(Platform.Android)]
    public class RankingPageTests
    {
        private IApp app;
        private Platform platform;

        public RankingPageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
            app.WaitForElement(x => x.Text("Takımlar"));
            app.Tap(x => x.Text("Takımlar"));
            app.WaitForElement("Ranking");
            app.Tap("Ranking");
        }

        /// <summary>
        /// Title alanı doğru verilmiş mi?
        /// </summary>
        [Test]
        public void Title_Control()
        {
            app.WaitForElement("Takımlar");
        }

        /// <summary>
        /// Meesaj listesindeki profil fotoğraflarına tıklayınca profile gidiyor mu
        /// </summary>
        [Test]
        public void Click_Profile_Photo()
        {
            app.WaitForElement("imgProfilePhoto");
            app.Tap("imgProfilePhoto");
        }

        /// <summary>
        /// Takip ettiklerim segmentine tıklayınca gidiyor mu
        /// </summary>
        [Test]
        public void Following_Segment_Click()
        {
            app.WaitForElement(x => x.Text(ContestParkResources.Following));
            app.Tap(x => x.Text(ContestParkResources.Following));
            app.WaitForElement("imgProfilePhoto");
        }

        /// <summary>
        /// Genel segmentine tıklayınca gidiyor mu
        /// </summary>
        [Test]
        public void OtherUsers_Segment_Click()
        {
            app.WaitForElement(x => x.Text(ContestParkResources.Following));
            app.Tap(x => x.Text(ContestParkResources.Following));

            app.WaitForElement(x => x.Text(ContestParkResources.Global));
            app.Tap(x => x.Text(ContestParkResources.Global));
            app.WaitForElement("imgProfilePhoto");
        }
    }
}