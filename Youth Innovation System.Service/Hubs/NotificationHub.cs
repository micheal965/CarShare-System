using Microsoft.AspNetCore.SignalR;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Service.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user.IsInRole(UserRoles.Admin.ToString()))
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.IsInRole(UserRoles.Admin.ToString()))
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");

            await base.OnDisconnectedAsync(exception);
        }
        //[HubMethodName("sendnotification")]
        //public async Task SendNotificationToAdminAsync(string userId, string message)
        //{
        //    await Clients.Group("Admins").SendAsync("ReceiveNotification", message);
        //}
    }
}
