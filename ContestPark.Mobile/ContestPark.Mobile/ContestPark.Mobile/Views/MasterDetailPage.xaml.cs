using ContestPark.Mobile.Events;
using Prism.Events;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage : Xamarin.Forms.MasterDetailPage
    {
        #region Constructors

        public MasterDetailPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator
                        .GetEvent<MasterDetailPageIsPresentedEvent>()
                        .Subscribe((isPresented) => IsPresented = isPresented);
        }

        #endregion Constructors
    }
}