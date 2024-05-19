using Microsoft.EntityFrameworkCore;
using WebDating.Entities.NotificationEntities;
using WebDating.Interfaces;

namespace WebDating.Data
{
    public class NotificationRepository : INotificationRepository
    {
        readonly DataContext _context;
        public NotificationRepository(DataContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            Notification notification = GetById(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }
        }
        public void Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
        }
        public Notification GetById(int id)
        {
            return _context.Notifications.Find(id);
        }

        public void Insert(Notification notification)
        {
            _context.Notifications.Add(notification);
        }

        public void UpdateStatus(int id, NotificationStatus status)
        {
            Notification notification = GetById(id);
            if (notification != null)
            {
                notification.Status = status;
            }
        }
        public async Task<List<Notification>> GetAllByUserId(int userId, int limit)
        {
            return await _context.Notifications.Where(it => it.NotifyToUserId == userId)
                .OrderByDescending(it => it.CreatedDate)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadByUser(int userId, int limit)
        {
            return await _context.Notifications.Where(it => it.NotifyToUserId == userId && it.Status == NotificationStatus.Unread)
                .OrderByDescending(it => it.CreatedDate)
                .Take(limit)
                .ToListAsync();
        }

        public void RemoveAllByDateId(int datingRequestId)
        {
            IEnumerable<Notification> notifications = _context.Notifications.Where(it => it.DatingRequestId == datingRequestId);
            _context.Notifications.RemoveRange(notifications);
        }
    }
}
