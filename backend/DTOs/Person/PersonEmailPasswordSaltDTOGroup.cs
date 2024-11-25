using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Person;
namespace backend.DTOs.Person
{
    public class PersonEmailPasswordSaltDTOGroup
    {
        public PersonDTO PersonDTO { get; set; }
        public PersonEmailDTO PersonEmailDTO { get; set; }
        public PersonPasswordDTO PersonPasswordDTO { get; set; }
        public PersonSaltDTO PersonSaltDTO { get; set; }

    }
}