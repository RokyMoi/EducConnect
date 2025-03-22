using EduConnect.Interfaces.GetUser;
using System.Security.Claims;

namespace EduConnect.Helpers
{
    public class Caller : ICaller
    {
        public string Email { get; }

        public Caller(HttpContext httpContext)
        {
            
            Email = httpContext.User?.FindFirst(ClaimTypes.Email)?.Value
                    ?? throw new UnauthorizedAccessException("Email not found in claims.");
        }
    }
}
