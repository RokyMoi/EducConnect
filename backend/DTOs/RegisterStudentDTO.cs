using System.ComponentModel.DataAnnotations;

namespace EduConnect.DTOs
{
    public class RegisterStudentDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
        public required string Username { get; set; }
        public required Guid PhoneNumberCountryCodeCountryId { get; set; }

        public required string PhoneNumber { get; set; }

        public required Guid CountryOfOriginCountryId { get; set; }
    }
}
