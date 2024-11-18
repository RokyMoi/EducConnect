using EduConnect.Entities.Person;
using EduConnect.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace EduConnect.Services
{
    public class TokenService : ITokenService
    {
      
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(PersonDetails person)
        {
            var _secretKey = _config["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(_secretKey) || _secretKey.Length < 64) {
                throw new Exception("Invalid token key");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,person.Username)
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
    }
}
