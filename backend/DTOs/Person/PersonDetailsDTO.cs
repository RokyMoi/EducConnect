using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonDetailsDTO
    {

        public Guid PersonDetailsId { get; set; }
        public Guid PersonId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        

        public Guid? CountryOfOriginCountryId { get; set; }
    }
}




