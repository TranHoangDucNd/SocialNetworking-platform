using System.ComponentModel.DataAnnotations;

namespace WebDating.Entities.ProfileEntities
{
    public enum Gender
    {
        [Display(Name = "Match gender")]
        MatchGender,
        [Display(Name = "Everyone")]
        EveryOne,
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female
    }
}
