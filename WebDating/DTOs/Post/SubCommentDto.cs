using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.DTOs.Post
{
    public class SubCommentDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        [Column(TypeName = "dateTime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "dateTime")]
        public DateTime? UpdatedAt { get; set; }
        public UserShortDto? UserShort { get; set; }
    }
}
