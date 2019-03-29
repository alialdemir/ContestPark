namespace ContestPark.Mobile.Tests.Tests.Chat
{
    [Category("Chat Details Page")]
    [TestFixture(Platform.Android)]
    public class ChatDetailsPageTests
    {
        private IApp app;
        private Platform platform;

        public ChatDetailsPageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
            app.WaitForElement("lblSubCategoryName");
            app.SwipeRightToLeft();// Mesajlar tab geçtik
            app.Tap("lstViewChat");// Mesajın detayına gittik
        }

        /// <summary>
        /// Sohbet detayı listeleniyor mu?
        /// </summary>
        [Test]
        public void Are_Messages_Detail_Listed()
        {
            app.WaitForElement("txtChatbox");
        }

        /// <summary>
        /// Mesaj yazıp gönderme
        /// </summary>
        [Test]
        public void Send_Message()
        {
            app.WaitForElement("txtChatbox");
            app.EnterText("txtChatbox", "deneme");
            app.DismissKeyboard();
            app.Tap("btnBtnSendMessage");
        }

        /// <summary>
        /// Boş mesaj yazıp gönderme
        /// </summary>
        [Test]
        public void Empty_Message_Send()
        {
            app.WaitForElement("txtChatbox");
            app.ClearText("txtChatbox");
            app.DismissKeyboard();
            app.Tap("btnBtnSendMessage");
        }
    }
}