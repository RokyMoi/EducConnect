
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Extensions
{
    public static class DatabaseConnectionService
    {
        
        public static IServiceCollection AddDatabaseConnectionServices
       (this IServiceCollection services, IConfiguration config)
        {
           

            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            });


            return services;

        }
    }
}
