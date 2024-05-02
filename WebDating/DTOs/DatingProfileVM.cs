using WebDating.Entities.ProfileEntities;

namespace WebDating.DTOs
{
    public class DatingProfileVM
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public Gender DatingObject { get; set; }
        public Height Height { get; set; }
        public Provice WhereToDate { get; set; }
        public IEnumerable<UserInterestVM> UserInterests { get; set; }
    }
}
