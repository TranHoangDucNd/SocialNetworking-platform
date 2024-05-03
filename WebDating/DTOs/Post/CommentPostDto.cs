namespace WebDating.DTOs.Post
{
    public class CommentPostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public UserShortDto? UserShort { get; set; }
        public string Content { get; set; } = String.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<SubCommentDto>? SubComment { get; set; }

    }
}
