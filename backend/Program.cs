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

            var app = builder.Build();

            // Add Database Connection
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
