using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person.PersonDetails
{
    public class PersonDetailsUpdateRequestDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public Guid? NationalCallingCodeCountryId { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? CountryOfOriginCountryId { get; set; }
    }
}