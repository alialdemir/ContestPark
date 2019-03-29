using System.Linq;

namespace ContestPark.Mobile.Tests.Tests.Account
{
    [Category("SignIn Page")]
    [TestFixture(Platform.Android)]
    public class SignInPageTests
    {
        private IApp app;
        private Platform platform;

        public SignInPageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            app.ClearText("txtUserName");
            app.ClearText("txtPassword");
        }

        /// <summary>
        /// Kullanıcı adı ve şifre boş bırakılırsa mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Username_and_Password_Is_Empty()
        {
            app.Tap("SignIn");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.enterYourUsernamEAndPassword, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı boş bırakılırsa şifre girilirse mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Username_Is_Empty()
        {
            app.EnterText("txtPassword", "19931993");
            app.DismissKeyboard();
            app.Tap("SignIn");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.enterYourUsernamEAndPassword, app.Query("message").First().Text);
        }

        /// <summary>
        /// Şifre boş bırakılırsa mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Password_Is_Empty()
        {
            app.EnterText("txtUserName", "witcherfearless");
            app.DismissKeyboard();
            app.Tap("SignIn");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.enterYourUsernamEAndPassword, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı vev şifre yanlşu ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Username_and_Password_Are_Incorrect()
        {
            app.EnterText("txtUserName", "test");
            app.DismissKeyboard();
            app.EnterText("txtPassword", "test1245");
            app.DismissKeyboard();
            app.Tap("SignIn");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_UsernameOrPasswordIsIncorrect, app.Query("message").First().Text);
        }

        /// <summary>
        /// Üye ol sayfasına gidiyor mu?
        /// </summary>
        [Test]
        public void SignUp_Tap_Navigation()
        {
            app.Tap("SignUp");
            Assert.AreEqual(ContestParkResources.SignUp, app.Query("SignUp").First().Text);
        }

        /// <summary>
        /// Şifremi unuttum sayfasına gidiyor mu?
        /// </summary>
        [Test]
        public void ForgotYourPassword_Tap_Navigation()
        {
            app.Tap("ForgotYourPassword");
            app.EnterText("txtUserNameOrEmail", "DENEME");
            app.DismissKeyboard();
            Assert.AreEqual(ContestParkResources.SendPassword, app.Query("SendPassword").First().Text);
        }
    }
}