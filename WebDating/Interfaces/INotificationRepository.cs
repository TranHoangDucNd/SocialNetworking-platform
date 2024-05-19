using WebDating.Entities.NotificationEntities;
using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Interfaces
{
    public interface INotificationRepository
    {
        void Insert(Notification notification);
        void UpdateStatus(int id, NotificationStatus status);
        void Delete(int id);
        void Delete(Notification notification);
        Notification GetById(int id);
        Task<List<Notification>> GetAllByUserId(int userId, int limit);
        Task<List<Notification>> GetUnreadByUser(int userId, int limit);

        void RemoveAllByDateId(int datingRequestId);
    }
}
