using WebDating.Entities.NotificationEntities;

namespace WebDating.Entities.UserEntities
{
    public class DatingRequest
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int CrushId { get; set; }

        public DatingStatus Status { get; set; }
        public DateTime ConfirmedDate { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Notification> Notifications { get; set; }

        public bool IsInConfirmedRelationship(int userId)
        {
            return (SenderId == userId || CrushId == userId) && Status == DatingStatus.Confirmed;
        }
        public bool IsDenied(int userId)
        {
            return (SenderId == userId || CrushId == userId) && Status == DatingStatus.Denied && (DateTime.Now - ConfirmedDate).TotalDays < 7;
        }

    }

    public enum DatingStatus
    {
        Waiting = 0,
        Confirmed = 1,
        Denied = 2
    }
}


