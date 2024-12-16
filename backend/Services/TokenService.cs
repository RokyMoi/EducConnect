using EduConnect.Data;
using EduConnect.Entities.Person;
using EduConnect.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace EduConnect.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;
        private readonly DataContext _db;
        public TokenService(IConfiguration config, DataContext db)
        {
            _config = config;
            _db = db;
        }
        public async Task<string> CreateTokenAsync(PersonEmail person)
        {
            var _secretKey = _config["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(_secretKey) || _secretKey.Length < 64)
            {
                throw new Exception("Invalid token key");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var role = await GetRole(person);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,person.Email),
                new Claim(ClaimTypes.Role, role)

            };
            var SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = SigningCredentials
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(TokenDescriptor);
            return TokenHandler.WriteToken(token);


        }
        public async Task<string> GetRole(PersonEmail person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "PersonDetails cannot be null.");
            }

            if (person.PersonId == Guid.Empty)
            {
                throw new ArgumentException("Invalid PersonId.", nameof(person));
            }

            Console.WriteLine("PersonId from TokenService: " + person.PersonId);
            var tutor = await _db.Tutor.Where(x => x.PersonId == person.PersonId).FirstOrDefaultAsync();

            Console.WriteLine(tutor != null ? $"Tutor found: {tutor.PersonId}" : "No tutor found");
            if (tutor != null)
            {
                return "tutor";
            }

            var student = await _db.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student != null)
            {
                return "student";
            }

            return "admin";
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var secretKey = _config["Jwt:SecretKey"];

            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 64)
            {
                throw new Exception("Invalid token key");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameter = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero,

            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameter, out _);
                Console.WriteLine("Principal: " + principal.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().ToString());
                Console.WriteLine("Token validation  successful.");
                return principal;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Invalid token", ex);
                return null;
            }


        }
    }


}

