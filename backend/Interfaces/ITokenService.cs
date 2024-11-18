using EduConnect.Entities.Person;

namespace EduConnect.Interfaces
{
    public interface ITokenService
    {
      
            string CreateToken(PersonDetails person);  // Umesto AppUser, koristi Person
       
    }
}
