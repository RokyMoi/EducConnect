using EduConnect.Data;
using EduConnect.Extensions;
using Microsoft.EntityFrameworkCore;
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
                    policy.WithOrigins(frontendApplicationOrigin).AllowAnyHeader().AllowAnyMethod();
                });
            });
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

            app.MapControllers();

            app.Run();
        }
    }
}
