using System.ComponentModel.DataAnnotations;

namespace WebDating.Entities
{
    public class UserInterest
    {
        [Key]
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public DatingProfile DatingProfile { get; set; }
        public Interest InterestName { get; set; }
    }
}
