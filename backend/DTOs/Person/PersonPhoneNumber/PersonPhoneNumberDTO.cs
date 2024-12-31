using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace backend.DTOs.Person.PersonPhoneNumber
{
    public class PersonPhoneNumberDTO
    {
        public Guid PersonPhoneNumberId { get; set; }
        public Guid PersonId { get; set; }
        public Guid NationalCallingCodeCountryId { get; set; }
        public string? NationalCallingCode { get; set; }
        public string? NationalCallingCodeCountryName { get; set; }
        public string PhoneNumber { get; set; }



    }
}