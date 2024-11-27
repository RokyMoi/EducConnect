using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Utilities;
using EduConnect.Entities.Person;

namespace backend.Entities.Person
{
    [Table("PersonVerificationCode", Schema = "Person")]
    public class PersonVerificationCode
    {
        [Key]
        public Guid PersonVerificationCodeId { get; set; }
        public Guid PersonId { get; set; }

        //Navigation property
        [ForeignKey(nameof(PersonId))]
        public EduConnect.Entities.Person.Person Person { get; set; }

        public string VerificationCode { get; set; } = string.Empty;
        public long ExpiryDateTime { get; set; } = new DateTimeOffset(DateTime.UtcNow.AddMinutes(15)).ToUnixTimeMilliseconds();

        public bool IsVerified { get; set; } = false;

        //Token expires in 15 minutes from the moment of it's creation in database
        public long CreatedAt { get; set; } = new DateTimeOffset().ToUnixTimeMilliseconds();
        public long? ModifiedAt { get; set; }

    }
}