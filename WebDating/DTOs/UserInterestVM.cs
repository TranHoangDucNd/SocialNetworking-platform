using WebDating.Entities;

namespace WebDating.DTOs
{
    public class UserInterestVM
    {
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public Interest InterestName { get; set; }
    }
}
