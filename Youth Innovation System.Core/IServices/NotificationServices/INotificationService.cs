using Youth_Innovation_System.Core.Entities;

namespace Youth_Innovation_System.Core.IServices.NotificationServices
{
    public interface INotificationService
    {
        Task NotifyAsync(string userId, string message);

        Task<IReadOnlyList<Notification>> GetAllNotificationsAsync();

        Task MarkNotificationAsReadAsync(int NotifyId);
    }
}
