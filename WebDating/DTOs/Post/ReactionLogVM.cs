using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class ReactionLogVM
    {
        public ReactionType Type { get; set; }
        public string DisplayName { get; set; }
        public string UserFullName { get; set; }
        public int UserId { get; set; }

    }
}
