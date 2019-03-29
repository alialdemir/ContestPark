using ContestPark.Entities.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class NotificationDuelButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NotificationTypes notification = (NotificationTypes)value;
            return NotificationTypes.Contest.HasFlag(notification);// Gelen value düello bildirimi ise true değilse false döner
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}