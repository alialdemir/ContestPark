namespace ContestPark.WebApi.Hubs
{
    public class ContestParkHub : Hub<IChatHub>
    {
        public static List<ActiveUsers> ActiveUsers = new List<ActiveUsers>();

        #region Properties

        private string userName = String.Empty;

        public string UserName
        {
            get
            {
                if (String.IsNullOrEmpty(userName)) userName = Context.Headers["UserName"];
                return userName;
            }
        }

        private string userId = String.Empty;

        public string UserId
        {
            get
            {
                if (String.IsNullOrEmpty(userId)) userId = Context.Headers["UserId"];
                return userId;
            }
        }

        #endregion Properties

        public override Task OnConnected()
        {
            if (!String.IsNullOrEmpty(UserId) && !String.IsNullOrEmpty(UserName))
            {
                ActiveUsers.Add(new ActiveUsers
                {
                    //UserIp = UserIp,
                    UserId = UserId,
                    ConnectionId = Context.ConnectionId,
                    LoginDate = DateTime.Now,
                    UserName = UserName
                });
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (!String.IsNullOrEmpty(UserId) && ActiveUsers != null && ActiveUsers.Count > 0 && ActiveUsers.Any(p => p.UserId == UserId)) ActiveUsers.Remove(ActiveUsers.FirstOrDefault(p => p.UserId == UserId));
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IChatHub
    {
    }

    public class ActiveUsers
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }

        //public string UserIp { get; set; }
        public DateTime LoginDate { get; set; }

        public bool OnlineStatus { get; set; }
        public string UserName { get; set; }
        //private List<string> groupList;
        //public List<string> GroupList
        //{
        //    get { return groupList ?? new List<string>(); }
        //    set { groupList = value; }
        //}
    }
}