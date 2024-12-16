
using backend.Data.DataSeeder;
using backend.Extensions;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using backend.Middleware.Tutor;
using backend.Repositories.Person;
using backend.Repositories.Reference;
using backend.Repositories.Tutor;
<<<<<<< HEAD
using EduConnect.Helpers;
=======
using backend.Services;
>>>>>>> e62e459ed1d7fa44c20fc57eae494a1e84df3398
using EduConnect.Interfaces;
using EduConnect.Services;

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
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITutorRepository, TutorRepository>();
<<<<<<< HEAD
            services.AddScoped<IPhotoService, PhotoService>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
=======
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonEducationInformationRepository, PersonEducationInformationRepository>();
            services.AddScoped<IPersonCareerInformationRepository, PersonCareerInformationRepository>();
            services.AddScoped<IReferenceRepository, ReferenceRepository>();
            services.AddScoped<IPersonAvailabilityRepository, PersonAvailabilityRepository>();
            services.AddScoped<IPersonPhoneNumberRepository, PersonPhoneNumberRepository>();
>>>>>>> e62e459ed1d7fa44c20fc57eae494a1e84df3398

            //Add Database Seeders as Scoped services
            services.AddScoped<CountryExtractor>();
            services.AddScoped<WorkTypeDatabaseSeeder>();
            services.AddScoped<EmploymentTypeDatabaseSeeder>();
            services.AddScoped<TutorRegistrationStatusDataSeeder>();

            services.AddScoped<CommunicationTypeDatabaseSeeder>();
            services.AddScoped<EngagementMethodDatabaseSeeder>();
            services.AddScoped<TutorTeachingStyleTypeDatabaseSeeder>();
            //Add Middleware 
            services.AddScoped<CheckTutorRegistrationAttribute>();
            services.AddScoped<CheckPersonLoginSignupAttribute>();
            //ADD HOSTED SERVICES
            services.AddHostedService<CountrySeederHostedService>();





            return services;
        }
    }
}
