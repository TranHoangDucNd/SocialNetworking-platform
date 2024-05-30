namespace WebDating.DTOs
{
    public class MessageSummaryDto
    {
        public string Username { get; set; }
        public string LastMessageContent { get; set; }
        public string KnownAs { get; set; }
        public string Url { get; set; }
        public DateTime LastMessageSent { get; set; }
        public bool DateRead { get; set; }
    }
}
