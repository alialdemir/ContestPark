using System.Linq;

namespace ContestPark.Mobile.Tests.Tests.Account
{
    [Category("Forget Your Password Page")]
    [TestFixture(Platform.Android)]
    public class ForgetYourPasswordPageTests
    {
        private IApp app;
        private Platform platform;

        public ForgetYourPasswordPageTests(Platform platform)
        {
            this.platform = platform;
        }

        /// <summary>
        /// Title alanı doğru verilmiş mi?
        /// </summary>
        [Test]
        public void Title_Control()
        {
            app.WaitForElement(ContestParkResources.ForgotYourPassword);
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            app.Tap("ForgotYourPassword");
        }

        /// <summary>
        /// Kullanıcı adı veya eposta adresi girilmez ise mesaj veriyor mu_
        /// </summary>
        [Test]
        public void If_UserNameOrEmail_Is_Empty()
        {
            app.Tap("SendPassword");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ForgetYourPasswordLabel1, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı veya eposta adresi girilmez ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_UserName_Is_Incorrect()
        {
            app.EnterText("txtUserNameOrEmail", "yanlış kullanıcı adı");
            app.DismissKeyboard();
            app.Tap("SendPassword");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_ourRegisteredMembersOfThisInformationDoesNotExist, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı doğru ise şifre gönderildi diye mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_UserName_Is_Correct()
        {
            app.EnterText("txtUserNameOrEmail", "elifoz");
            app.DismissKeyboard();
            app.Tap("SendPassword");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_userInfoSendMailAdress, app.Query("message").First().Text);
        }

        /// <summary>
        /// Eposta adresi doğru ise şifre gönderildi diye mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Email_Is_Correct()
        {
            app.EnterText("txtUserNameOrEmail", "aldemirali93@gmail.com");
            app.DismissKeyboard();
            app.Tap("SendPassword");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_userInfoSendMailAdress, app.Query("message").First().Text);
        }
    }
}