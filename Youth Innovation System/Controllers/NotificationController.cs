using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Youth_Innovation_System.Core.IServices.NotificationServices;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Shared.ApiResponses;

namespace Youth_Innovation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("Get-All-Notifications/")]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        [HttpPut("Mark-Notification-As-Read/{NotifyId}")]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<IActionResult> MarkNotificationAsRead(int NotifyId)
        {
            await _notificationService.MarkNotificationAsReadAsync(NotifyId);
            return Ok(new ApiResponse(StatusCodes.Status200OK, "Notification marked as readed sucessfully"));
        }

    }
}
