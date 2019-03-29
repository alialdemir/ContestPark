using Autofac;
using ContestPark.Mobile.Configs;
using ContestPark.Mobile.Modules;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class DuelResultButtonsHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;
            string competitorUserName = value.ToString();
            string founderUserName = parameter.ToString();
            return true;

            IUserDataModule userDataModule = RegisterTypesConfig.Container.Resolve<IUserDataModule>();

            return !(userDataModule.UserModel.UserName == competitorUserName || userDataModule.UserModel.UserName == founderUserName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}