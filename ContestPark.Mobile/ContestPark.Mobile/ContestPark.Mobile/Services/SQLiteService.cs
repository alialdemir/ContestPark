using ContestPark.Mobile.Models;
using SQLite.Net;
using Xamarin.Forms;

namespace ContestPark.Mobile.Services
{
    public class SQLiteService<T> : ISQLiteService<T> where T : class
    {
        #region Constructors

        public SQLiteService()
        {
            SQLiteConnection = DependencyService
                .Get<ISQLiteConnection>()
                .GetConnection();
            SQLiteConnection
                .CreateTable<UserModel>();
            SQLiteConnection
                .CreateTable<LanguageModel>();
        }

        #endregion Constructors

        #region Properties

        public SQLiteConnection SQLiteConnection { get; set; }

        #endregion Properties

        #region Methods

        public void Insert(T model)
        {
            SQLiteConnection
                .Insert(model);
        }

        public void Update(T model)
        {
            SQLiteConnection
                .Update(model);
        }

        public void Delete(object id)
        {
            SQLiteConnection
                .Delete<T>(id);
        }

        public int Count
        {
            get
            {
                return SQLiteConnection.Table<T>().Count();
            }
        }

        public void Dispose()
        {
            SQLiteConnection.Dispose();
        }

        public T First()
        {
            return SQLiteConnection.Table<T>().FirstOrDefault();
        }

        #endregion Methods
    }

    public interface ISQLiteService<T>
    {
        SQLiteConnection SQLiteConnection { get; set; }

        void Insert(T model);

        void Update(T model);

        void Delete(object id);

        int Count { get; }

        void Dispose();

        T First();
    }

    public interface ISQLiteConnection
    {
        SQLiteConnection GetConnection();
    }
}