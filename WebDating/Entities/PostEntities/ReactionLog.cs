namespace WebDating.Entities.PostEntities
{
    public class ReactionLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public ReactionType ReactionType { get; set; }

        public virtual Comment Comment { get; set; }
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
}
