using backend.Data.DataSeeder;
using backend.Entities.Reference.Country;
using backend.Extensions;
using backend.Utilities;
using EduConnect.Data;
using EduConnect.Extensions;
using EduConnect.SignalIR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace EduConnect
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Console.WriteLine(RuntimeInformation.OSDescription);



            // Extension for services implemented
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJWTAuthService(builder.Configuration);
            builder.Services.AddDatabaseConnectionServices(builder.Configuration, builder.Environment);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Setup CORS Policy, for request originating from frontend application origin only (http://localhost:4200)

            //Get link to frontend application from app.settings.json (AllowedOrigins:FrontendApplication)
            var frontendApplicationOrigin = builder.Configuration.GetSection("AllowedOrigins:FrontendApplication").Value;

            //Add CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontEndPolicy", policy =>
                {
                    policy.WithOrigins(frontendApplicationOrigin)  // Allow frontend origin
                          .AllowAnyHeader()                       // Allow any headers
                          .AllowAnyMethod()/// Allow any methods (GET, POST, etc.)
                          .AllowCredentials()
                          .WithExposedHeaders("Authorization"); // Expose the Authorization header
                });
            });

            //Set CORS Policy for requests from Swagger, only in development environment
            if (builder.Environment.IsDevelopment())
            {
                //Get link to swagger localhost instance for http requests testing from app.settings.json (AllowedOrigins:Swagger)
                var swaggerLink = builder.Configuration.GetSection("AllowedOrigins:Swagger").Value;

                //Add CORS Policy
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("SwaggerDevelopmentEnvironmentPolicy", policy =>
                    {
                        policy.WithOrigins(swaggerLink).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    });
                });

            }
            var app = builder.Build();

            //Seed database with work type data
            using (var scope = app.Services.CreateScope())
            {
                //The following code block is used to seed the database with work and employment type data
                //This code block uses async operations, but to not make the Main class async, GetAwaiter().GetResult() to execute the async operation, which is blocking in nature, and will block the Main method from executing the next line of code until the async operation is completed

                try
                {
                    //Get the WorkType database seeder scoped service from the service container
                    var workTypeSeeder = scope.ServiceProvider.GetRequiredService<WorkTypeDatabaseSeeder>();
                    workTypeSeeder.SeedWorkTypeDataToDatabase().GetAwaiter().GetResult();

                    //Get the EmploymentType database scoped seeder service from the service container
                    var employmentTypeSeeder = scope.ServiceProvider.GetRequiredService<EmploymentTypeDatabaseSeeder>();
                    employmentTypeSeeder.SeedEmploymentTypeDataToDatabase().GetAwaiter().GetResult();

                    //Get the TutorRegistrationStatus database seeder scoped service from the service container
                    var tutorRegistrationStatusSeeder = scope.ServiceProvider.GetRequiredService<TutorRegistrationStatusDataSeeder>();
                    tutorRegistrationStatusSeeder.SeedTutorRegistrationStatusDataToDatabase().GetAwaiter().GetResult();

                    //Get the CommunicationType database seeder scoped service from the service container
                    var communicationTypeSeeder = scope.ServiceProvider.GetRequiredService<CommunicationTypeDatabaseSeeder>();
                    communicationTypeSeeder.SeedCommunicationTypeDataToDatabase().GetAwaiter().GetResult();

                    //Get the EngagementMethod database seeder scoped service from the service container
                    var engagementMethodSeeder = scope.ServiceProvider.GetRequiredService<EngagementMethodDatabaseSeeder>();
                    engagementMethodSeeder.SeedEngagementMethodDataToDatabase().GetAwaiter().GetResult();

                    //Get the TutorType database seeder scoped service from the service container
                    var tutorTeachingStyleTypeDatabaseSeeder = scope.ServiceProvider.GetRequiredService<TutorTeachingStyleTypeDatabaseSeeder>();
                    tutorTeachingStyleTypeDatabaseSeeder.SeedTutorTeachingStyleTypeDataToDatabase().GetAwaiter().GetResult();


                }
                catch (Exception ex)
                {
                    // Log and handle seeding errors
                    Console.Error.WriteLine($"An error occurred during seeding: {ex.Message}");
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //Set backend application to use CORS policy for requests originating only from specific origin (frontend application)
            app.UseCors("FrontEndPolicy");

            //Set backend application to use CORS policy for requests originating only from specific origin (swagger) in development environment
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("SwaggerDevelopmentEnvironmentPolicy");
            }

            app.MapControllers();
            app.MapHub<PresenceHub>("hubs/presence");
            app.MapHub<MessageHub>("hubs/message");

            app.Run();
        }
    }
}
