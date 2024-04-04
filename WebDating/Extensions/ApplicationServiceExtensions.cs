using Microsoft.EntityFrameworkCore;
using WebDating.Data;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Extensions
{
    static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySetting"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
