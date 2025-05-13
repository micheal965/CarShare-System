using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IRepositories;
using Youth_Innovation_System.Core.IServices.NotificationServices;
using Youth_Innovation_System.Service.Hubs;

namespace Youth_Innovation_System.Service.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationService(IHubContext<NotificationHub> hubContext, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<Notification>> GetAllNotificationsAsync()
        {
            var Notifications = await _unitOfWork.Repository<Notification>().GetAllAsync();
            return Notifications;
        }


        public async Task MarkNotificationAsReadAsync(int NotifyId)
        {
            var Notification = await _unitOfWork.Repository<Notification>().GetAsync(NotifyId);
            Notification.IsRead = true;
            await _unitOfWork.CompleteAsync();
        }

        [HubMethodName("sendnotification")]
        public async Task NotifyAsync(string userId, string message)
        {
            var notification = new Notification()
            {
                userId = userId,
                Message = message,
                IsRead = false,
            };
            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", notification);
        }
    }
}
