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
        public Provice WhereToDate { get; set; }
        public int DatingAgeFrom { get; set; }
        public int DatingAgeTo { get; set; }
        public int HeightFrom { get; set; }
        public int HeightTo { get; set; }
        public int WeightFrom {  get; set; }
        public int WeightTo { get; set; }
        public IEnumerable<UserInterest> UserInterests { get; set; }
        public IEnumerable<Occupations> Occupations { get; set; }
        public AppUser User { get; set; }
    }
}
