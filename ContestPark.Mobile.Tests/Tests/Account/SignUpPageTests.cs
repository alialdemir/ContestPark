using System;
using System.Linq;

namespace ContestPark.Mobile.Tests.Tests.Account
{
    [Category("SignUp Page")]
    [TestFixture(Platform.Android)]
    public class SignUpPageTests
    {
        private IApp app;
        private Platform platform;

        public SignUpPageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            app.Tap("SignUp");
        }

        /// <summary>
        /// Title alanı doğru verilmiş mi?
        /// </summary>
        [Test]
        public void Title_Control()
        {
            app.WaitForElement(ContestParkResources.SignUp);
        }

        /// <summary>
        /// Tüm alanlar girilmez ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_All_Field_Are_Empty()
        {
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.RequiredFields, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı girilmez ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_UserName_Is_Empty()
        {
            app.EnterText("txtFullName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "deneme");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.RequiredFields, app.Query("message").First().Text);
        }

        /// <summary>
        /// Ad soyad girilmez ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_FullName_Is_Empty()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "deneme");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.RequiredFields, app.Query("message").First().Text);
        }

        /// <summary>
        /// Eposta girilmez ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Email_Is_Empty()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "deneme");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.RequiredFields, app.Query("message").First().Text);
        }

        /// <summary>
        /// Şifre girilmez ise mesaj veriyor mu?
        /// </summary>
        [Test]
        public void If_Password_Is_Empty()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.RequiredFields, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı 3 karakterden az girilirse
        /// </summary>
        [Test]
        public void UserName_Min_Length_2_Chacter()
        {
            app.EnterText("txtSignUpUserName", "ab");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_userNameMinLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adı 255 karakterden fazşa girilirse
        /// </summary>
        [Test]
        public void UserName_Max_Length_300_Chacter()
        {
            app.EnterText("txtSignUpUserName", "abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_userNameMaxLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Ad soyad 3 karakterden az girilirse
        /// </summary>
        [Test]
        public void FullName_Min_Length_2_Chacter()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "ab");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_fullNameMinLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Ad soyad 255 karakterden fazla girilirse
        /// </summary>
        [Test]
        public void FullName_Max_Length_300_Chacter()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_fullNameMaxLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Eposta adresi 255 karakterden fazla girilirse
        /// </summary>
        [Test]
        public void Email_Max_Length_300_Chacter()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5abab5");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_emailMaxLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Eposta adresinin formatı yanlış girilirse
        /// </summary>
        [Test]
        public void If_Email_Formating_Is_Incorrect()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "yanlış formatta");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_emailFormating, app.Query("message").First().Text);
        }

        /// <summary>
        /// Password 7 karakterden az girilirse
        /// </summary>
        [Test]
        public void Password_Min_Length_7_Chacter()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_passwordMinLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Şifre 32 karakterden fazla girilirse
        /// </summary>
        [Test]
        public void Password_Min_Length_40_Chacter()
        {
            app.EnterText("txtSignUpUserName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890123456789012345678901234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_passwordMaxLength, app.Query("message").First().Text);
        }

        /// <summary>
        /// Kullanıcı adın da türkçe karahter girilirse
        /// </summary>
        [Test]
        public void UserName_Turkish_Character_Control()
        {
            app.EnterText("txtSignUpUserName", "alişçşüğ");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "deneme");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "deneme@deneme.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "123456789");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_userNameTurkishCharactersCanNot, app.Query("message").First().Text);
        }

        /// <summary>
        /// Var olan kullanıcı adı ile üye olma
        /// </summary>
        [Test]
        public void UserName_Registered()
        {
            app.EnterText("txtSignUpUserName", "witcherfearless");
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "Otomasyon" + Guid.NewGuid().ToString().Substring(0, 5) + "@testotomasyonu.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_thisUserNameWasUsedByAnotherUser, app.Query("message").First().Text);
        }

        /// <summary>
        /// Var olan eposta adresi ile üye olma
        /// </summary>
        [Test]
        public void Email_Registered()
        {
            app.EnterText("txtSignUpUserName", Guid.NewGuid().ToString().Substring(0, 5));
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtEmail", "aldemirali93@gmail.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.ServerMessage_thisEmailAddressUsedByAnotherUser, app.Query("message").First().Text);
        }

        /// <summary>
        /// Tüm bilgiler doğru ise üye olsun
        /// </summary>
        [Test]
        public void Registered()
        {
            string guid = Guid.NewGuid().ToString().Substring(0, 5);
            app.EnterText("txtSignUpUserName", guid);
            app.DismissKeyboard();
            app.EnterText("txtFullName", "adı soyadı");
            app.DismissKeyboard();
            app.EnterText("txtEmail", guid + "@gmail.com");
            app.DismissKeyboard();
            app.EnterText("txtSignUpPassword", "1234567890");
            app.DismissKeyboard();
            app.ScrollDownTo("btnSignUp");
            app.Tap("btnSignUp");
            app.WaitForElement("message");
            Assert.AreEqual(ContestParkResources.WelcomeAboard, app.Query("message").First().Text);
        }
    }
}