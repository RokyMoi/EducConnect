using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Country
{
    public class CountryResponseDTO
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public string NationalCallingCode { get; set; }
        public string ShorthandCode { get; set; }
        public string FlagEmoji { get; set; }

    }
}