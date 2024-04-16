using Microsoft.EntityFrameworkCore;
using WebDating.Data;
using WebDating.Helpers;
using WebDating.Interfaces;
using WebDating.Services;
using WebDating.SignalR;

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
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //automapper
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySetting"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();//Không muốn bị hủy sau khi yêu cầu HTTP đã hoàn thành
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
