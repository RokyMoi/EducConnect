using System.ComponentModel.DataAnnotations;

namespace EduConnect.DTOs
{
    public class RegisterStudentDTO
    {
        [Required]
        [MinLength(1)]
        public required string FirstName { get; set; }
        [Required]
        [MinLength(1)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]

        public required string Password { get; set; }

        [Required]
        [MinLength(8)]
        public required string Username { get; set; }

        [Required]
        [RegularExpression(@"^\+\d+$", ErrorMessage = "The field must start with '+' and contain at least one digit.")]
        public required string PhoneNumberCountryCode { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The field must contain numbers only.")]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string CountryOfOrigin { get; set; }
    }
}
