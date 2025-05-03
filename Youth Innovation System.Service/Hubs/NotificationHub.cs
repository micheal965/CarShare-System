using Microsoft.AspNetCore.SignalR;

namespace Youth_Innovation_System.Service.Hubs
{
    public class NotificationHub : Hub
    {
        [HubMethodName("sendnotification")]
        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
