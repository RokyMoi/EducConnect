using EduConnect.Data;
using EduConnect.Entities.Person;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace EduConnect.Services
{
    public class TokenService(IConfiguration config, DataContext db, UserManager<Person> userManager, IHttpContextAccessor httpContextAccessor) : ITokenService
    {

        private readonly IConfiguration _config = config;
        private readonly DataContext _db = db;
        private readonly UserManager<Person> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<AuthenticationToken?> CreateTokenAsync(Person person)
        {
            var personId = person.PersonPublicId;
            var email = db.PersonEmail.Where(x => x.PersonId == person.PersonId).FirstOrDefault();
            var role = await GetRole(email);

            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, person.PersonPublicId.ToString()),
                new (ClaimTypes.Email, email.Email),
                new (ClaimTypes.Role, role)

            };



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30), //Persistent token
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            var jwtString = tokenHandler.WriteToken(token);

            var authToken = new AuthenticationToken
            {
                AuthenticationTokenId = Guid.NewGuid(),
                PersonId = person.PersonId,
                Token = jwtString,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UpdatedAt = null
            };

            try
            {
                await _db.AuthenticationToken.AddAsync(authToken);
                await _db.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return null;

            }

            return authToken;


        }

        public async Task<bool> RevokeToken(string token)
        {
            var authToken = await _db.AuthenticationToken.Where(x => x.Token == token).FirstOrDefaultAsync();

            if (authToken == null)
            {
                return false;
            }

            try
            {
                _db.AuthenticationToken.Remove(authToken);
                await _db.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to revoke token");
                Console.WriteLine(ex.ToString());
            }

            return true;
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


        public async Task<bool> ValidateToken(string authenticationToken)
        {
            var secretKey = _config["Jwt:SecretKey"];



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero

            };

            try
            {
                var principal = tokenHandler.ValidateToken(authenticationToken, validationParameters, out var validatedToken);

                Console.WriteLine("Token validated");
                Console.WriteLine($"Validated Token ID: {validatedToken.Id}");
                foreach (var claim in principal.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }

                _httpContextAccessor.HttpContext.User = principal;

                var storedToken = await _db.AuthenticationToken.FirstOrDefaultAsync(x => x.Token == authenticationToken);

                Console.WriteLine("StoredToken::" + storedToken);
                if (storedToken == null)
                {
                    Console.WriteLine("DatabaseTokenError::Token not found in database");
                    return false;
                }

                if (storedToken != null)
                {
                    Console.WriteLine("Token validation successful and found in database");
                    return true;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                Console.WriteLine("SecurityTokenExpiredException::Token expired::" + ex.ToString());
            }
            catch (SecurityTokenNotYetValidException ex)
            {
                Console.WriteLine("SecurityTokenNotYetValidException::Token not yet valid::" + ex.ToString());
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                Console.WriteLine("SecurityTokenInvalidIssuerException::Invalid token issuer::" + ex.ToString());
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                Console.WriteLine("SecurityTokenInvalidAudienceException::Invalid token audience::" + ex.ToString());
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                Console.WriteLine("SecurityTokenInvalidSignatureException::Invalid token signature::" + ex.ToString());
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine("SecurityTokenException::Token validation failed::" + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception::Token validation failed::" + ex.ToString());
            }
            return false;


        }

        public async Task<AuthenticationToken?> GetTokenByPersonPublicId(Guid publicId)
        {
            var person = await _db.Person.Where(x => x.PersonPublicId == publicId).FirstOrDefaultAsync();

            if (person == null)
            {
                return null;
            }

            var token = await _db.AuthenticationToken.Where(x => x.PersonId == person.PersonId).FirstOrDefaultAsync();

            if (token == null)
            {
                return null;
            }

            token.Person = person;
            return token;
        }

        public ClaimsPrincipal? ValidateTokenWithClaims(string token)
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

        public async Task<bool> CheckIfExistsByPersonId(Guid personId)
        {
            return await _db.AuthenticationToken.Where(x => x.PersonId == personId).AnyAsync();
        }

        public async Task<bool> RevokeTokenByPersonId(Guid personId)
        {
            var token = await _db.AuthenticationToken.Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            if (token == null)
            {
                return false;
            }

            try
            {
                _db.AuthenticationToken.Remove(token);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An error occurred while revoking the token");
                Console.WriteLine(ex.ToString());
                return false;

            }
        }
    }




}

