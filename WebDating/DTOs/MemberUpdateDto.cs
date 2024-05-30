using WebDating.Entities.ProfileEntities;

namespace WebDating.DTOs
{
    public class MemberUpdateDto
    {
        public string Username { get; set; }
        public string Introduction { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public Gender DatingObject { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        //dating profile
        public int DatingAgeFrom { get; set; }
        public int DatingAgeTo { get; set; }
        public int HeightFrom { get; set; }
        public int HeightTo { get; set; }
        public int WeightFrom { get; set; }
        public int WeightTo { get; set; }

        public Provice WhereToDate { get; set; }
        public DatingProfileDto DatingProfile { get; set; }
    }
}
