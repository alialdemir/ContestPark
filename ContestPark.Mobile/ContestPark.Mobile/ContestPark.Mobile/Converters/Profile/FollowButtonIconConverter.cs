using System;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class FollowButtonIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "ic_favorite_white_24dp.png" : "ic_favorite_border_white_24dp.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "ic_favorite_white_24dp.png" : "ic_favorite_border_white_24dp.png";
        }
    }
}