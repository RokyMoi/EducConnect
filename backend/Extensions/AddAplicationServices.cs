﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using backend.Data.DataSeeder;
using backend.Extensions;
// using backend.Interfaces.Course;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using backend.Middleware.Tutor;
// using backend.Repositories.Course;
using backend.Repositories.Person;
using backend.Repositories.Reference;
using backend.Repositories.Tutor;

using EduConnect.Helpers;

using backend.Services;
using EduConnect.Entities.Person;

using EduConnect.Interfaces;
using EduConnect.Services;
using Microsoft.AspNetCore.Identity;
using EduConnect.Data;
using EduConnect.Utilities;
using EduConnect.Interfaces.Course;
using EduConnect.Repositories.Course;
using EduConnect.Repositories.MessageRepository;
using EduConnect.SignalIR;
using EduConnect.Interfaces.Shopping;
using EduConnect.Interfaces.GenericTesting;
using EduConnect.Repositories;
using Stripe;
using Microsoft.Extensions.Options;


namespace EduConnect.Extensions
{
    public static class AddAppServices
    {
        public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHttpClient();

            //ADD SCOOPED SERVICES 
            services.AddScoped<ITokenService, Services.TokenService>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITutorRepository, TutorRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ICollaborationDocumentRepository, CollaborationDocumentRepository>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonEducationInformationRepository, PersonEducationInformationRepository>();
            services.AddScoped<IPersonCareerInformationRepository, PersonCareerInformationRepository>();
            services.AddScoped<IReferenceRepository, ReferenceRepository>();
            services.AddScoped<IPersonAvailabilityRepository, PersonAvailabilityRepository>();
            services.AddScoped<IPersonPhoneNumberRepository, PersonPhoneNumberRepository>();


           

            services.AddScoped<ICourseRepository, CourseRepository>(); services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddScoped<IAdminRepository, AdminRepository>();


            services.AddScoped<CountryExtractor>();
            services.AddScoped<ExtractIndustryClassification>();
            services.AddScoped<ExtractLearningCategoriesAndSubcategories>();
           services.Configure<StripeSettings>(
     configuration.GetSection("StripeSettings"));
          services.AddSingleton(resolver => resolver
                .GetRequiredService<IOptions<StripeSettings>>().Value);


            services.AddScoped<WorkTypeDatabaseSeeder>();
            services.AddScoped<EmploymentTypeDatabaseSeeder>();
            services.AddScoped<TutorRegistrationStatusDataSeeder>();

            services.AddScoped<CommunicationTypeDatabaseSeeder>();
            services.AddScoped<EngagementMethodDatabaseSeeder>();
            services.AddScoped<TutorTeachingStyleTypeDatabaseSeeder>();
            services.AddScoped<LearningDifficultyLevelDatabaseSeeder>();
            services.AddScoped<LanguageDatabaseSeeder>();
            // services.AddScoped<CourseTypeDatabaseSeeder>();

            services.AddScoped<FullTextSearchEnableService>();

            //Add Middleware 
            services.AddScoped<CheckTutorRegistrationAttribute>();
            services.AddScoped<CheckPersonLoginSignupAttribute>();

            //Add Azure Blob Storage
            services.AddScoped<AzureBlobStorageService>();

            //ADD HOSTED SERVICES
            services.AddHostedService<CountrySeederHostedService>();
            services.AddHostedService<IndustryClassificationHostedService>();


            services.AddAutoMapper(typeof(AutoMapperProfiles));
            /////////SHOPPING SERVICES
            services.AddScoped<IShoppingCartService,ShoppingCartService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IStudentEnrollmentService, StudentEnrollmentService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



            ///////////////////////////////
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddHostedService<LearningCategoryAndSubcategoryHostedService>();

            services.AddIdentity<Entities.Person.Person, IdentityRole<Guid>>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = false;

                }
            )
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole<Guid>>();

            services.AddScoped<UserManager<Entities.Person.Person>, PersonManager>();



            return services;
        }
    }

 
}
