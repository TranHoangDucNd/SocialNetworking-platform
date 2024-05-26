using WebDating.Entities.PostEntities;

namespace WebDating.DTOs.Post
{
    public class PostReportAdminDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string KnownAs { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
        public bool Checked { get; set; } = false;
        public string Report { get; set; }
    }
}
