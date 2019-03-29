using System.Linq;

namespace ContestPark.Mobile.Tests.Tests.Chat
{
    [Category("Chat All Page")]
    [TestFixture(Platform.Android)]
    public class ChatAllPageTests
    {
        private IApp app;
        private Platform platform;

        public ChatAllPageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
            app.WaitForElement("lblSubCategoryName");
            app.SwipeRightToLeft();// Mesajlar tab geçtik
        }

        /// <summary>
        /// Title alanı doğru verilmiş mi?
        /// </summary>
        [Test]
        public void Title_Control()
        {
            app.WaitForElement(ContestParkResources.Chats);
        }

        /// <summary>
        /// Sohbet listesi listeleniyor mu?
        /// </summary>
        [Test]
        public void Are_Messages_Listed()
        {
            app.WaitForElement("lstViewChat");
        }

        /// <summary>
        /// Mesaj silme testi
        /// </summary>
        [Test]
        public void Message_Delete_Click_Remove()
        {
            app.WaitForElement("lstViewChat");
            app.TouchAndHold("lstViewChat");

            string text = ContestParkResources.Delete;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            app.Tap(p => p.Text(text));
        }

        /// <summary>
        /// Mesaj silme iptal etme
        /// </summary>
        [Test]
        public void Message_Delete_Click_Cancel()
        {
            app.WaitForElement("lstViewChat");
            app.TouchAndHold("lstViewChat");
            string text = ContestParkResources.Cancel;
            bool alertMessaTextControl = text == app.Query(x => x.Text(text)).FirstOrDefault().Text;
            app.Tap(p => p.Text(text));
        }

        /// <summary>
        /// Mesaja tıklayınca detaya gidiyor mu
        /// </summary>
        [Test]
        public void Click_Message()
        {
            app.WaitForElement("lstViewChat");
            app.Tap("lstViewChat");
        }

        /// <summary>
        /// Sağdaki fab buttona tıklama
        /// </summary>
        [Test]
        public void Click_Fab_Button()
        {
            app.WaitForElement("fabUsers");
            app.Tap("fabUsers");
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
    }
}