using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonEmailWithPersonObjectDTO
    {
        public Guid PersonEmailId { get; set; }
        public Guid PersonId { get; set; }

        public EduConnect.Entities.Person.Person Person { get; set; }
        public string Email { get; set; }


    }
}