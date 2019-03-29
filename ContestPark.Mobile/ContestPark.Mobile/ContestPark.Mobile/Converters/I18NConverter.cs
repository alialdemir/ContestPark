using ContestPark.Mobile.AppResources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class I18NConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ContestParkResources.ResourceManager.GetString(value.ToString(), ContestParkResources.Culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ContestParkResources.ResourceManager.GetString(value.ToString(), ContestParkResources.Culture);
        }
    }
}