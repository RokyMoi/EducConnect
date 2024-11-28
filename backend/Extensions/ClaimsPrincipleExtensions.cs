using System.Security.Claims;

namespace EduConnect.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            if (email == null) throw new Exception("Cannot get email from token");
            return email;
        }
    }
}
