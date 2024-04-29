using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities
{
    [Table("PostSubComment")]
    public class PostSubComment
    {
        [Key]
        public int Id { get; set; }
        public int? PreCommentId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual PostComment PreComment { get; set; }
        public virtual AppUser User { get; set; }
    }
}
