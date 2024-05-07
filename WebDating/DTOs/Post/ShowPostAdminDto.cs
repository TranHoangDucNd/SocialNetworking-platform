using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class ShowPostAdminDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public IEnumerable<string> Images { get; set; }
        public UserShortDto UserShort { get; set; } = new UserShortDto();
    }
}
