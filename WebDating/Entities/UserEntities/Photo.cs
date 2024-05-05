using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities.UserEntities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public int AppUserId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public AppUser AppUser { get; set; }
    }
}
