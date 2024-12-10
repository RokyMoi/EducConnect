using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference
{
    public class PersonPhoneNumberDTO
    {
        public Guid PersonId { get; set; }
        public Guid PersonDetailsId { get; set; }
        public string NationalCallingCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}