namespace WebDating.DTOs.Post
{
    public class CommentPostDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        public string PostId { get; set; } = String.Empty;
        public UserShortDto? UserShort { get; set; }
        public string Content { get; set; } = String.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public List<SubCommentDto>? SubComment { get; set; }

    }
}
