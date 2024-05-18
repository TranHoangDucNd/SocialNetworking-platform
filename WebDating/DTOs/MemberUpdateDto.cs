using WebDating.Entities.ProfileEntities;

namespace WebDating.DTOs
{
    public class MemberUpdateDto
    {
        public string Username { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public Gender DatingObject { get; set; }
        public Height Height { get; set; }
        public Provice WhereToDate { get; set; }
        public DatingProfileDto DatingProfile { get; set; }
    }
}
