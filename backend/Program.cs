using backend.Entities.Reference.Country;
using backend.Extensions;
using backend.Utilities;
using EduConnect.Data;
using EduConnect.Extensions;
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
                          .AllowAnyMethod();                      // Allow any methods (GET, POST, etc.)
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
                        policy.WithOrigins(swaggerLink).AllowAnyHeader().AllowAnyMethod();
                    });
                });

            }
            var app = builder.Build();



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

            app.Run();
        }
    }
}
