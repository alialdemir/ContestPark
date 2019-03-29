namespace ContestPark.Mobile.Services.Base
{
    public class ServiceBase
    {
        #region MyRegion

        public ServiceBase(IRequestProvider requestProvider)
        {
            RequestProvider = requestProvider;
        }

        #endregion MyRegion

        #region Property

        public IRequestProvider RequestProvider { get; private set; }

        #endregion Property
    }
}