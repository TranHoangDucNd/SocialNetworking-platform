using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDating.Entities.NotificationEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Entities.PostEntities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Chỉ định Id comment cha (nếu ParentId !=0 tức đây là comment trả lời)
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Cấp của comment, tạm lưu chưa xử lý
        /// </summary>
        public int Level { get; set; } = 1;
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "dateTime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "dateTime")]//chỉ định kiểu dl trong db là dateTime
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 1-n relationship: 1 Comment thì có danh sách các reation histories
        /// </summary>
        public virtual Post Post { get; set; }
        public virtual List<ReactionLog> ReactionLogs { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
