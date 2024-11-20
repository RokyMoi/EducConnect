
using backend.Services;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Extensions
{
    public static class DatabaseConnectionService
    {


        public static IServiceCollection AddDatabaseConnectionServices

       (this IServiceCollection services, IConfiguration config, IHostEnvironment env)
        {

            string connectionString = DatabaseServerConnectionService.GetConnectionString(config, env);
            services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(
            connectionString);
        });
            TestDatabaseConnection(config, env);
            return services;

        }
        private static void TestDatabaseConnection(IConfiguration config, IHostEnvironment env)
        {

            string connectionString = DatabaseServerConnectionService.GetConnectionString(config, env);
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);
            Console.WriteLine(connectionString);

            using (var context = new DataContext(optionsBuilder.Options))
            {
                try
                {
                    if (context.Database.CanConnect())
                    {
                        Console.WriteLine("::::::::::INFO:::::::Database connection successful.");
                    }
                    else
                    {

                        throw new Exception("Database connection failed. Check your connection string.");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error connecting to the database: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    throw; // Re-throw the exception to ensure visibility during startup.
                }
            }
        }
    }


}
