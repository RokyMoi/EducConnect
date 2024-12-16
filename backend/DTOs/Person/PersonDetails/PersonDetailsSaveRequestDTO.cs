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
        public string Username { get; set; }
        public string? PhoneNumberCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? CountryOfOriginCountryId { get; set; }

    }
}