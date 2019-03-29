﻿using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class RankConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return 1;
            int rank = ((ListView)parameter).ItemsSource.Cast<object>().ToList().IndexOf(value) + 1;
            return rank;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}