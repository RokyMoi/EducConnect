using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonPasswordDTO
    {
        public Guid PersonPasswordId { get; set; }
        public Guid PersonId { get; set; }
        public byte[] Hash { get; set; } = [];
        public byte[] Salt { get; set; } = [];
    }
}