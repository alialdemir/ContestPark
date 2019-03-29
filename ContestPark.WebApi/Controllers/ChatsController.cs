using ContestPark.WebApi.Hubs;

namespace ContestPark.WebApi.Controllers
{
    public class ChatsController : BaseHubController<ContestParkHub>
    {
        #region Private Variables

        private readonly IChatService _chatService;
        private readonly IChatBlockService _chatBlockService;
        private readonly IUserService _userService;

        #endregion Private Variables

        #region Constructors

        public ChatsController(
            IConnectionManager signalRConnectionManager,
            IChatService chatService,
            IChatBlockService chatBlockService,
            IUserService userService) : base(signalRConnectionManager)
        {
            _chatService = chatService;
            _chatBlockService = chatBlockService;
            _userService = userService;
        }

        #endregion Constructors

        #region SignalR

        [HttpPost]
        public IActionResult SendChat([FromBody]SendChatModel sendChat)
        {
            #region Validations

            // Not :
            // sendChat.PublicKey değişkeni şu işe yarıyor signalr seft-host olarak çalıştırdığımız için dışardan x bir kişide bizim chati kullanabilir
            // Fakat biz key ile sadece bizim clientlerimizde(mobil tarafı veya web tarafı yada yarın öbürgün farklı platformda olabilir..) bulunan bir key ile karşılaştırıyoruz böylece bizim clientimizden chat isteği geldiğini böyle anlıyoruz...
            if (sendChat.PublicKey != "675b5dce-10cc-4bcd-b635-1e911f6c4eaa") Check.BadStatus("ServerMessage_keyFalse");
            else if (String.IsNullOrEmpty(sendChat.Message) && String.IsNullOrEmpty(sendChat.ReceiverId)) Check.BadStatus("RequiredFields");
            else if (_chatBlockService.BlockingStatus(UserId, sendChat.ReceiverId)) Check.BadStatus("ServerMessage_userBlocked");

            #endregion Validations

            _chatService.Insert(new DataAccessLayer.Tables.Chat()
            {
                SenderId = UserId,
                ReceiverId = sendChat.ReceiverId,
            }, sendChat.Message, UserFullName);

            var fromUser = ContestParkHub.ActiveUsers.Where(x => x.UserId == sendChat.ReceiverId).ToList();
            if (fromUser != null && fromUser.Count > 0)
            {
                string currentUserName = ContestParkHub.ActiveUsers.FirstOrDefault(p => p.UserId == UserId).UserName;
                string currentUserProfilePicturePath = _userService.GetProfilePictureByUserId(UserId);
                foreach (var item in fromUser)
                {
                    Clients.Client(item.ConnectionId).Send(new ChatHistoryModel
                    {
                        Date = DateTime.Now,
                        Message = sendChat.Message,
                        PicturePath = currentUserProfilePicturePath,
                        SenderId = UserId,
                        UserName = currentUserName
                    });
                }
            }
            return Ok();
        }

        #endregion SignalR

        #region Services

        /// <summary>
        /// Online olma sayısına göre kullanıcı listesi
        /// </summary>
        /// <param name="paging">Sayfalama 0 dan baþlar ve 10 ar 10 ar artar 10 dan küçük data geliyorsa sayfa sonudur</param>
        /// <returns>kullanıcı listesi</returns>
        [HttpGet]
        public IActionResult ChatPeople([FromQuery]PagingModel pagingModel, [FromQuery]string search = "")
        {
            ServiceModel<ChatPeopleModel> chatPeopleList = _chatService.ChatPeople(UserId, search, pagingModel);
            foreach (var item in ContestParkHub.ActiveUsers)
            {
                ChatPeopleModel chatPeople = chatPeopleList
                    .Items
                    .Where(p => p.UserId == item.UserId)
                    .FirstOrDefault();
                if (chatPeople != null)
                {
                    chatPeople.OnlineStatus = item.OnlineStatus;
                    chatPeople.LastActiveDate = item.LoginDate;//Zaten aktif ise singalr ile son aktif olma tarihini aldýk deðilse veri tabanýndaki gelir zaten
                }
            }
            if (chatPeopleList.Items != null) chatPeopleList.Items = chatPeopleList.Items.OrderByDescending(p => p.LastActiveDate).ToList();
            return Ok(chatPeopleList);
        }

        /// <summary>
        /// kullanıcılar arasýndaki sohbet geçmiþi
        /// </summary>
        /// <param name="senderUserId ">kullanıcı Id</param>
        /// <returns>Sohbet geçmişinin listesi</returns>
        [HttpPost("{senderUserId}")]
        public IActionResult ChatHistory(string senderUserId, [FromQuery]PagingModel pagingModel)
        {
            string receiverId = UserId;
            return Ok(_chatService.ChatHistory(receiverId, senderUserId, pagingModel));
        }

        /// <summary>
        /// Login olan kullanıcının mesaj listesi
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("User")]
        public IActionResult UserChatList([FromQuery]PagingModel pagingModel)
        {
            return Ok(_chatService.UserChatList(UserId, pagingModel));
        }

        #endregion Services

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (_chatService != null)
        //    {
        //        _chatService.Dispose();
        //        _chatService = null;
        //    }
        //    if (_pictureService != null)
        //    {
        //        _pictureService.Dispose();
        //        _pictureService = null;
        //    }
        //    if (_chatBlockService != null)
        //    {
        //        _chatBlockService.Dispose();
        //        _chatBlockService = null;
        //    }
        //}
    }
}