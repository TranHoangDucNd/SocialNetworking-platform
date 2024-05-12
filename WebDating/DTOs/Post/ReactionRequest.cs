using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class ReactionRequest
    {
        public int UserId { get; set; } = 0;
        public int TargetId { get; set; }

        public ReactionType ReactionType { get; set; } = ReactionType.Like;
        public ReactTarget Target { get; set; } = ReactTarget.Post;

    }
}
