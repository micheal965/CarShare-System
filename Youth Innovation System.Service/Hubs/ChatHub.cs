using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IRedis;
using Youth_Innovation_System.Core.IRepositories;
using Youth_Innovation_System.Core.IServices.IChat;
using Youth_Innovation_System.Core.IServices.IHubs;
namespace Youth_Innovation_System.Service.Hubs
{
    // [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IRedisConnectionManager _redisConnectionManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChatService _chatService;
        private readonly UserManager<ApplicationUser> _userManager;


        public ChatHub(IRedisConnectionManager redisConnectionManager,
                      IUnitOfWork unitOfWork,
                      IChatService chatService,
                      UserManager<ApplicationUser> userManager)
        {
            _redisConnectionManager = redisConnectionManager;
            _unitOfWork = unitOfWork;
            _chatService = chatService;
            _userManager = userManager;
        }
        //public override async Task OnConnectedAsync()
        //{
        //    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var connectionId = Context.ConnectionId;
        //    if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(connectionId))
        //    {
        //        await _redisConnectionManager.AddConnectionAsync(userId, connectionId);
        //        await _redisConnectionManager.SetUserOnlineAsync(userId);
        //    }
        //    await base.OnConnectedAsync();
        //}
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userId != null)
        //    {
        //        await _redisConnectionManager.RemoveConnectionAsync(userId);
        //        await _redisConnectionManager.SetUserOfflineAsync(userId);

        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}


        //[HubMethodName("sendmessagetouser")]
        //public async Task SendMessageToUser(string receiverId, string message)
        //{
        //    if (string.IsNullOrWhiteSpace(receiverId) || string.IsNullOrWhiteSpace(message))
        //        throw new ArgumentException("Receiver ID and message cannot be empty.");

        //    var connectionId = await _redisConnectionManager.GetConnectionAsync(receiverId);
        //    var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (senderId == null)
        //        throw new UnauthorizedAccessException("User is not authenticated.");

        //    var user = await _userManager.FindByIdAsync(senderId);
        //    if (user == null)
        //        throw new InvalidOperationException("Sender not found.");

        //    if (connectionId != null)
        //        await Clients.Client(connectionId).ReceiveMessage(user.firstName, message);

        //    var messageObj = new Message
        //    {
        //        SenderId = senderId,
        //        ReceiverId = receiverId,
        //        Content = message,
        //        Timestamp = DateTime.UtcNow,
        //    };
        //    await _unitOfWork.Repository<Message>().AddAsync(messageObj);
        //    await _unitOfWork.CompleteAsync();
        //}
        //[HubMethodName("fetchchathistory")]
        //public async Task<IReadOnlyList<Message>> FetchChatHistory(string receiverUserId)
        //{
        //    var senderUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    return await _chatService.GetChatHistoryAsync(senderUserId, receiverUserId);

        //}
        //[HubMethodName("deletemessage")]
        //public async Task DeleteMessage(int messageId)
        //{
        //    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //        throw new HubException("Unauthorized access.");

        //    var result = await _chatService.DeleteMessageAsync(messageId, userId);

        //    if (result)
        //    {
        //        // Notify clients to remove the message from the UI
        //        await Clients.All.MessageDeleted(messageId);
        //    }
        //}

    }
}
