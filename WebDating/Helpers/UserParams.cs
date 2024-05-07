using WebDating.Entities.ProfileEntities;

namespace WebDating.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }
        public Gender Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;

        public Height MinHeight { get; set; }
        public Height MaxHeight { get; set; }
        public string OrderBy { get; set; } = "lastActive";
        public Provice City { get; set; }
    }
}
