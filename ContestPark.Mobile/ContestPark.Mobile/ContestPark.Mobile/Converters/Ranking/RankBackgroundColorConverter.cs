using Autofac;
using ContestPark.Entities.Models;
using ContestPark.Mobile.Configs;
using ContestPark.Mobile.Modules;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class RankBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Color.White;
            int rank = ((ListView)parameter).ItemsSource.Cast<object>().ToList().IndexOf(value);

            IUserDataModule userDataModule = RegisterTypesConfig.Container.Resolve<IUserDataModule>();
            return userDataModule.UserModel.UserName == ((ScoreRankingModel)value).UserName ? "#ff8800" : rank % 2 == 0 ? "#ecf0f1" : "#f9f9f9";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}