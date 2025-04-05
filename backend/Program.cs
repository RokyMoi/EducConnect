using backend.Data.DataSeeder;
using backend.Entities.Reference.Country;
using backend.Extensions;
using EduConnect.Data;
using EduConnect.Entities.Person;
using EduConnect.Extensions;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Identity;
using EduConnect.SignalIR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EduConnect
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddHttpContextAccessor();

            Console.WriteLine(RuntimeInformation.OSDescription);

            Console.WriteLine(RuntimeInformation.OSArchitecture);
            //Configure Kestrel to allow request body up to 100MB in size
            builder.WebHost.ConfigureKestrel(
                options =>
                {
                    options.Limits.MaxRequestBodySize = 120 * 1024 * 1024;
                }
            );

            // Extension for services implemented
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJWTAuthService(builder.Configuration);
            builder.Services.AddDatabaseConnectionServices(builder.Configuration, builder.Environment);

            builder.Services.AddScoped<UserManager<Person>, PersonManager>();
            builder.Services.AddScoped<PersonManager>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                c =>
                {

                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                        {
                                            Type=ReferenceType.SecurityScheme,
                                            Id="Bearer"
                                        }
                                },
                                Array.Empty<string>()
                            }
                        }
                    );

                }
            );

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


                    //Get the LearningDifficultyLevel database seeder scoped service from the service container
                    var learningDifficultyLevelDatabaseSeeder = scope.ServiceProvider.GetRequiredService<LearningDifficultyLevelDatabaseSeeder>();
                    learningDifficultyLevelDatabaseSeeder.SeedLearningDifficultyLevelDataToDatabase().
                    GetAwaiter().GetResult();

                    //Get the Language database seeder scoped service from the service container
                    var languageDatabaseSeeder = scope.ServiceProvider.GetRequiredService<LanguageDatabaseSeeder>();
                    languageDatabaseSeeder.SeedLanguageDataToDatabase().GetAwaiter().GetResult();


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
                app.UseSwaggerUI(options =>
                {
                    options.EnablePersistAuthorization();
                });
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


            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Person>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                var roles = new[] { "Admin", "Tutor", "Student" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    }
                }

                var superAdminEmail = "superadmin@educonnect.com";
                var superAdminUsername = "superadmin";
                var superAdminPassword = "1234";

                var superAdmin = await dbContext.PersonEmail.Where(x => x.Email == superAdminEmail).AnyAsync();

                Console.WriteLine("SuperAdmin user exists: " + superAdmin);
                if (!superAdmin)
                {

                    //Create new Person
                    var Person = new EduConnect.Entities.Person.Person
                    {
                        PersonId = Guid.NewGuid(),
                        PersonPublicId = Guid.NewGuid(),
                        Email = superAdminEmail,
                        UserName = superAdminUsername,
                        IsActive = false,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        ModifiedAt = null
                    };



                    var result = await userManager.CreateAsync(Person, superAdminPassword);
                    await dbContext.SaveChangesAsync();

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(Person, "Admin");

                        Console.WriteLine("Added SuperAdmin user, set role to Admin");

                        //Create new PersonEmail
                        var PersonEmail = new PersonEmail
                        {
                            PersonId = Person.PersonId,
                            PersonEmailId = Guid.NewGuid(),
                            Email = superAdminEmail,
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null

                        };

                        //Create new PersonDetails
                        var PersonDetails = new PersonDetails
                        {
                            PersonDetailsId = Guid.NewGuid(),
                            PersonId = Person.PersonId,
                            FirstName = "SuperAdmin",
                            LastName = "SuperAdmin",
                            Username = superAdminUsername,
                            CountryOfOriginCountryId = null,
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null,
                        };






                        //Create new PersonPassword 
                        var PersonPassword = new PersonPassword
                        {
                            PersonId = Person.PersonId,
                            PersonPasswordId = Guid.NewGuid(),
                            PasswordHash = EncryptionUtilities.HashPassword(superAdminPassword),
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null
                        };

                        //Create new PersonSalt 
                        var PersonSalt = new PersonSalt
                        {
                            PersonSaltId = Guid.NewGuid(),
                            PersonId = Person.PersonId,
                            NumberOfRounds = 12,
                            Salt = EncryptionUtilities.GenerateSalt(),
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null
                        };

                        await dbContext.PersonEmail.AddAsync(PersonEmail);
                        await dbContext.PersonPassword.AddAsync(PersonPassword);
                        await dbContext.PersonSalt.AddAsync(PersonSalt);
                        await dbContext.PersonDetails.AddAsync(PersonDetails);
                        await dbContext.SaveChangesAsync();
                    }

                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Failed to create SuperAdmin user");

                        Console.WriteLine("Error list: ");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine("Code: " + error.Code);
                            Console.WriteLine("Description:" + error.Description);
                        }
                    }


                }
            }
            app.Run();
        }
    }
}
