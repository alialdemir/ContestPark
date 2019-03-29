using System.Globalization;

namespace ContestPark.Mobile.AppResources
{
    public interface ILocalize
    {
        void SetDefaultLocale();

        void SetCultureInfo(CultureInfo cultureInfo);

        CultureInfo GetCurrentCultureInfo();
    }
}