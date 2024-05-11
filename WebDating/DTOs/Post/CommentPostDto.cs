using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class CommentPostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int ParentCommentId { get; set; }
        public UserShortDto? UserShort { get; set; }
        public string Content { get; set; } = String.Empty;

    }


   
}
