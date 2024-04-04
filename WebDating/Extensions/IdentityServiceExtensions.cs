using Microsoft.AspNetCore.Identity;
using WebDating.Data;
using WebDating.Entities;

namespace WebDating.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                  .AddRoles<AppRole>()
                  .AddRoleManager<RoleManager<AppRole>>()
                  .AddEntityFrameworkStores<DataContext>();


            return services;
        }
    }
}
