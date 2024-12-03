using backend.Extensions;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Repositories.Person;
using backend.Repositories.Reference;
using backend.Repositories.Tutor;
using backend.Services;
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
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonEducationInformationRepository, PersonEducationInformationRepository>();

            services.AddScoped<CountryExtractor>();

            //ADD HOSTED SERVICES
            services.AddHostedService<CountrySeederHostedService>();





            return services;
        }
    }
}
