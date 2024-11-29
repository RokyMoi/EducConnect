using EduConnect.Entities.Person;

namespace EduConnect.Interfaces
{
    public interface ITokenService
    {

        Task<string> CreateTokenAsync(PersonEmail person);  

    }
}
