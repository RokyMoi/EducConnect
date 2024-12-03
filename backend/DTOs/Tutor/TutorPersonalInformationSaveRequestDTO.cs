using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorPersonalInformationSaveRequestDTO
    {
        [Required]
        [EmailAddress]
        public string TutorEmail { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9._]{1,12}$", ErrorMessage = "Username must be between 1 and 12 characters and must contain at least one letter, and can contain numbers, underscores, and dots.")]
        public string Username { get; set; }
        [RegularExpression(@"^[1-9][0-9]{0,3}$", ErrorMessage = "National calling code must be between 1 and 9999.")]
        public string? PhoneNumberCountryCode { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must only contain numbers without spaces or special characters.")]
        public string? PhoneNumber { get; set; }
        public string? CountryOfOrigin { get; set; }


    }
}