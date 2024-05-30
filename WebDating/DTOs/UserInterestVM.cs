using System.ComponentModel.DataAnnotations;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.DTOs
{
    public class UserInterestVM
    {
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public Interest InterestName { get; set; }
    }

    public class UserInterestDto
    {
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public string InterestName { get; set; }
        public Interest InterestNameCode { get; set; }
        public InterestType InterestType { get; set; }
    }
    public class OccupationDto
    {
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public string OccupationName { get; set; }
        public Occupation OccupationNameCode { get; set; }
        public OccupationType OccupationType { get; set; }
    }
}
