using Microsoft.Identity.Client;

namespace WebDating.Entities.PostEntities
{
    public class ReactionLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public ReactionType ReactionType { get; set; }

        public int? CommentId { get; set; }
        public int? PostId { get; set; }
        public ReactTarget Target { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Post Post { get; set; }
    }
    public enum ReactionType
    {
        Like = 0,
        Love = 1,
        Haha = 2,
        Wow = 3,
        Sad = 4,
        Angry = 5,
    }

    public enum ReactTarget
    {
        Post = 0,
        Comment = 1,
    }
}
