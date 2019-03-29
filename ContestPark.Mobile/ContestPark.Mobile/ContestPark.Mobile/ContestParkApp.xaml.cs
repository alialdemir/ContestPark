using Autofac;
using ContestPark.Mobile.Configs;
using ContestPark.Mobile.Events;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Prism.Autofac;
using Prism.Autofac.Forms;
using Prism.Events;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ContestPark.Mobile
{
    public partial class ContestParkApp : PrismApplication
    {
        #region Constructors

        public ContestParkApp(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        #endregion Constructors

        #region OnInitialized

        protected override void OnInitialized()
        {
            MobileCenter.Start("android=ae36189b-a623-4ac5-9fc0-b12b877f8636;", typeof(Analytics), typeof(Crashes));
            InitializeComponent();
            NavigationService.NavigateAsync(nameof(Views.MainPage));
        }

        #endregion OnInitialized

        #region Register Types

        protected override void RegisterTypes()
        {
            RegisterTypesConfig.Init(Container);
        }

        #endregion Register Types

        #region OnResume and OnSleep

        protected override void OnResume()
        {
            SignalrConnectStatus(true);
            base.OnResume();
        }

        protected override void OnSleep()
        {
            SignalrConnectStatus(false);
            base.OnSleep();
        }

        /// <summary>
        /// Signalr connection açıp kapatmaya yarar
        /// </summary>
        /// <param name="isConnect"></param>
        private void SignalrConnectStatus(bool isConnect)
        {
            Container
                    .Resolve<IEventAggregator>()
                    .GetEvent<SignalRConnectEvent>()
                    .Publish(isConnect);
        }

        #endregion OnResume and OnSleep
    }
}