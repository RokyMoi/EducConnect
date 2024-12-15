using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person.PersonPhoneNumber
{
    public class PersonPhoneNumberSaveRequestDTO
    {
        public Guid NationalCallingCodeCountryId { get; set; }
        public string PhoneNumber { get; set; }
    }
}