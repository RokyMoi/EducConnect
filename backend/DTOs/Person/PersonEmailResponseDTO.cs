using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEmailResponseDTO
    {
        public Guid PersonEmailId { get; set; }
        public string Email { get; set; }
    }
}