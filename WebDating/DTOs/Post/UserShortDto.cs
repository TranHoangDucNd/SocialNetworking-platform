namespace WebDating.DTOs.Post
{
    public class UserShortDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
