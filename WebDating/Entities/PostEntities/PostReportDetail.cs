using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDating.Entities.UserEntities;

namespace WebDating.Entities.PostEntities
{
    [Table("PostReportDetail")]
    public class PostReportDetail
    {
        [Key]
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Description { get; set; }
        public DateTime ReportDate { get; set; }
        public bool Checked { get; set; } = false;
        public Post Post { get; set; }
        public AppUser User { get; set; }
        public Report Report { get; set; }
    }
}
