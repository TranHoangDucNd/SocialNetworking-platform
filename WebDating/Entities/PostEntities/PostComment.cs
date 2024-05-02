using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDating.Entities.UserEntities;

namespace WebDating.Entities.PostEntities
{
    [Table("PostComment")]
    public class PostComment
    {
        public PostComment()
        {
            PostSubComments = new HashSet<PostSubComment>();
        }
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "dateTime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "dateTime")]//chỉ định kiểu dl trong db là dateTime
        public DateTime? UpdatedAt { get; set; }
        public Post Post { get; set; }
        public AppUser User { get; set; }
        public ICollection<PostSubComment> PostSubComments { get; set; }

    }
}
