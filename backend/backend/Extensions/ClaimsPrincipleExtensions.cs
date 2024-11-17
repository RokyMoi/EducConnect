using System.Security.Claims;

namespace EduConnect.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        //Da bismo omogućili da korisnik može promijeniti sliku samo ako je autentificiran
        //(tj. posjeduje odgovarajući token s valjanom autorizacijom)
        //Za update.
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null) throw new Exception("Cannot get username from token");
            return username;
        }
    }
}
