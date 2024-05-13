using WebDating.Entities.NotificationEntities;

namespace WebDating.DTOs
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string Content { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
