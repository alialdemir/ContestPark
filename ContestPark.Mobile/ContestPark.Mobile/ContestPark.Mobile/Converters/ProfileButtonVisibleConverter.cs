using Autofac;
using ContestPark.Mobile.Configs;
using ContestPark.Mobile.Modules;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ContestPark.Mobile.Converters
{
    public class ProfileButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IUserDataModule userDataModule = RegisterTypesConfig.Container.Resolve<IUserDataModule>();
            if (userDataModule.UserModel == null || value == null)
                return false;

            return userDataModule.UserModel.UserId != value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IUserDataModule userDataModule = RegisterTypesConfig.Container.Resolve<IUserDataModule>();
            if (userDataModule.UserModel == null || value == null)
                return false;

            return userDataModule.UserModel.UserId != value.ToString();
        }
    }
}