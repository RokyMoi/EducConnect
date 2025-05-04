using System.Security.Claims;
using EduConnect.Entities.Person;

namespace EduConnect.Interfaces
{
    public interface ITokenService
    {

        Task<AuthenticationToken?> CreateTokenAsync(EduConnect.Entities.Person.Person person);
        Task<bool> ValidateToken(string authenticationToken);

        Task<string?> GetRole(PersonEmail person);

        Task<AuthenticationToken?> GetTokenByPersonPublicId(Guid publicId);

        ClaimsPrincipal? ValidateTokenWithClaims(string token);

        Task<bool> CheckIfExistsByPersonId(Guid personId);

        Task<bool> RevokeTokenByPersonId(Guid personId);
    }
}
