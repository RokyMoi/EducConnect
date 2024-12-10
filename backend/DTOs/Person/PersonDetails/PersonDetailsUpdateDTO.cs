using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person.PersonDetails
{
    public class PersonDetailsUpdateDTO
    {
        public Guid PersonDetailsId { get; set; }
        public Guid PersonId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }

        public string? PhoneNumberCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryOfOrigin { get; set; }
    }
}