namespace WebDating.DTOs
{
    //chỉ định ng nhận message from other users
    public class CreateMessageDto
    {
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public string? Url { get; set; }
        public string? PublicId { get; set; }
    }
}
