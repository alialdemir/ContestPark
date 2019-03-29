using ContestPark.Entities.Enums;
using ContestPark.Extensions;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Models;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Services
{
    public class LanguageService : ILanguageService
    {
        #region Private variable

        private ISQLiteService<LanguageModel> _SQLiteService;

        #endregion Private variable

        #region Property

        /// <summary>
        /// Kullanıcının o anki dil seçimini döndürür
        /// </summary>
        private Languages _language;

        public Languages Language { get { return _language; } }

        #endregion Property

        #region Constructors

        public LanguageService(ISQLiteService<LanguageModel> SQLiteService)
        {
            _SQLiteService = SQLiteService;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Dil koduna göre dili değiştir
        /// </summary>
        /// <param name="languageName">Dil kodu</param>
        public void SetUserLanguage(string languageName)
        {
            SetUserLanguage(new CultureInfo(languageName));
        }

        /// <summary>
        /// Resource ve ilgili Thread'lara dil bilgisini ekler ve Sqlite kısmında da günceller/ekler
        /// </summary>
        /// <param name="cultureInfo">Dil bilgisi</param>
        public void SetUserLanguage(CultureInfo cultureInfo)
        {
            ContestParkResources.Culture = cultureInfo;
            DependencyService.Get<ILocalize>().SetCultureInfo(cultureInfo);
            _language = LanguageExtension.ToLanguagesEnum(cultureInfo.Name);
            EntityInsert(_language);
        }

        /// <summary>
        /// SqlLite tablosunda dil kaydı varsa onu set eder yoksa telefonun dil bilgisini set eder
        /// </summary>
        public void SetDefaultLanguage()
        {
            var languageModelLocal = _SQLiteService.First();
            if (languageModelLocal != null) SetUserLanguage(languageModelLocal.Language.ToLanguageCode());
            else// Telefonun dil bilgisi yüklendi
            {
                CultureInfo cultureInfo = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                SetUserLanguage(cultureInfo);
            }
        }

        /// <summary>
        /// Dil tablosudan kayıt varsa güncelleme yoksa ekleme yapar
        /// </summary>
        /// <param name="language"></param>
        public void EntityInsert(Languages language)
        {
            var languageModelLocal = _SQLiteService.First();
            if (languageModelLocal != null)
            {
                languageModelLocal.Language = language;
                _SQLiteService.Update(languageModelLocal);
            }
            else _SQLiteService.Insert(new LanguageModel { Language = language });
        }

        #endregion Methods
    }

    public interface ILanguageService
    {
        void EntityInsert(Languages language);

        void SetDefaultLanguage();

        void SetUserLanguage(CultureInfo cultureInfo);

        void SetUserLanguage(string languageName);

        Languages Language { get; }
    }
}