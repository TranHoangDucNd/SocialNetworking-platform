using System.ComponentModel.DataAnnotations;

namespace WebDating.Entities
{
    public class DatingProfile
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public Gender DatingObject { get; set; }
        public Height Height { get; set; }
        public Provice WhereToDate { get; set; }
        public IEnumerable<UserInterest> UserInterests { get; set; }

        public AppUser User { get; set; }
    }
}
