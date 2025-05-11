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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EduConnect.Interfaces.Redis;
using EduConnect.Services;

namespace EduConnect
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure structured logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddHttpContextAccessor();

            // Security headers
            builder.Services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = false;
            });

            // Configure Kestrel with reasonable limits
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100MB
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
            });

            // Extension for services implemented
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJWTAuthService(builder.Configuration);
            builder.Services.AddDatabaseConnectionServices(builder.Configuration, builder.Environment);

            // Configure SignalR with increased message size
            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 102400; // 100KB
                options.EnableDetailedErrors = builder.Environment.IsDevelopment();
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddStackExchangeRedisCache(
               options =>
               {
                   options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
                   options.InstanceName = "EduConnect";
               }
           );

            // builder.Services.AddScoped<IRedisCachingService, RedisCachingService>();

            builder.Services.AddScoped<UserManager<Person>, PersonManager>();
            builder.Services.AddScoped<PersonManager>();

            builder.Services.AddHostedService<CourseViewershipChangeService>();
            builder.Services.AddHostedService<CourseViewershipDataSnapshotBackgroundService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Prevent JSON circular reference issues
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EduConnect API",
                    Version = "v1",
                    Description = "API for EduConnect platform"
                });

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
            });

            // Get link to frontend application from appsettings.json
            var frontendApplicationOrigin = builder.Configuration.GetSection("AllowedOrigins:FrontendApplication").Value;
            if (string.IsNullOrEmpty(frontendApplicationOrigin))
            {
                throw new InvalidOperationException("Frontend application origin is not configured in appsettings.json");
            }

            // Consolidated CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(frontendApplicationOrigin)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials() // Critical for SignalR
                          .WithExposedHeaders("Authorization");
                });

                // Add separate policy for Swagger if needed
                if (builder.Environment.IsDevelopment())
                {
                    var swaggerLink = builder.Configuration.GetSection("AllowedOrigins:Swagger").Value;
                    if (!string.IsNullOrEmpty(swaggerLink))
                    {
                        options.AddPolicy("SwaggerPolicy", policy =>
                        {
                            policy.WithOrigins(swaggerLink)
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials();
                        });
                    }
                }
            });

            var app = builder.Build();

            // Seed database with necessary data
            await SeedDatabaseAsync(app);

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.EnablePersistAuthorization();
                    options.DefaultModelsExpandDepth(-1); // Hide schemas section
                });
            }
            else
            {
                // Add security headers in production
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Apply security headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
                context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
                await next();
            });

            // IMPORTANT: UseCors must come BEFORE UseRouting and authentication
            app.UseCors(); // Apply default policy to all endpoints

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            // Map endpoints at the end
            app.MapControllers();

            // Map SignalR hubs
            app.MapHub<CourseAnalyticsHub>("/course-analytics-hub");
            app.MapHub<CollaborationDocumentHub>("hubs/collaboration-document").RequireAuthorization();
            app.MapHub<PresenceHub>("/hubs/presence");
            app.MapHub<MessageHub>("/hubs/message");

            // Create superadmin user if not exists
            await CreateSuperAdminUserAsync(app);

            app.Run();
        }

        private static async Task SeedDatabaseAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var workTypeSeeder = scope.ServiceProvider.GetRequiredService<WorkTypeDatabaseSeeder>();
                await workTypeSeeder.SeedWorkTypeDataToDatabase();

                var employmentTypeSeeder = scope.ServiceProvider.GetRequiredService<EmploymentTypeDatabaseSeeder>();
                await employmentTypeSeeder.SeedEmploymentTypeDataToDatabase();

                var tutorRegistrationStatusSeeder = scope.ServiceProvider.GetRequiredService<TutorRegistrationStatusDataSeeder>();
                await tutorRegistrationStatusSeeder.SeedTutorRegistrationStatusDataToDatabase();

                var communicationTypeSeeder = scope.ServiceProvider.GetRequiredService<CommunicationTypeDatabaseSeeder>();
                await communicationTypeSeeder.SeedCommunicationTypeDataToDatabase();

                var engagementMethodSeeder = scope.ServiceProvider.GetRequiredService<EngagementMethodDatabaseSeeder>();
                await engagementMethodSeeder.SeedEngagementMethodDataToDatabase();

                var tutorTeachingStyleTypeDatabaseSeeder = scope.ServiceProvider.GetRequiredService<TutorTeachingStyleTypeDatabaseSeeder>();
                await tutorTeachingStyleTypeDatabaseSeeder.SeedTutorTeachingStyleTypeDataToDatabase();

                var learningDifficultyLevelDatabaseSeeder = scope.ServiceProvider.GetRequiredService<LearningDifficultyLevelDatabaseSeeder>();
                await learningDifficultyLevelDatabaseSeeder.SeedLearningDifficultyLevelDataToDatabase();

                var languageDatabaseSeeder = scope.ServiceProvider.GetRequiredService<LanguageDatabaseSeeder>();
                await languageDatabaseSeeder.SeedLanguageDataToDatabase();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during database seeding");
            }
        }

        private static async Task CreateSuperAdminUserAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Person>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                // Create roles if they don't exist
                var roles = new[] { "Admin", "Tutor", "Student" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                        logger.LogInformation("Created role: {Role}", role);
                    }
                }

                // Check if superadmin exists
                var superAdminEmail = "superadmin@educonnect.com";
                var superAdminUsername = "superadmin";

                // In production, this should be loaded from configuration or environment variables
                var superAdminPassword = app.Environment.IsDevelopment()
                    ? "1234"
                    : Guid.NewGuid().ToString(); // Generate a secure password in production

                var superAdmin = await dbContext.PersonEmail
                    .AnyAsync(x => x.Email == superAdminEmail);

                logger.LogInformation("SuperAdmin user exists: {Exists}", superAdmin);

                if (!superAdmin)
                {
                    // Create new Person
                    var person = new EduConnect.Entities.Person.Person
                    {
                        PersonId = Guid.NewGuid(),
                        PersonPublicId = Guid.NewGuid(),
                        Email = superAdminEmail,
                        UserName = superAdminUsername,
                        IsActive = true,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        ModifiedAt = null
                    };

                    var result = await userManager.CreateAsync(person, superAdminPassword);
                    await dbContext.SaveChangesAsync();

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(person, "Admin");
                        logger.LogInformation("Added SuperAdmin user with ID {PersonId}", person.PersonId);

                        // Create related records
                        var personEmail = new PersonEmail
                        {
                            PersonId = person.PersonId,
                            PersonEmailId = Guid.NewGuid(),
                            Email = superAdminEmail,
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null
                        };

                        var personDetails = new PersonDetails
                        {
                            PersonDetailsId = Guid.NewGuid(),
                            PersonId = person.PersonId,
                            FirstName = "SuperAdmin",
                            LastName = "SuperAdmin",
                            Username = superAdminUsername,
                            CountryOfOriginCountryId = null,
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null,
                        };

                        var salt = EncryptionUtilities.GenerateSalt();
                        var personPassword = new PersonPassword
                        {
                            PersonId = person.PersonId,
                            PersonPasswordId = Guid.NewGuid(),
                            PasswordHash = EncryptionUtilities.HashPassword(superAdminPassword),
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null
                        };

                        var personSalt = new PersonSalt
                        {
                            PersonSaltId = Guid.NewGuid(),
                            PersonId = person.PersonId,
                            NumberOfRounds = 12,
                            Salt = salt,
                            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            ModifiedAt = null
                        };

                        await dbContext.PersonEmail.AddAsync(personEmail);
                        await dbContext.PersonPassword.AddAsync(personPassword);
                        await dbContext.PersonSalt.AddAsync(personSalt);
                        await dbContext.PersonDetails.AddAsync(personDetails);
                        await dbContext.SaveChangesAsync();

                        if (app.Environment.IsProduction())
                        {
                            logger.LogWarning("SuperAdmin created with auto-generated password. Please check logs or reset password.");
                        }
                    }
                    else
                    {
                        logger.LogError("Failed to create SuperAdmin user");
                        foreach (var error in result.Errors)
                        {
                            logger.LogError("Error: {Code} - {Description}", error.Code, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the SuperAdmin user");
            }
        }
    }
}