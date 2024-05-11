using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class CommentVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int ParentCommentId { get; set; }
        public string Content { get; set; }
        public Dictionary<ReactionType, int> Stats { get; set; }
        public List<CommentVM> Descendants { get; set; } = new List<CommentVM>();
        public string CreateAt { get; set; }
    }
}
