using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDating.Entities.NotificationEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Entities.PostEntities
{
    [Table("Post")]
    public class Post
    {
        public Post()
        {
            PostReportDetails = new HashSet<PostReportDetail>();
        }
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "dateTime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "dateTime")]
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public AppUser User { get; set; }
        public ICollection<ImagePost> Images { get; set; }
        //public ICollection<PostComment> PostComments { get; set; }
        public ICollection<PostReportDetail> PostReportDetails { get; set; }
        public virtual ICollection<ReactionLog> ReactionLogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
