using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities
{
    [Table("Report")]
    public class Report
    {
        public Report()
        {
            //khởi tạo một tập hợp rỗng để chứa các PostReportDetail tương ứng với mỗi đối tượng Report.
            PostReportDetails = new HashSet<PostReportDetail>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "dateTime")]
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<PostReportDetail> PostReportDetails { get; set; }
    }
}
