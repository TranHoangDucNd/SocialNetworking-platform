using WebDating.Entities.ProfileEntities;

namespace WebDating.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }
        public Gender Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public string OrderBy { get; set; } = "lastActive";
        public Provice Province { get; set; }
    }
}
