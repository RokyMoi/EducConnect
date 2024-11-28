using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonSaltDTO
    {
        public Guid PersonSaltId { get; set; }
        public Guid PersonId { get; set; }
        public string Salt { get; set; }
        public int NumberOfRounds { get; set; }
    }
}