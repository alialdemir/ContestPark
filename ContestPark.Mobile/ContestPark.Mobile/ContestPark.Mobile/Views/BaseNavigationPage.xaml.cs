﻿using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseNavigationPage : NavigationPage, INavigationPageOptions
    {
        #region Constructors

        public BaseNavigationPage()
        {
            InitializeComponent();
            BarTextColor = (Color)ContestParkApp.Current.Resources["Black"];
        }

        #endregion Constructors

        #region Property

        public bool ClearNavigationStackOnNavigation
        {
            get { return true; }
        }

        #endregion Property
    }
}