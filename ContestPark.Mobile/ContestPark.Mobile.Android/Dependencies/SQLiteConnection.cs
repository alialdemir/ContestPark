[assembly: Xamarin.Forms.Dependency(typeof(ContestPark.Mobile.Droid.Dependencies.SQLiteConnection))]
namespace ContestPark.Mobile.Droid.Dependencies
{
    public class SQLiteConnection : ISQLiteConnection
    {
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(documentPath, UserDataModule.SQLiteDbName);
            var platform = new SQLitePlatformAndroid();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);
            return connection;
        }
    }
}