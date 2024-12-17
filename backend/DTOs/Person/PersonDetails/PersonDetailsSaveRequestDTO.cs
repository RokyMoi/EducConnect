using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace backend.DTOs.Person.PersonDetails
{
    public class PersonDetailsSaveRequestDTO
    {



        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9_.]+$",
        ErrorMessage = "Username must contain at least one letter, and can contain only numbers, underscores, and periods.")]
        public string Username { get; set; }
        public Guid? CountryOfOriginCountryId { get; set; }

    }
}