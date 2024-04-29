using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities
{
    [Table("PostLike")]
    public class PostLike
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Post Post { get; set; }

    }
}
