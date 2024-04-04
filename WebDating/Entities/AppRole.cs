using Microsoft.AspNetCore.Identity;

namespace WebDating.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
