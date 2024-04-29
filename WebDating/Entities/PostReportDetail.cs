using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities
{
    [Table("PostReportDetail")]
    public class PostReportDetail
    {
        [Key]
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int UserId { get; set; }
        [StringLength(250)]
        public int PostId { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
        public bool Checked { get; set; } = false;
        public virtual Post Post { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Report Report { get; set; }
    }
}
