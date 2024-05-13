using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Entities.NotificationEntities
{
    public class Notification
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int? NotifyFromUserId { get; set; }
        public int NotifyToUserId { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual AppUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }

    }

    public enum NotificationStatus
    {
        Unread = 0,
        Read = 1,
        Hidden = 2,
    }

    public enum NotificationType
    {
        ReactionPost = 0,
        CommentPost = 1,
        ReplyComment = 2,
        ReactionComment = 3,
    }
}
