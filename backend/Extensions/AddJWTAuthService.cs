using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EduConnect.Extensions
{
    public static class AddJWTService
    {
        public static IServiceCollection AddJWTAuthService
     (this IServiceCollection services, IConfiguration config)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenKey = config["Jwt:SecretKey"] ?? throw new Exception("Token not found");
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        ;
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.Request.Path;
                        Console.WriteLine($"JWT middleware: Request path = {path}");
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            Console.WriteLine("Populating signalr context with access token");
                            Console.WriteLine(accessToken);
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }

                };
            });
            return services;
        }
    }
}
