using EduConnect.Interfaces.GetUser;
using EduConnect.Utilities;
using System.Security.Claims;

namespace EduConnect.Helpers
{
    public class Caller : ICaller
    {
        public string Email { get; }

        public Caller(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                Console.WriteLine("❌Caller Class| HttpContext is null");
                throw new UnauthorizedAccessException("No HttpContext.");
            }

            var user = httpContext.User;
            PrintObjectUtility.PrintObjectProperties(user);

            if (user == null || !user.Identity.IsAuthenticated)
            {
                Console.WriteLine("❌ Caller Class|User is not authenticated.");
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            Console.WriteLine("✅ Caller Class|Claims found:");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"   🔹 {claim.Type}: {claim.Value}");
            }

            Email = user.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(Email))
            {
                Console.WriteLine("❌ Caller Class|Email claim not found.");
                throw new UnauthorizedAccessException("Email not found in claims.");
            }

            Console.WriteLine($"📧 Email found: {Email}");
        }
    }
}