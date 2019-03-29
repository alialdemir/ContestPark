namespace ContestPark.WebApi.Controllers
{
    public abstract class BaseHubController<T> : BaseController where T : Hub
    {
        #region Properties

        public IHubConnectionContext<dynamic> Clients { get; private set; }
        public IGroupManager Groups { get; private set; }

        #endregion Properties

        #region Constructors

        protected BaseHubController(IConnectionManager signalRConnectionManager)
        {
            var _hub = signalRConnectionManager.GetHubContext<T>();
            Clients = _hub.Clients;
            Groups = _hub.Groups;
        }

        #endregion Constructors
    }
}