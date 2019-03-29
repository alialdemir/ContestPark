using System;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class CategoryTitleBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string titleBackgroundColor = "#fff";//Default beyaz
            if (value == null) return titleBackgroundColor;
            int CategoryId = System.Convert.ToInt32(value);
            switch (CategoryId)
            {
                case 1: titleBackgroundColor = "#F7931B"; break;
                case 6: titleBackgroundColor = "#1DC2DE"; break;
                case 7: titleBackgroundColor = "#47B04B"; break;
                default: titleBackgroundColor = "#B71C1C"; break;
            }
            return titleBackgroundColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}