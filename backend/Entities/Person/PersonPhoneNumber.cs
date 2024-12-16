using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Entities.Person
{
    [Table("PersonPhoneNumber", Schema = "Person")]
    [Index(nameof(PersonId), IsUnique = true)]
    public class PersonPhoneNumber
    {
        [Key]
        public Guid PersonPhoneNumberId { get; set; }

        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public EduConnect.Entities.Person.Person Person { get; set; }

        //Reference to Country table, which stores the country code for the phone number
        public Guid NationalCallingCodeCountryId { get; set; }

        [ForeignKey("NationalCallingCodeCountryId")]
        public Reference.Country.Country Country { get; set; }

        public string PhoneNumber { get; set; }

        public long CreatedAt { get; set; } = new DateTimeOffset().ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; }


    }
}