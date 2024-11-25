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
        }
        public async Task<string> CreateTokenAsync(PersonDetails person)
        {
            var _secretKey = _config["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(_secretKey) || _secretKey.Length < 64) {
                throw new Exception("Invalid token key");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var role = await GetRole(person);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,person.Username),
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
        public async Task<string> GetRole(PersonDetails person)
        {
            var tutor = await _db.Tutor.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
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
    }
}

