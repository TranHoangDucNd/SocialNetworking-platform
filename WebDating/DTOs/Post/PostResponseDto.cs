using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public IEnumerable<string> Images { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserShortDto UserShort { get; set; } = new UserShortDto();
        public int ViewNumber { get; set; } = 0;
        public int CommentNumber { get; set; } = 0;
        public int LikeNumber { get; set; } = 0;
        public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<ReactionLogVM> Reactions { get; set; } = new List<ReactionLogVM>();
    }
}
