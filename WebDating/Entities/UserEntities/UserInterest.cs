using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using WebDating.Entities.ProfileEntities;

namespace WebDating.Entities.UserEntities
{
    public class UserInterest
    {
        [Key]
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public DatingProfile DatingProfile { get; set; }
        public Interest InterestName { get; set; }
        public InterestType InterestType { get; set; }
    }

    public enum InterestType
    {
        OwnInterest = 1,
        DesiredInterest = 2
    }
}
