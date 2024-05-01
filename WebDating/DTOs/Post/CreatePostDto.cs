namespace WebDating.DTOs.Post
{
    public class CreatePostDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<IFormFile>? Image { get; set; }
    }
}
