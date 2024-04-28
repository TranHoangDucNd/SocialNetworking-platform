using System.ComponentModel.DataAnnotations;

namespace WebDating.Entities
{
    public enum Gender
    {
        [Display(Name = "Mọi người")]
        EveryOne,
        [Display(Name = "Nam")]
        Male,
        [Display(Name = "Nữ")]
        Female
    }
}
