using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEmailDTO
    {
        public Guid PersonId { get; set; }
        public Guid PersonEmailId { get; set; }
        public EduConnect.Entities.Person.Person? Person { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }

    }
}