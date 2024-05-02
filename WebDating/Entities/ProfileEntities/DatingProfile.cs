using System.ComponentModel.DataAnnotations;
using WebDating.Entities.UserEntities;

namespace WebDating.Entities.ProfileEntities
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
