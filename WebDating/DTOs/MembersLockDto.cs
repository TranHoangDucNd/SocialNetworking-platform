namespace WebDating.DTOs
{
    public class MembersLockDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public bool Lock { get; set; }

    }
}
