namespace WebDating.DTOs.Post
{
    public class CommentDto
    {
        public string Fullname { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public int ParentCommentId { get; set; }
        public string Content { get; set; }
        public List<CommentDto> ChildComments { get; set; }
    }
}

