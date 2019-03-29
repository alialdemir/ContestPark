namespace ContestPark.Mobile.Tests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform, bool isLogin = false)
        {
            IApp app = ConfigureApp
                      .Android
                     .ApkFile(@"C:\ContestPark\ContestPark.Mobile\ContestPark.Mobile.Android\bin\Release\ContestPark.Mobile.Android-Signed.apk")
                      // .InstalledApp("com.companyname.App1")
                      //       .DeviceSerial("169.254.76.233:5555")
                      //.PreferIdeSettings()
                      .EnableLocalScreenshots()
                      .StartApp();
            if (isLogin)
            {
                //app.ClearText("txtUserName");
                //app.EnterText("txtUserName", "witcherfearless");
                //app.ClearText("txtPassword");
                //app.EnterText("txtPassword", "19931993");
                app.Tap("SignIn");
            }
            //if (platform == Platform.Android)
            //{
            return app;
            //   }

            //return ConfigureApp
            //    .iOS
            //    .StartApp();
        }
    }
}