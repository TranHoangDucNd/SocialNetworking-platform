using System.Threading.Tasks;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities.NotificationEntities;
using WebDating.Entities.PostEntities;

namespace WebDating.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationVM>> GetNewest(int userId, int limit);
        Task<IEnumerable<NotificationVM>> GetUnread(int userId, int limit);
        Task<ResultDto<string>> Delete(int notificationId, int userId);
        Task<ResultDto<string>> MarkAsRead(int notificationId, int userId);
        string GenerateNotificationContent(string fullname, NotificationType notificationType);
        Task SendNotification(int userId, Notification notification);

    }
}
