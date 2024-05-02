using Microsoft.AspNetCore.Identity;

namespace WebDating.Entities.UserEntities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
